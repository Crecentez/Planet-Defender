using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerController))]
public class PlayerMovement : MonoBehaviour
{

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
    [SerializeField] private InputHandler _input;
    [SerializeField] private PlayerController _controller;
    [SerializeField] private Rigidbody2D _rb;

    private InputAction _moveInput;
    private InputAction _boostInput;

    private float _speedMultiplier = 1f;

    #endregion

    #region Unity Methods

    private void OnEnable() {
        InputSystemActions.Instance.Enable();
        _moveInput = InputSystemActions.Instance.Player.Move;
        _boostInput = InputSystemActions.Instance.Player.Boost;
    }

    private void OnDisable() {
        InputSystemActions.Instance.Disable();
    }

    private void Update()
    {
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


    private void updateMovement()
    {

        Vector2 input = getMoveInput();

        gameObject.transform.eulerAngles = new Vector3(0, 0, gameObject.transform.eulerAngles.z + SteerSpeed * Time.deltaTime * input.x);

        float accel = Acceleration;
        if (_boostInput.IsPressed()) {
            accel = BoostAcceleration;
        }

        if (input.y > 0) {
            _rb.AddForce(new Vector2(gameObject.transform.up.x, gameObject.transform.up.y) * Time.deltaTime * accel);
        } else if (input.y < 0) {
            _rb.AddForce(new Vector2(gameObject.transform.up.x, gameObject.transform.up.y) * Time.deltaTime * (accel * -0.75f));
        }

        //int steerDir = getXAxisInput();
        //gameObject.transform.eulerAngles = new Vector3(0, 0, gameObject.transform.eulerAngles.z + SteerSpeed * Time.deltaTime * steerDir);

        //float accel = Acceleration;

        //if (_input.Boost.GetKey())
        //    accel = BoostAcceleration;

        //if (_input.ForwardGas.GetKey())
        //{
        //    _rb.AddForce(new Vector2(gameObject.transform.up.x, gameObject.transform.up.y) * Time.deltaTime * accel);
        //} else if (_input.BackwardGas.GetKey())
        //{
        //    _rb.AddForce(new Vector2(gameObject.transform.up.x, gameObject.transform.up.y) * Time.deltaTime * (accel * -0.75f));
        //}
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
