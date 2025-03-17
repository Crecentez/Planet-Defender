using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables Classes

    [Serializable]
    public class GunConnection {
        public Vector2 Position = Vector2.zero;
        public float Rotation = 0f;
        public Vector2 Attenuation = Vector2.zero;
        public float AttenuationRotation = 0f;

        public GameObject GunPrefab;
        [HideInInspector]
        public Gun Gun;
    }

    [Serializable]
    public class GunClass {
        public List<GunConnection> Guns = new List<GunConnection>();
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


        [Header("Knockback")]
        public float PlanetKnockback = 1f;

    }

    #endregion

    #region Variables

    // Public
    public MovementClass Movement = new MovementClass();
    public GunClass Guns = new GunClass();

    // Private
    private InputHandler inputHandler;
    private Rigidbody2D rb;
    private PlayerController pc;

    #endregion

    #region Unity Methods

    private void Start()
    {
        Movement.Boost = Movement.MaxBoost;
        rb = GetComponent<Rigidbody2D>();
        pc = GetComponent<PlayerController>();
        inputHandler = GetComponent<InputHandler>();

        AttatchGuns();
    }

    private void Update()
    {
        if (!pc.IsDead()) {
            updateMovement();
            updateFiring();
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Planet") {
            //Debug.Log("Hit Planet");
            Vector3 dir3 = (collision.gameObject.transform.position - transform.position).normalized;
            Vector2 dir = new Vector2(dir3.x, dir3.y);
            GetComponent<Rigidbody2D>().velocity = dir * Movement.PlanetKnockback * -1;
        }
    }

    #endregion

    #region Methods

    private void updateFiring() 
    {
        if (inputHandler.Controls.Fire.GetKey()) {
            for (int i = 0; i < Guns.Guns.Count; i++) {
                Gun gun = Guns.Guns[i].Gun;
                if (gun != null) {
                    gun.Fire();
                }
            }
        }
    }

    private void updateMovement()
    {

        int steerDir = getXAxisInput();
        gameObject.transform.eulerAngles = new Vector3(0, 0, gameObject.transform.eulerAngles.z + Movement.SteerSpeed * Time.deltaTime * steerDir);

        float accel = Movement.Acceleration;
        bool isBoosting = false;

        if (inputHandler.Controls.Boost.GetKey())
        {
            accel = Movement.BoostAcceleration;
            isBoosting = true;
        }

        if (inputHandler.Controls.ForwardGas.GetKey())
        {
            rb.AddForce(new Vector2(gameObject.transform.up.x, gameObject.transform.up.y) * Time.deltaTime * accel);
        } else if (inputHandler.Controls.BackwardGas.GetKey())
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
        if (inputHandler.Controls.SteerLeft.GetKey()) { x += 1; }
        if (inputHandler.Controls.SteerRight.GetKey()) { x -= 1; }
        return x;
    }

    private void AttatchGuns() {
        for (int i = 0; i < Guns.Guns.Count; i++) {
            GunConnection gunConnection = Guns.Guns[i];
            GameObject GO_gun = Instantiate(gunConnection.GunPrefab, transform);
            gunConnection.Gun = GO_gun.GetComponent<Gun>();
            GO_gun.transform.position = transform.position + (transform.up * gunConnection.Position.y) + (transform.right * gunConnection.Position.x);
        }
    }

    #endregion

    #region Upgrades

    public void MultiplySpeed(float multi) {
        Movement.MaxSpeed *= multi;
    }

    #endregion

    #region Gizmos

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;

        for (int i = 0; i < Guns.Guns.Count; i++) {
            GunConnection gunConnection = Guns.Guns[i];
            Gizmos.DrawWireSphere(transform.position + (transform.up * gunConnection.Position.y) + (transform.right * gunConnection.Position.x), 0.05f);
        }
    }

    #endregion
}
