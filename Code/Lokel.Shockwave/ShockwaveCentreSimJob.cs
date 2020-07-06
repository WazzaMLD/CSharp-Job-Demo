/*
 * (c) Copyright 2020 Lokel Digital Pty Ltd.
 * https://www.lokeldigital.com
 * 
 * LokelPackage can be used under the Creative Commons License AU by Attribution
 * https://creativecommons.org/licenses/by/3.0/au/legalcode
 */


using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Burst;

using Lokel.Util;

using Time = UnityEngine.Time;
using Lokel.Collections;


namespace Lokel.Shockwave
{

    /// <summary>Performs background updates on each centre.</summary>
    [BurstCompile]
    public struct ShockwaveCentreSimJob : IQueueIterator<ShockwaveData>
    {
        public static JobHandle Begin(
            NativeQueue<ShockwaveData> centres,
            ShockMasterParams settings,
            JobHandle dependency = default
        )
        {
            var iterator = new ShockwaveCentreSimJob()
            {
                deltaTime = Time.deltaTime,
                Params = settings
            };
            return QueueJob<ShockwaveData>.AsycIterate(centres, iterator, dependency);
        }

        public ShockMasterParams Params;
        public float deltaTime;
        private NativeArray<ShockwaveData> _Data;
        private OutValue<int> _Count;

        public NativeArray<ShockwaveData> Store { set => _Data = value; }
        public OutValue<int> Count { set => _Count = value; }

        public void Execute()
        {
            for(int index = 0; index < _Count.Value; index++)
            {
                Iterate(index);
            }
        }

        private void Iterate(int index)
        {
            ShockwaveData centre = _Data[index];

            float height;
            float time = centre.WaveTime;
            float2 pos = centre.Position;


            time += deltaTime;

            float diminishing = ShockDataExt.DiminishingFactor(Params, time);

            height = (diminishing * Params.HeightFactor * math.sin(centre.Angle)).ZeroIfSmall();

            _Data[index] = new ShockwaveData()
            {
                Position = pos,
                Height = height,
                WaveTime = time,
                NumberCentres = index
            };
        }
    }

}
