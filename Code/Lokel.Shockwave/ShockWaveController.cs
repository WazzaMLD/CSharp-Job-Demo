/*
 * (c) Copyright 2020 Lokel Digital Pty Ltd.
 * https://www.lokeldigital.com
 * 
 * LokelPackage can be used under the Creative Commons License AU by Attribution
 * https://creativecommons.org/licenses/by/3.0/au/legalcode
 */


using Lokel.Util;
using Lokel.Collections;

using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;

namespace Lokel.Shockwave
{

    [AddComponentMenu("Lokel/Shockwave Controller")]
    public class ShockWaveController : MonoBehaviour
    {
        [SerializeField]
        private GameObject Template = null;

        [SerializeField]
        private ShockMasterParams _Params = ShockMasterParams.Defaults();

        private NativeArray<ShockwaveData> _ShockwaveCells;
        private NativeQueue<ShockwaveData> _Centres;

        private Transform[] _Transforms;
        private float2[] _Positions;
        private TransformAccessArray _NativeTransforms;

        private JobHandle _Handle;
        private JobHandle _CentresHandle;

        public void OnPressMakeRandomShockwaveCentre()
        {
            float x = UnityEngine.Random.Range(0f, _Params.Size.x);
            float y = UnityEngine.Random.Range(0f, _Params.Size.y);

            ShockwaveData centre = ShockwaveData.Create(
                x, y, -_Params.HeightFactor, math.PI / 2
            );
            _CentresHandle = QueueJob<ShockwaveData>.AsyncEnqueue(_Centres, centre, _CentresHandle);
        }

        private void Awake()
        {
            _Handle = new JobHandle();
            _CentresHandle = new JobHandle();

            if (Template != null)
            {
                var transformsAndPositions = Factory.CreatePlaneFromObjectTemplate(
                    Template,
                    _Params.Size
                );
                _Transforms = transformsAndPositions.transforms;
                _Positions = transformsAndPositions.positions;
                _NativeTransforms = new TransformAccessArray(_Transforms);
            }
            CreateShockwaveData();
            PopulateShockwaveData();
        }

        void Update()
        {
            if (_Transforms != null)
            {
                if (IsCentresQueueComputed())
                {
                    ClearCentreIfDampedOut();
                    UpdateActiveShockwaveCentre();
                }
                JobHandle simHandle = NextSimRun();
                _Handle = MoveShockwaveCells(simHandle);
            }
        }

        private void OnDestroy()
        {
            JobHandle.ScheduleBatchedJobs();
            _Handle.Complete();
            _CentresHandle.Complete();
            DestroyShockwaveData();
            DestroyTransforms();
        }

        class SimData
        {
            public ShockMasterParams Params;
            public NativeArray<ShockwaveData> Cells;
            public JobHandle PriorJob;
        }

        private bool IsCentresQueueComputed()
        {
            bool isOK = _CentresHandle.IsCompleted;
            if (isOK) _CentresHandle.Complete();
            return isOK;
        }

        private JobHandle NextSimRun()
        {
            SimData data = new SimData()
            {
                Params = _Params,
                Cells = _ShockwaveCells,
                PriorJob = _Handle
            };

            _CentresHandle.Complete();
            _Centres.VisitAllItemsNewestToOldest<SimData>(data,
                (ShockwaveData centre, SimData sim) => {
                sim.PriorJob = ShockwaveSimJob.Begin(
                    centre,
                    sim.Params,
                    sim.Cells,
                    sim.PriorJob
                );
            });
            return ShockwaveSuperpositionJob.Begin(_ShockwaveCells, _Params, data.PriorJob);
        }

        private JobHandle MoveShockwaveCells(JobHandle simHandle)
            => ShockReactionJob.Begin(_ShockwaveCells, _NativeTransforms, simHandle);

        private void ClearCentreIfDampedOut()
        {
            if (_Centres.TryDequeue(out var centre))
            {
                if (centre.WaveTime < _Params.Cutoff)
                {
                    _Centres.TryEnqueue(centre);
                }
            }
        }

        private void UpdateActiveShockwaveCentre()
        {
            _CentresHandle = ShockwaveCentreSimJob.Begin(
                _Centres, _Params, _CentresHandle
            );
        }

        private void CreateShockwaveData()
        {
            _ShockwaveCells = CreateDataArray();
            _Centres = new NativeQueue<ShockwaveData>(
                _ShockwaveCells.Length, default, Allocator.Persistent
            );
        }

        private void PopulateShockwaveData()
        {
            for(int index = 0; index < _Positions.Length; index++)
            {
                _ShockwaveCells[index] = ShockDataExt.FromParts(_Positions[index], 0, 0);
            }
        }

        private NativeArray<ShockwaveData> CreateDataArray()
            => new NativeArray<ShockwaveData>(
                _Params.Size.x * _Params.Size.y, Allocator.Persistent
            );

        private void DestroyShockwaveData()
        {
            if (_ShockwaveCells.IsCreated) _ShockwaveCells.Dispose();
            if (_Centres.IsCreated) _Centres.Dispose();
        }

        private void DestroyTransforms()
        {
            if (_NativeTransforms.isCreated) _NativeTransforms.Dispose();
        }
    }

}