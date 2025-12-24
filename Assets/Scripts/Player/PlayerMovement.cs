using UnityEngine;
using UnityEngine.InputSystem;


namespace Player {
    [RequireComponent(typeof(PlayerController))]
    public class PlayerMovement : MonoBehaviour {

        #region Variables

        // Constants

        public const float MaxSpeed = 5f;
        public const float SteerSpeed = 100f;
        public const float BoostSpeed = 3f;
        public const float Acceleration = 100f;
        public const float BoostAcceleration = 200f;
        public const float PlanetKnockback = 0.7f;

        // Public



        // Private
        [SerializeField] private PlayerController _controller;
        [SerializeField] private Rigidbody2D _rb;

        private GameInputMap _input;
        private InputAction _moveInput;
        private InputAction _boostInput;

        private float _speedMultiplier = 1f;

        #endregion

        #region Unity Methods

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
            if (!_controller.isDead) {
                updateMovement();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.tag == "Planet") {
                Vector3 dir3 = (collision.gameObject.transform.position - transform.position).normalized;
                Vector2 dir = new Vector2(dir3.x, dir3.y);
                GetComponent<Rigidbody2D>().linearVelocity = dir * PlanetKnockback * -1;
            }
        }

        #endregion

        #region Methods


        private void updateMovement() {

            Vector2 input = getMoveInput();

            gameObject.transform.eulerAngles = new Vector3(0, 0, gameObject.transform.eulerAngles.z + SteerSpeed * Time.deltaTime * input.x * -1);

            float accel = Acceleration;
            if (_boostInput.IsPressed()) {
                accel = BoostAcceleration;
            }

            if (input.y > 0) {
                _rb.AddForce(new Vector2(gameObject.transform.up.x, gameObject.transform.up.y) * Time.deltaTime * accel);
            } else if (input.y < 0) {
                _rb.AddForce(new Vector2(gameObject.transform.up.x, gameObject.transform.up.y) * Time.deltaTime * (accel * -0.75f));
            }
        }

        private Vector2 getMoveInput() {
            return _moveInput.ReadValue<Vector2>();
        }

        #endregion

        #region Upgrades

        public void MultiplySpeed(float multi) {
            _speedMultiplier *= multi;
        }

        #endregion
    }

}