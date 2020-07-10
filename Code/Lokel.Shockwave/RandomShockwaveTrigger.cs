/*
 * (c) Copyright 2020 Lokel Digital Pty Ltd.
 * https://www.lokeldigital.com
 * 
 * This Lokel package can be used under the Creative Commons License AU by Attribution
 * https://creativecommons.org/licenses/by/3.0/au/legalcode
 */

using UnityEngine;



namespace Lokel.Shockwave
{

    /// <summary>Add to a shockwave controller to inject randomly placed waves.</summary>
    [AddComponentMenu("Lokel/Random Shockwave Trigger")]
    [RequireComponent(typeof(ShockwaveController))]
    public class RandomShockwaveTrigger : MonoBehaviour
    {
        private ShockwaveController _Controller;

        private void Awake()
        {
            _Controller = GetComponent<ShockwaveController>();
        }

        public void OnPressMakeRandomShockwaveCentre()
        {
            if (_Controller != null)
            {
                (float width, float depth) = _Controller.GetSize();

                float x = Random.Range(0f, width);
                float y = Random.Range(0f, depth);

                ShockwaveData centre = ShockwaveData.Create(
                    x, y, _Controller.GetHeightFactor(), 0
                );
                _Controller.InjectShockwave(centre);
            }
        }

    }

}
