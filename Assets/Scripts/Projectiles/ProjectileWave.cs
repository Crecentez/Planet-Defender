using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProjectileWave : Projectile
{

    #region Classes 

    [Serializable]
    public class Settings_Class {
        public float speed = 5f;
        public float waveIntensity = 1f;
        public float waveLength = 1f;
        public float lifeTime = 3f;
        public int damage = 4;
        public float knockback = 1f;

        public Settings_Class() { }
    }

    #endregion

    #region Variables

    public Settings_Class settings;

    private Vector3 moveDir;
    private Vector3 rightDir;
    private Vector3 position;
    private float timePassed;
    private float offset;

    private Rigidbody2D rb;
    //private Planet planet;

    #endregion

    #region Unity Methods

    private void Start() {
        position = transform.position;
        moveDir = transform.up;
        rightDir = transform.right;
        offset = Random.Range(0, 40) / 10;
        StartLifeTimer();
    }

    public void Update() {
        position += moveDir * Time.deltaTime * settings.speed;
        Vector3 sideUpdate = rightDir * (float)GetOffset(Time.deltaTime);
        transform.position = position + sideUpdate;
        //gameObject.transform.position = transform.position + (moveDir * settings.speed * Time.deltaTime) + (rightDir * (float)GetOffset(Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        GameObject gm = collision.gameObject;
        
        if (CanDamage(CanDamageTypes.Enemy) && gm.GetComponent<Enemy>()) {
            ApplyKnockback(gm, (gm.transform.position - transform.position) * settings.speed * settings.knockback);
            Damage(gm.GetComponent<Enemy>(), settings.damage);
            Destroy(gameObject);
        } else if (CanDamage(CanDamageTypes.Player) && gm.GetComponent<PlayerController>()) {
            ApplyKnockback(gm, settings.knockback * rb.linearVelocity.normalized);
            Damage(gm.GetComponent<PlayerController>(), settings.damage);
            Destroy(gameObject);
        } else if (gm.GetComponent<Planet>()) {
            if (CanDamage(CanDamageTypes.Planet))
                Damage(gm.GetComponent<Planet>(), settings.damage);
            Destroy(gameObject);
        }
    }

    #endregion

    #region Methods

    private double GetOffset(float dt) {
        timePassed += dt;
        return settings.waveIntensity * Math.Sin(settings.waveLength * (timePassed + offset));
    }

    #endregion

}
