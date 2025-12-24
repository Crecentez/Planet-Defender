using UnityEngine;
using UnityEngine.InputSystem;

namespace Ship.Attachments.Shields {
    public class BasicShield : Shield {

        #region Varaibles

        [SerializeField] private float RegenTime = 5f;
        [SerializeField] private float RegenRate = 5f;

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
            SetHealth(MaxHealth);
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
                _regenHealth += RegenRate * Time.fixedDeltaTime;
                if (_regenHealth >= MaxHealth) {
                    _regenHealth = MaxHealth;
                    SetHealth(MaxHealth);
                    _isHealing = false;
                } else {
                    Heal(Mathf.CeilToInt(_regenHealth));
                }

            }
        }

        #endregion

    }
}
