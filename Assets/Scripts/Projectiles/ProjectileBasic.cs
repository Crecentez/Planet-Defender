using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBasic : Projectile
{

    #region Classes 

    [Serializable]
    public class Settings_Class {
        public float speed = 5f;
        public float lifeTime = 3f;
        public int damage = 4;
        public float knockback = 1f;

        public Settings_Class() { }
    }

    #endregion

    #region Variables

    public Settings_Class settings;

    private Rigidbody2D rb;
    //private Planet planet;

    #endregion

    #region Unity Methods

    private void Start() {
        
        rb = GetComponent<Rigidbody2D>();
        if (rb == null ) {
            Debug.LogWarning("RigidBody2D not found!");
            Destroy(gameObject);
        }
        //GameObject planet_gm = GameObject.FindGameObjectWithTag("Planet");
        //if (planet_gm != null) {
        //    planet = planet_gm.GetComponent<Planet>();
        //} else {
        //    Debug.LogWarning("Planet not found!");
        //    Destroy(gameObject);
        //}

        rb.linearVelocity = new Vector2(transform.up.x, transform.up.y) * settings.speed;
        StartLifeTimer();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        GameObject gm = collision.gameObject;
        
        if (CanDamage(CanDamageTypes.Enemy) && gm.GetComponent<Enemy>()) {
            ApplyKnockback(gm, settings.knockback * rb.linearVelocity.normalized);
            Damage(gm.GetComponent<Enemy>(), settings.damage);
            Destroy(gameObject);
        } else if (CanDamage(CanDamageTypes.Player) && gm.GetComponent<PlayerController>()) {
            ApplyKnockback(gm, settings.knockback * rb.linearVelocity.normalized);
            Damage(gm.GetComponent<PlayerController>(), settings.damage);
            Destroy(gameObject);
        } else if (CanDamage(CanDamageTypes.Planet) && gm.GetComponent<Planet>()) {
            Damage(gm.GetComponent<Planet>(), settings.damage);
            Destroy(gameObject);
        }
    }

    #endregion

}
