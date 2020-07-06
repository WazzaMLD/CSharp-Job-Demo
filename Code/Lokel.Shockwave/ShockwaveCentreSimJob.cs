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
    public struct ShockwaveCentreSimJob : IQueueIterator<float4>
    {
        public static JobHandle Begin(
            NativeQueue<float4> centres,
            ShockMasterParams settings,
            JobHandle dependency = default
        )
        {
            var iterator = new ShockwaveCentreSimJob()
            {
                deltaTime = Time.deltaTime,
                Params = settings
            };
            return QueueJob<float4>.AsycIterate(centres, iterator, dependency);
        }

        public ShockMasterParams Params;
        public float deltaTime;
        private NativeArray<float4> _Data;
        private OutValue<int> _Count;

        public NativeArray<float4> Store { set => _Data = value; }
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
            float height;
            float4 centre = _Data[index];
            float angle = centre.w;
            float2 pos = new float2(centre.x, centre.y);

            angle += deltaTime * math.PI;

            float diminishing = ShockDataExt.DiminishingFactor(Params, angle);

            height = (diminishing * Params.HeightFactor * math.sin(angle) ).ZeroIfSmall();
            _Data[index] = new float4(pos.x, pos.y, height, angle);
        }
    }

}
