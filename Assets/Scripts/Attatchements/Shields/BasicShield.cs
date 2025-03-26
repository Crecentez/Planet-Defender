using UnityEngine;
using UnityEngine.InputSystem;

public class BasicShield : Shield
{

    #region Varaibles

    
    [SerializeField] private float _regenTime = 5f;
    [SerializeField] private float _regenRate = 5f;

    private GameInputMap _input;
    private InputAction _regenInput;

    private float _regenHealth;

    private bool _isHealing;

    #endregion

    #region Unity Methods

    private void OnEnable() {
        _input = new GameInputMap();
        _input.Shield.Enable();
        _regenInput = _input.Shield.Regenerate;
        SetHealth(_maxHealth);
        _isHealing = false;
    }

    private void OnDisable() {
        _input.Shield.Disable();
        _input = null;
        _isHealing = false;
    }

    private void Update() {
        if (_regenInput.WasPressedThisFrame()) {
            _isHealing = true;
        }
    }

    private void FixedUpdate() {
        if (_isHealing) {
            _regenHealth += _regenRate * Time.fixedDeltaTime;
            if (_regenHealth >= _maxHealth) {
                _regenHealth = _maxHealth;
                SetHealth(_maxHealth);
                _isHealing = false;
            } else {
                Heal(Mathf.CeilToInt(_regenHealth));
            }

        }
    }

    #endregion

}
