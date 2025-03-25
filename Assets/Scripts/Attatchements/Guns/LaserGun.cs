using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LaserGun : Attatchement
{
    #region Variables

    [SerializeField] private int _damage = 3;
    [SerializeField] private float _damageSpeed = 0.2f;
    [SerializeField] private float _maxRange = 15f;
    [SerializeField] private Vector2 _offset = Vector2.zero;
    [SerializeField] private LayerMask _mask = -1;

    [SerializeField] private LineRenderer _lineRenderer;

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

    private IEnumerator DoDamage() {
        if (_canDamage && _enemy != null) {
            _canDamage = false;
            _enemy.Damage(_damage);
            yield return new WaitForSeconds(_damageSpeed);
            _canDamage = true;
        }
    }

    private void UpdateLineRenderer() {
        if (_lineRenderer != null) {
            Vector3 startPos = transform.TransformPoint(new Vector3(_offset.x, _offset.y, 0));
            _lineRenderer.SetPosition(0, startPos);
            RaycastHit2D hit = Physics2D.Raycast(startPos, transform.up, _maxRange, _mask.value);
            if (hit) {
                Enemy e = hit.collider.gameObject.GetComponent<Enemy>();
                if (e) _enemy = e;
                _lineRenderer.SetPosition(1, hit.point);
            } else {
                if (_enemy != null) _enemy = null;
                _lineRenderer.SetPosition(1, transform.position + transform.up * _maxRange);
            }
        }
    }
}
