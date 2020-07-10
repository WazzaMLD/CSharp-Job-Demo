/*
 * (c) Copyright 2020 Lokel Digital Pty Ltd.
 * https://www.lokeldigital.com
 * 
 * This Lokel package can be used under the Creative Commons License AU by Attribution
 * https://creativecommons.org/licenses/by/3.0/au/legalcode
 */


using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

using Lokel.Util;

namespace Lokel.Sinusoidal
{

    [AddComponentMenu("Lokel/Sinusoidal Tiles Controller")]
    public class SinusoidalTilesController : MonoBehaviour
    {
        [SerializeField]
        private GameObject Template = null;

        [SerializeField]
        private SinusoidalMasterParams _Params = SinusoidalMasterParams.Defaults();

        private Transform[] _Transforms;
        private TransformAccessArray _NativeTransforms;

        private JobHandle _Handle;

        private void Awake()
        {
            _Handle = new JobHandle();

            if (Template != null)
            {
                var transformsAndPositions = Factory.CreatePlaneFromObjectTemplate(
                    Template,
                    _Params.Size
                );
                _Transforms = transformsAndPositions.transforms;
                _NativeTransforms = new TransformAccessArray(_Transforms);
            }
        }

        void Update()
        {
            if (_Transforms != null && _Handle.IsCompleted)
            {
                _Handle = SinusoidalTilesJob.Begin(_Params, _NativeTransforms);
            }
        }

        private void OnDestroy()
        {
            if (_NativeTransforms.isCreated) _NativeTransforms.Dispose();
        }
    }

}