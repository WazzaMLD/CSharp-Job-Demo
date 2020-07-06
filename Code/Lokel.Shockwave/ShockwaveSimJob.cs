/*
 * (c) Copyright 2020 Lokel Digital Pty Ltd.
 * https://www.lokeldigital.com
 * 
 * LokelPackage can be used under the Creative Commons License AU by Attribution
 * https://creativecommons.org/licenses/by/3.0/au/legalcode
 */

using Unity.Jobs;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Burst;

using Lokel.Util;

namespace Lokel.Shockwave
{
    /// <summary>Simulates the movement of cells surrounding the shockwave centre.</summary>
    [BurstCompile]
    public struct ShockwaveSimJob : IJobParallelFor
    {
        public NativeArray<ShockwaveData> Cells;

        [ReadOnly]
        public ShockwaveData Centre;

        [ReadOnly]
        public ShockMasterParams Params;

        public static JobHandle Begin(
            ShockwaveData centre,
            ShockMasterParams masterParams,
            NativeArray<ShockwaveData> cells,
            JobHandle dependency
        )
        {
            ShockwaveSimJob job = new ShockwaveSimJob()
            {
                Cells = cells,
                Centre = centre,
                Params = masterParams
            };
            return IJobParallelForExtensions.Schedule(
                job,
                cells.Length,
                ShockDataExt.GetInnerLoopCountFromSize(masterParams),
                dependency
            );
        }

        public void Execute(int index)
        {
            if (IsCentre(index))
                UpdateCentre(index);
            else
                ProcessWithCentre(index);
        }

        private bool IsCentre(int index)
        => IndexTool.ToIndex(Centre.Position, Params.Size) == index;

        private void UpdateCentre(int index)
        {
            Cells[index] = new ShockwaveData()
            {
                Position = Centre.Position,
                WaveTime = Centre.WaveTime,
                Height = Cells[index].Height,
                AdditionalHeight = Centre.Height,
                NumberCentres = 1
            };
        }

        private void ProcessWithCentre(int index)
        {
            float2 cellPos = Cells[index].Position;
            float cellHeight = Cells[index].Height;
            float time = Cells[index].WaveTime;
            int numCentres = Cells[index].NumberCentres;

            float distance = ShockDataExt.Distance(cellPos, Centre.Position);

            float additionalHeight = Cells[index].AdditionalHeight;

            if (distance < Params.InfluenceRadius)
            {
                additionalHeight = (Centre.Height / (1 + distance)).ZeroIfSmall();
            }

            Cells[index] =
                new ShockwaveData()
                {
                    Position = cellPos,
                    Height = cellHeight,
                    WaveTime = time,
                    NumberCentres = numCentres + 1,
                    AdditionalHeight = additionalHeight
                };
        }
    }

}
