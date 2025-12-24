using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ship.Attachments.Guns {
    public class BasicGun : Attachment {

        #region Variables

        [SerializeField] private float FireRate = 0.5f;
        [SerializeField] private Vector2 Offset = Vector2.zero;
        [Tooltip("In Degrees")][SerializeField]
        private float Rotation = 0f;
        [SerializeField] private GameObject BulletPrefab;

        // Private
        private GameInputMap _input;
        private InputAction _fireInput;
        private bool _canFire = false;

        #endregion

        #region Unity Methods

        private void OnEnable() {
            _input = new GameInputMap();
            _input.Gun.Enable();
            _fireInput = _input.Gun.Fire;
            _canFire = true;
        }

        private void OnDisable() {
            _input.Gun.Disable();
            _input = null;
            _canFire = false;
        }


        private void Update() {
            if (_canFire && _fireInput.IsPressed()) {
                StartCoroutine(Fire());
            }
        }

        #endregion

        #region Methods

        public IEnumerator Fire() {
            if (_canFire) {
                _canFire = false;

                GameObject b = Instantiate(BulletPrefab);

                //GameObject player = GameObject.FindWithTag("Player");
                //player.GetComponent<Rigidbody2D>().AddForce(player.transform.up * b.GetComponent<Projectile>().PlayerKnockback * -1);
                b.transform.position = transform.position + (transform.up * Offset.y) + (transform.right * Offset.x);
                b.transform.rotation = new quaternion(transform.rotation.x + (Mathf.Deg2Rad * Rotation), transform.rotation.y, transform.rotation.z, transform.rotation.w);

                yield return new WaitForSeconds(FireRate);
                _canFire = true;
            }
        }


        #endregion

        #region Gizmos

        private void OnDrawGizmosSelected() {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(settings.offset, 0.02f);
        }

        #endregion
    }
}
