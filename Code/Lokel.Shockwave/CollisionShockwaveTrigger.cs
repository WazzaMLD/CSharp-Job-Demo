/*
 * (c) Copyright 2020 Lokel Digital Pty Ltd.
 * https://www.lokeldigital.com
 * 
 * This Lokel package can be used under the Creative Commons License AU by Attribution
 * https://creativecommons.org/licenses/by/3.0/au/legalcode
 */

using UnityEngine;

using Unity.Mathematics;
using System;

namespace Lokel.Shockwave
{

    /// <summary>
    /// Should be added to the prefab for the blocks making up the area where the
    /// shockwave can take place and be triggered by a collision.
    /// </summary>
    [AddComponentMenu("Lokel/Collision Shockwave Trigger")]
    [RequireComponent(typeof(Collision))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(AudioSource))]
    public class CollisionShockwaveTrigger : MonoBehaviour
    {
        private ShockwavePhysics _Controller = null;

        private Transform _Transform;
        private AudioSource _Audio;
        private ShockwaveData _Shockwave;

        public void SetController(ShockwavePhysics controller)
        {
            _Controller = controller;
        }

        private void Awake()
        {
            ConfigureForShockwaveReaction();
        }

        private ShockwavePhysics OnDemandGetController()
        {
            if (_Controller == null)
                _Controller = GameObject.FindObjectOfType<ShockwavePhysics>();
            return _Controller;
        }

        private void OnCollisionEnter(Collision collision)
        {
            var whoBumpedMe = collision.collider.gameObject;

            SetPosition(GetBlockPosition());
            UseController( controller => controller.InjectShockwave(_Shockwave) );
            MakeSoundIfAvailable();
            GameObject.Destroy(whoBumpedMe);
        }

        private void MakeSoundIfAvailable()
        {
            if (_Audio.clip != null)
            {
                _Audio.Play();
            }
        }

        private void UseController(Action<ShockwavePhysics> usage)
        {
            var controller = OnDemandGetController();
            if (controller != null) usage(controller);
        }

        private void SetPosition(Vector3 position)
        {
            _Shockwave.Position = new float2(position.x, position.z);
        }

        private Vector3 GetBlockPosition() => _Transform.position;

        private void ConfigureForShockwaveReaction()
        {
            _Transform = GetComponent<Transform>();
            _Audio = GetComponent<AudioSource>();
            ConfigureShockwave();
            MakeObjectPhysicsUnderScriptControl();
        }

        private void ConfigureShockwave()
        {
            var pos = _Transform.position;
            _Shockwave = ShockwaveData.Create(
                new float2(pos.x, pos.z),
                0
            );
        }

        private void MakeObjectPhysicsUnderScriptControl()
        {
            var rigidBody = GetComponent<Rigidbody>();
            rigidBody.isKinematic = true;
        }
    }
}
