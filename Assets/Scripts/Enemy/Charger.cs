using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : Enemy
{

    [Serializable]
    protected class V_Settings {
        [Header("Speed")]
        [SerializeField]
        public float MaxSpeed = 1f;
        [SerializeField]
        public float Acceleration = 1f;
        [SerializeField]
        public float RotSpeed = 150f;

        [Header("Knockback")]
        [SerializeField]
        public float PlayerKnockback = 3f;
        [SerializeField]
        public float SelfKnockback = 1f;

        [Header("Damage")]
        [SerializeField]
        public int PlanetDamage = 5;
        [SerializeField]
        public int PlayerDamage = 10;

        [Header("Other")]
        [SerializeField]
        public float SightRadius = 5f;
    }

    [SerializeField]
    protected V_Settings Settings = new V_Settings();

    [SerializeField]
    private bool ChasePlayer = false;

    

    private Rigidbody2D rb;

    private void Start() {
        Health = MaxHealth;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        UpdateRotation();
        UpdateMovement();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        GameObject gm = collision.gameObject;
        if (gm.CompareTag("Player") && gm.GetComponent<PlayerController>()) {
            PlayerController pc = gm.GetComponent<PlayerController>();
            DoDamage(pc, Settings.PlayerDamage);
            ApplyKnockback(transform.position, gm.GetComponent<Rigidbody2D>(), Settings.PlayerKnockback);
            Damage(Mathf.FloorToInt(Settings.PlayerDamage * playerDamageScale));
            ApplyKnockback(gm.transform.position, rb, Settings.SelfKnockback);
        } else if (gm.CompareTag("Player") && gm.GetComponent<Planet>()) {
            Planet planet = gm.GetComponent<Planet>();
            DoDamage(planet, Settings.PlanetDamage);
            Damage(Mathf.FloorToInt(Settings.PlanetDamage * planetDamageScale));
            ApplyKnockback(gm.transform.position, rb, Settings.SelfKnockback);
        }
    }

    private void UpdateRotation() {
        gameObject.transform.eulerAngles = new Vector3(0, 0, gameObject.transform.eulerAngles.z + Settings.RotSpeed * Time.deltaTime);
    }

    private void UpdateMovement() {
        GameObject Target = null;

        if (ChasePlayer) {
            GameObject TargetPlayer = GameObject.FindWithTag("Player");
            if (TargetPlayer != null && (TargetPlayer.transform.position - gameObject.transform.position).magnitude <= Settings.SightRadius) {
                Target = TargetPlayer;
            } else { Target = GameObject.FindWithTag("Planet"); }
        } else { Target = GameObject.FindWithTag("Planet"); }
        if (Target == null) {
            return;
        }

        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();

        Vector3 movDir = (transform.position - Target.transform.position).normalized;
        Vector2 vel = new Vector2(movDir.x, movDir.y) * Settings.Acceleration * -1;

        if (rb.linearVelocity.magnitude < Settings.MaxSpeed) {
            rb.AddForce(vel);
        }
    }

    public void ApplyKnockback(Vector3 AwayFrom, Rigidbody2D To, float amount) {
        Vector3 dir3 = (AwayFrom - To.gameObject.transform.position).normalized;
        Vector2 dir = new Vector2(dir3.x, dir3.y);
        To.linearVelocity = dir * amount * -1f;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Settings.SightRadius);
    }

}
