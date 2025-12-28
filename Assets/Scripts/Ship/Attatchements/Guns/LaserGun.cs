using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ship.Attachments.Guns {
    [RequireComponent(typeof(LineRenderer))]
    public class LaserGun : Gun {

        #region Variables

        [Header("Gun-Specific Settings")]
        [SerializeField] private int Damage = 3;
        [SerializeField] private float DamageSpeed = 0.2f;
        [SerializeField] private float MaxRange = 15f;
        [SerializeField] private Vector2 Offset = Vector2.zero;
        [SerializeField] private LayerMask Mask = -1;

        

        private Enemy _enemy;
        private LineRenderer _lineRenderer;
        private bool _canFire = false;
        private bool _canDamage = false;

        #endregion

        #region Unity Methods

        protected override void OnEnable() {
            base.OnEnable();

            _lineRenderer = GetComponent<LineRenderer>();

            _canFire = true;
            _canDamage = true;
        }

        private void Update() {
            if (_canFire) {
                if (_fireInput.WasPressedThisFrame()) {
                    _lineRenderer.enabled = true;
                }
                if (_fireInput.WasReleasedThisFrame()) {
                    _lineRenderer.enabled = false;
                }

                if (_fireInput.IsPressed()) {
                    UpdateLineRenderer();
                    if (_canDamage) {
                        StartCoroutine(DoDamage());
                    }
                }

            } else {
                _lineRenderer.enabled = false;
            }
        }

        #endregion

        #region Methods

        private IEnumerator DoDamage() {
            if (_canDamage && _enemy != null) {
                _canDamage = false;
                _enemy.Damage(Damage);
                yield return new WaitForSeconds(DamageSpeed);
                _canDamage = true;
            }
        }

        private void UpdateLineRenderer() {
            if (_lineRenderer != null) {
                Vector3 startPos = transform.TransformPoint(new Vector3(Offset.x, Offset.y, 0));
                _lineRenderer.SetPosition(0, startPos);
                RaycastHit2D hit = Physics2D.Raycast(startPos, transform.up, MaxRange, Mask.value);
                if (hit) {
                    Enemy e = hit.collider.gameObject.GetComponent<Enemy>();
                    if (e) _enemy = e;
                    _lineRenderer.SetPosition(1, hit.point);
                } else {
                    if (_enemy != null) _enemy = null;
                    _lineRenderer.SetPosition(1, transform.position + transform.up * MaxRange);
                }
            }
        }

        #endregion
    }
}
