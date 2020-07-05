/*
 * (c) Copyright 2020 Lokel Digital Pty Ltd.
 * https://www.lokeldigital.com
 * 
 * LokelPackage can be used under the Creative Commons License AU by Attribution
 * https://creativecommons.org/licenses/by/3.0/au/legalcode
 */

using Unity.Jobs;
using UnityEngine.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Lokel.Sinusoidal
{
    internal struct SinusoidalTilesJob : IJobParallelForTransform
    {
        public float time;
        public SinusoidalMasterParams Params;

        public void Execute(int index, TransformAccess transform)
        {
            float3 pos = transform.position;
            float angle = Params.OffsetFactor * index + time * math.PI * Params.Speed;
            pos.y = math.sin(angle);
            transform.position = pos;
        }

        public static JobHandle Begin(
            SinusoidalMasterParams master,
            TransformAccessArray transforms
        )
        {
            SinusoidalTilesJob job = new SinusoidalTilesJob()
            {
                time = Time.realtimeSinceStartup,
                Params = master
            };

            return IJobParallelForTransformExtensions.Schedule(job, transforms);
        }
    }
}
