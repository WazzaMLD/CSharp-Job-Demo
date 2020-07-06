/*
 * (c) Copyright 2020 Lokel Digital Pty Ltd.
 * https://www.lokeldigital.com
 * 
 * LokelPackage can be used under the Creative Commons License AU by Attribution
 * https://creativecommons.org/licenses/by/3.0/au/legalcode
 */


using Unity.Mathematics;

namespace Lokel.Shockwave
{

    public struct ShockwaveData
    {
        public float2 Position;
        public float Height;
        public float Angle;
        public int NumberCentres;
        public float TotalHeight;

        public static ShockwaveData Create(
            float2 position,
            float height,
            float angle
        )
        {
            return new ShockwaveData()
            {
                Position = position,
                Height = height,
                Angle = angle,
                NumberCentres = 0,
                TotalHeight = 0
            };
        }

        public static ShockwaveData Create(
            float x, float y,
            float height,
            float angle
        ) => Create(new float2(x, y), height, angle);
    }

}
