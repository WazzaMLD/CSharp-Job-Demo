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
        private bool IsNoCentre;

        [ReadOnly]
        public ShockMasterParams Params;

        public static JobHandle Begin(
            ShockwaveData centre,
            ShockMasterParams masterParams,
            NativeArray<ShockwaveData> cells,
            JobHandle dependency
        )
        {
            float angle = centre.Angle();

            ShockwaveSimJob job = new ShockwaveSimJob()
            {
                Cells = cells,
                Centre = centre,
                IsNoCentre = !(angle * angle > 0f),
                Params = masterParams
            };
            return IJobParallelForExtensions.Schedule(
                job,
                cells.Length,
                GetInnerLoopCountFromSize(masterParams.Size),
                dependency
            );
        }

        private static int GetInnerLoopCountFromSize(float2 size) => (int)size.x;

        public void Execute(int index)
        {
            if (IsNoCentre)
                ProcessNoCentre(index);
            else
            {
                if (IsCentre(index))
                    UpdateCentre(index);
                else
                    ProcessWithCentre(index);
            }
        }

        private bool IsCentre(int index)
        => IndexTool.ToIndex(Centre.Position(), Params.Size) == index;

        private void UpdateCentre(int index)
        {
            Cells[index] = Centre;
        }

        private void ProcessWithCentre(int index)
        {
            float2 cellPos = Cells[index].Position();
            float cellHeight = Cells[index].Height();
            float cellAngle = Cells[index].Angle();
            int numCentres = Cells[index].NumberCentres;
            float totalHeight = Cells[index].TotalHeight;

            float distance = ShockDataExt.Distance(cellPos, Centre.Position());

            float additionalHeight;
            if (distance < Params.InfluenceRadius)
            {
                additionalHeight = 
                    (Centre.Height() / (1 + distance)).ZeroIfSmall();
                cellHeight =
                    (ShockDataExt.DiminishingFactor(Params, Centre.Angle())
                    * cellHeight).ZeroIfSmall();
            }
            else
            {
                additionalHeight = 0;
            }

            cellHeight += (additionalHeight);

            Cells[index] =
                new ShockwaveData()
                {
                    Position = cellPos,
                    Height = cellHeight,
                    Angle = cellAngle,
                    NumberCentres = numCentres + 1,
                    TotalHeight = totalHeight + additionalHeight
                };
        }

        private void ProcessNoCentre(int index)
        {
            float cellHeight = Cells[index].Height();

            cellHeight = (
                    ShockDataExt.DiminishingFactor(Params, Centre.Angle()) * cellHeight
                ).ZeroIfSmall();

            Cells[index] = ShockwaveData.Create(
                Cells[index].Position(),
                cellHeight,
                Cells[index].Angle()
            );
        }
    }

}
