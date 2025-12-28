using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ship.Attachments.Guns {

    public class BasicGun : Gun {

        #region Variables

        [Header("Gun-Specific Settings")]
        [SerializeField] private float FireRate = 0.5f;
        [SerializeField] private Vector2 Offset = Vector2.zero;
        [Tooltip("In Degrees")][SerializeField]
        private float Rotation = 0f;
        [SerializeField] private GameObject BulletPrefab;

        // Private
        private bool _canFire = false;

        #endregion

        #region Unity Methods

        protected override void OnEnable() {
            base.OnEnable();

            _canFire = true;
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
            Gizmos.DrawWireSphere(Offset, 0.02f);
        }

        #endregion
    }
}
