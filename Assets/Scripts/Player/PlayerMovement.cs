using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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

    private float _speedMultiplier = 1f;

    #endregion

    #region Unity Methods


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
            GetComponent<Rigidbody2D>().velocity = dir * PlanetKnockback * -1;
        }
    }

    #endregion

    #region Methods


    private void updateMovement()
    {

        int steerDir = getXAxisInput();
        gameObject.transform.eulerAngles = new Vector3(0, 0, gameObject.transform.eulerAngles.z + SteerSpeed * Time.deltaTime * steerDir);

        float accel = Acceleration;
        bool isBoosting = false;

        if (_input.Controls.Boost.GetKey())
        {
            accel = BoostAcceleration;
            isBoosting = true;
        }

        if (_input.Controls.ForwardGas.GetKey())
        {
            _rb.AddForce(new Vector2(gameObject.transform.up.x, gameObject.transform.up.y) * Time.deltaTime * accel);
        } else if (_input.Controls.BackwardGas.GetKey())
        {
            _rb.AddForce(new Vector2(gameObject.transform.up.x, gameObject.transform.up.y) * Time.deltaTime * (accel * -0.75f));
        }
    }

    private int getXAxisInput()
    {
        int x = 0;
        if (_input.Controls.SteerLeft.GetKey()) { x += 1; }
        if (_input.Controls.SteerRight.GetKey()) { x -= 1; }
        return x;
    }

    #endregion

    #region Upgrades

    public void MultiplySpeed(float multi) {
        _speedMultiplier *= multi;
    }

    #endregion
}
