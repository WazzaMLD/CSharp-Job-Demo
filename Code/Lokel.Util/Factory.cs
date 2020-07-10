/*
 * (c) Copyright 2020 Lokel Digital Pty Ltd.
 * https://www.lokeldigital.com
 * 
 * This Lokel package can be used under the Creative Commons License AU by Attribution
 * https://creativecommons.org/licenses/by/3.0/au/legalcode
 */

using UnityEngine;
using Unity.Mathematics;

namespace Lokel.Util
{

    /// <summary>The birthing place for bulk GameObjects</summary>
    public static class Factory
    {
        public static (Transform[] transforms, float2[] positions)
            CreatePlaneFromObjectTemplate(GameObject template, int2 size)
        {
            Transform[] transforms = MakeArray<Transform>(size);
            float2[] positions = MakeArray<float2>(size);
            Populate(transforms, positions, template, size);
            return (transforms, positions);
        }

        private static void Populate(
            Transform[] transforms, float2[] positions,
            GameObject template,
            int2 size
        )
        {
            int2 NextRow = new int2(0, 1), NextCol = new int2(1, 0);

            for(int row = 0; row < size.y; row++)
            {
                for(int col = 0; col < size.x; col++)
                {
                    int2 pos = new int2(col, row);

                    var TransAndPos = CreateAndPlace(template, pos);
                    int index = IndexTool.ToIndex(pos, size);
                    transforms[index] = TransAndPos.transform;
                    positions[index] = TransAndPos.position;
                }
            }
        }

        private static (Transform transform, float2 position)  CreateAndPlace(GameObject template, int2 pos)
        {
            float3 position = new float3(pos.x, 0, pos.y);
            var gObject = GameObject.Instantiate(template, position, Quaternion.identity);
            return (gObject.transform, pos);
        }

        private static T[] MakeArray<T>(int2 size) => new T[size.x * size.y];
    }

}
