using UnityEngine;
using UnityEngine.InputSystem;

public class BasicShield : Shield
{

    #region Varaibles

    [SerializeField] private int _maxHealth = 50;
    [SerializeField] private float _regenTime = 5f;
    [SerializeField] private float regenRate = 5f;

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
        _health = _maxHealth;
        _isHealing = false;
    }

    private void OnDisable() {
        _input.Shield.Disable();
        _input = null;
        _isHealing = false;
    }

    private void FixedUpdate() {
        if (_isHealing) {
            _regenHealth += regenRate * Time.fixedDeltaTime;
            if (_regenHealth >= _maxHealth) {
                _regenHealth = _maxHealth;
                _health = _maxHealth;
                _isHealing = false;
            } else {
                _health = Mathf.CeilToInt(_regenHealth);
            }

        }
    }

    #endregion

}
