/*
 * (c) Copyright 2020 Lokel Digital Pty Ltd.
 * https://www.lokeldigital.com
 * 
 * LokelPackage can be used under the Creative Commons License AU by Attribution
 * https://creativecommons.org/licenses/by/3.0/au/legalcode
 */



using Unity.Mathematics;

namespace Lokel.Util
{
    /// <summary>Embodies all assumptions about converting index <-> float2 position</summary>
    public static class IndexTool
    {
        public const int ILLEGAL_INDEX = -1;

        public static int ToIndex(float2 pos, float2 size)
        {
            if (pos.IsWithin(size))
            {
                int2 iSize = (int2)size;
                int2 iPos = (int2)pos;
                return (iPos.x * iSize.y) + iPos.y;
            }
            else
                return ILLEGAL_INDEX;
        }

        public static bool IsWithin(this float2 pos, float2 size)
            => 0 <= pos.x && pos.x < size.x
            && 0 <= pos.y && pos.y < size.y;
    }
}
