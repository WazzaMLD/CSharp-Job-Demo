/*
 * (c) Copyright 2020 Lokel Digital Pty Ltd.
 * https://www.lokeldigital.com
 * 
 * This Lokel package can be used under the Creative Commons License AU by Attribution
 * https://creativecommons.org/licenses/by/3.0/au/legalcode
 */

using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;

using Lokel.Util;
using UnityEngine;

namespace Lokel.Shockwave
{

    public struct ShockwaveSuperpositionJob : IJobParallelFor
    {
        public NativeArray<ShockwaveData> Cells;
        public ShockMasterParams Params;
        public float deltaTime;

        public static JobHandle Begin(
            NativeArray<ShockwaveData> cells,
            ShockMasterParams master,
            JobHandle dependency
        )
        {
            var job = new ShockwaveSuperpositionJob()
            {
                Cells = cells,
                Params = master,
                deltaTime = Time.deltaTime
            };
            return IJobParallelForExtensions.Schedule(
                job,
                cells.Length,
                ShockDataExt.GetInnerLoopCountFromSize(master),
                dependency
            );
        }

        public void Execute(int index)
        {
            float cellHeight = Cells[index].Height;
            float factor = ShockDataExt.DiminishingFactor(Params, deltaTime);

            cellHeight = (factor * cellHeight + Cells[index].AdditionalHeight).ZeroIfSmall();

            Cells[index] = ShockwaveData.Create(
                Cells[index].Position,
                cellHeight,
                Cells[index].WaveTime
            );
        }
    }

}
