using System.Collections;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Ship.Attachments.Guns {
    public class LaserGun : Attachment {

        #region Variables

        [SerializeField] private int Damage = 3;
        [SerializeField] private float DamageSpeed = 0.2f;
        [SerializeField] private float MaxRange = 15f;
        [SerializeField] private Vector2 Offset = Vector2.zero;
        [SerializeField] private LayerMask Mask = -1;

        [SerializeField] private LineRenderer LineRenderer;

        private GameInputMap _input;
        private InputAction _fireInput;
        private Enemy _enemy;
        private bool _canFire = false;
        private bool _canDamage = false;

        #endregion

        private void OnEnable() {
            _input = new GameInputMap();
            _input.Gun.Enable();
            _fireInput = _input.Gun.Fire;
            _canFire = true;
            _canDamage = true;
        }

        private void OnDisable() {
            _input.Gun.Disable();
            _input = null;
            _canFire = false;
            _canDamage = false;
        }

        private void Update() {
            if (_canFire) {
                if (_fireInput.WasPressedThisFrame()) {
                    LineRenderer.enabled = true;
                }
                if (_fireInput.WasReleasedThisFrame()) {
                    LineRenderer.enabled = false;
                }

                if (_fireInput.IsPressed()) {
                    UpdateLineRenderer();
                    if (_canDamage) {
                        StartCoroutine(DoDamage());
                    }
                }

            } else {
                LineRenderer.enabled = false;
            }
        }

        private IEnumerator DoDamage() {
            if (_canDamage && _enemy != null) {
                _canDamage = false;
                _enemy.Damage(Damage);
                yield return new WaitForSeconds(DamageSpeed);
                _canDamage = true;
            }
        }

        private void UpdateLineRenderer() {
            if (LineRenderer != null) {
                Vector3 startPos = transform.TransformPoint(new Vector3(Offset.x, Offset.y, 0));
                LineRenderer.SetPosition(0, startPos);
                RaycastHit2D hit = Physics2D.Raycast(startPos, transform.up, MaxRange, Mask.value);
                if (hit) {
                    Enemy e = hit.collider.gameObject.GetComponent<Enemy>();
                    if (e) _enemy = e;
                    LineRenderer.SetPosition(1, hit.point);
                } else {
                    if (_enemy != null) _enemy = null;
                    LineRenderer.SetPosition(1, transform.position + transform.up * MaxRange);
                }
            }
        }
    }
}
