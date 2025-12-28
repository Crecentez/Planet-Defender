using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using Damage;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

namespace Ship {
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collision2D))]
    [RequireComponent(typeof(Damageable))]
    //[RequireComponent(typeof(EventSystem))]
    //[RequireComponent(typeof(InputSystemUIInputModule))]
    //[RequireComponent(typeof(PlayerInput))]
    public class Ship : MonoBehaviour {

        #region Veriables

        // Public


        // Protected
        [Header("Input")]
        [SerializeField] private InputActionAsset InputActions;

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
        private Damageable _damageable;
        //private GameInputMap _input;
        private InputAction _moveInput;
        private InputAction _boostInput;

        #endregion

        #region Unity Methods

        private void Awake() {
            if (!InputActions) {
                Debug.LogError(gameObject.name + " does not have a InputActionAsset assigned!");
                return;
            }

            InputActionMap playerInputMap = InputActions.FindActionMap("Player");
            _moveInput = playerInputMap.FindAction("Move");
            _boostInput = playerInputMap.FindAction("Boost");
        }

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
            _damageable = GetComponent<Damageable>();
            if (!_damageable) {
                Debug.LogWarning("Damageable not found on " + gameObject.name);
            }
        }

        private void OnEnable() {
            //_input = new GameInputMap();
            //_input.Player.Enable();
            //_moveInput = _input.Player.Move;
            //_boostInput = _input.Player.Boost;
            _moveInput.Enable();
            _boostInput.Enable();
        }

        private void OnDisable() {
            //_input.Player.Disable();
            //_input = null;
            _moveInput.Disable();
            _boostInput.Disable();
        }

        private void Update() {
            if (!_damageable.IsDead()) {
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

        protected void UpdateMovement() {
            if (_damageable.IsDead()) return;
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

