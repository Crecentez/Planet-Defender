using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables Classes

    [Serializable]
    public class ControlsClass {

        public KeyCode ForwardGas = KeyCode.W;
        public KeyCode BackwardGas = KeyCode.S;
        public KeyCode SteerLeft = KeyCode.A;
        public KeyCode SteerRight = KeyCode.D;
        public KeyCode Boost = KeyCode.LeftShift;

        public KeyCode Fire = KeyCode.Mouse0;
        
    }

    [Serializable]
    public class MovementClass {

        [Header("Speed")]
        public float MaxSpeed = 5f;
        public float SteerSpeed = 1f;

        public float BoostSpeed = 3f;

        public int MaxBoost = 5000;
        public int Boost = 5000;
        

        [Header("Acceleration")]
        public float Acceleration = 1f;
        public float BoostAcceleration = 3f;

    }

    



    #endregion

    #region Variables

    public ControlsClass Controls = new ControlsClass();
    public MovementClass Movement = new MovementClass();

    [Header("Test Variables")]

    public Gun LeftGun;
    public Gun RightGun;

    private Rigidbody2D rb;
    private PlayerController pc;

    #endregion

    private void Start()
    {
        Movement.Boost = Movement.MaxBoost;
        rb = GetComponent<Rigidbody2D>();
        pc = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (!pc.IsDead) {
            updateMovement();
            updateFiring();
        }
        
    }

    private void updateFiring() 
    {
        
        if (LeftGun.BulletPrefab == RightGun.BulletPrefab) {
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1)) {
                LeftGun.Fire();
                RightGun.Fire();
            }
        } else {
            if (Input.GetMouseButton(0)) {

                LeftGun.Fire();
            }
            if (Input.GetMouseButton(1)) {

                RightGun.Fire();
            }
        }
        

    }


    private void updateMovement()
    {

        int steerDir = getXAxisInput();
        gameObject.transform.eulerAngles = new Vector3(0, 0, gameObject.transform.eulerAngles.z + Movement.SteerSpeed * Time.deltaTime * steerDir);

        float accel = Movement.Acceleration;
        bool isBoosting = false;

        if (Input.GetKey(Controls.Boost))
        {
            accel = Movement.BoostAcceleration;
            isBoosting = true;
        }

        if (Input.GetKey(Controls.ForwardGas))
        {
            rb.AddForce(new Vector2(gameObject.transform.up.x, gameObject.transform.up.y) * Time.deltaTime * accel);
        } else if (Input.GetKey(Controls.BackwardGas))
        {
            rb.AddForce(new Vector2(gameObject.transform.up.x, gameObject.transform.up.y) * Time.deltaTime * (accel * -0.75f));
        }

        if (isBoosting)
        {

        }
    }

    private int getXAxisInput()
    {

        int x = 0;

        if (Input.GetKey(Controls.SteerLeft)) { x += 1; }
        if (Input.GetKey(Controls.SteerRight)) { x -= 1; }

        return x;
    }
}
