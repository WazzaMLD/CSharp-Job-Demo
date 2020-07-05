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
        public NativeArray<float4> Cells;

        [ReadOnly]
        public float4 Centre;

        [ReadOnly]
        private bool IsNoCentre;

        [ReadOnly]
        public ShockMasterParams Params;

        public static JobHandle Begin(
            float4 centre,
            ShockMasterParams masterParams,
            NativeArray<float4> cells,
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
            float cellHeight = Cells[index].z;
            float cellAngle = Cells[index].w;

            float distance = ShockData.Distance(cellPos, Centre.Position());

            float additionalHeight 
                = distance < Params.InfluenceRadius
                ? (Centre.Height() / (1 + distance)).ZeroIfSmall()
                : 0;

            cellHeight = distance < Params.InfluenceRadius
                ? (ShockData.DiminishingFactor(Params,Centre.Angle()) * cellHeight).ZeroIfSmall()
                : cellHeight;

            cellHeight = cellHeight + additionalHeight;

            Cells[index] = new float4(cellPos.x, cellPos.y, cellHeight, cellAngle);
        }

        private void ProcessNoCentre(int index)
        {
            float cellHeight = Cells[index].Height();

            float diminishing = math.pow(
                (1 - Params.DecayRatePerSecond),
                Centre.TimeInSeconds()
            );

            cellHeight =  (cellHeight * diminishing).ZeroIfSmall();

            Cells[index] = new float4(
                Cells[index].Position(),
                cellHeight,
                Cells[index].Angle()
            );
        }
    }

}
