using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ship {
    public delegate void HealthUpdated(int old, int current);
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collision2D))]
    public class Ship : MonoBehaviour {

        #region Veriables

        // Public


        // Protected
        [SerializeField] protected int MaxHealth = 1000;

        [Header("Movement")]
        [SerializeField] protected float MaxSpeed = 5f;
        [SerializeField] protected float SteerSpeed = 100f;
        [SerializeField] protected float Acceleration = 100f;
        [SerializeField] protected bool BoostEnabled = true;
        [SerializeField] protected float BoostSpeed = 3f;
        [SerializeField] protected float BoostAcceleration = 200f;
        [SerializeField] protected float PlanetKnockback = 0.7f;

        // Private
        private Rigidbody2D _rigidbody;
        private GameInputMap _input;
        private InputAction _moveInput;
        private InputAction _boostInput;
        private int _health = 0;
        private bool _isDead = false;

        // Events
        public event HealthUpdated OnHealthUpdated;
        public event Action OnKilled;

        #endregion

        #region Unity Methods

        private void Start() {
            GameObject vc = GameObject.FindGameObjectWithTag("VirtualCamera");
            if (vc) {
                CinemachineVirtualCamera cvc = vc.GetComponent<CinemachineVirtualCamera>();
                if (cvc) cvc.Follow = gameObject.transform;
            }
            _rigidbody = GetComponent<Rigidbody2D>();
            if (!_rigidbody) {
                Debug.LogWarning("Rigidbody not found on " + gameObject.name);
            }
            _health = MaxHealth;
        }

        private void OnEnable() {
            _input = new GameInputMap();
            _input.Player.Enable();
            _moveInput = _input.Player.Move;
            _boostInput = _input.Player.Boost;
        }

        private void OnDisable() {
            _input.Player.Disable();
            _input = null;
        }

        private void Update() {
            if (!IsDead()) {
                UpdateMovement();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.tag == "Planet") {
                Vector3 dir3 = (collision.gameObject.transform.position - transform.position).normalized;
                Vector2 dir = new Vector2(dir3.x, dir3.y);
                _rigidbody.linearVelocity = dir * PlanetKnockback * -1;
            }
        }

        #endregion

        #region Methods

        public void Damage(int damage) {
            int old = _health;
            _health -= damage;
            if (_health < 0) _health = 0;
            OnHealthUpdated?.Invoke(old, _health);
            if (_health <= 0) Kill();
        }

        public void Heal(int heal) {
            int old = _health;
            _health += heal;
            if (_health > MaxHealth) _health = MaxHealth;
            OnHealthUpdated?.Invoke(old, _health);
        }

        public void Kill() {
            if (_isDead) return;
            Debug.Log("Ship Killed");
            int old = _health;
            _health = 0;
            _isDead = true;
            OnHealthUpdated?.Invoke(old, _health);
            OnKilled?.Invoke();
        }

        public int GetMaxHealth() { return MaxHealth; }
        public int GetHealth() { return _health; }
        public bool IsDead() { return _isDead; }

        protected void UpdateMovement() {
            if (IsDead()) return;
            Vector2 input = GetMovementInput();

            gameObject.transform.eulerAngles = new Vector3(0, 0, gameObject.transform.eulerAngles.z + SteerSpeed * Time.deltaTime * input.x * -1);

            float accel = Acceleration;
            if (_boostInput.IsPressed()) {
                accel = BoostAcceleration;
            }

            if (input.y > 0) {
                _rigidbody.AddForce(new Vector2(gameObject.transform.up.x, gameObject.transform.up.y) * Time.deltaTime * accel);
            } else if (input.y < 0) {
                _rigidbody.AddForce(new Vector2(gameObject.transform.up.x, gameObject.transform.up.y) * Time.deltaTime * (accel * -0.75f));
            }
        }

        protected Vector2 GetMovementInput() {
            return _moveInput.ReadValue<Vector2>();
        }

        #endregion
    }
}

