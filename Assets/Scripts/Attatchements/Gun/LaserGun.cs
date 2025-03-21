using UnityEngine;
using UnityEngine.InputSystem;

public class LaserGun : Attatchement
{
    #region Variables

    [SerializeField] private int _damage = 1;
    [SerializeField] private float _damageSpeed = 0.3f;
    [SerializeField] private float _maxRange = 15f;
    [SerializeField] private Vector2 offset = Vector2.zero;

    [SerializeField] private LineRenderer _lineRenderer;

    private GameInputMap _input;
    private InputAction _fireInput;

    private bool _canFire = false;

    #endregion

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
        if (_canFire) {
            if (_fireInput.WasPressedThisFrame()) {
                _lineRenderer.enabled = true;
            }
            if (_fireInput.WasReleasedThisFrame()) {
                _lineRenderer.enabled = false;
            }

            if (_fireInput.IsPressed()) {
                UpdateLineRenderer();
            }
            
        } else {
            _lineRenderer.enabled = false;
        }
    }

    private void UpdateLineRenderer() {
        if (_lineRenderer != null) {
            Vector3 startPos = transform.TransformVector(new Vector3(offset.x, offset.y, 0))
            _lineRenderer.SetPosition(0, startPos);
            //RaycastHit2D hit = Physics2D.Raycast();

        }
    }
}
