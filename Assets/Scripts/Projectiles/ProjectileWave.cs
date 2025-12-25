using System;
using UnityEngine;
using Random = UnityEngine.Random;
using Damage;

namespace Projectile {
    public class ProjectileWave : Projectile {


        #region Variables

        [SerializeField] private float speed = 5f;
        [SerializeField] private float waveIntensity = 1f;
        [SerializeField] private float waveLength = 1f;
        [SerializeField] private int hitDamage = 4;
        [SerializeField] private float knockback = 1f;

        private Vector3 moveDir;
        private Vector3 rightDir;
        private Vector3 position;
        private float timePassed = 0f;

        private Rigidbody2D rb;
        //private Planet planet;

        #endregion

        #region Unity Methods

        private void Start() {
            position = transform.position;
            moveDir = transform.up;
            rightDir = transform.right;
            timePassed += (Random.Range(0, 2) * Mathf.PI) / waveLength;
            StartLifeTimer();
        }

        public void Update() {
            position += moveDir * Time.deltaTime * speed;
            Vector3 sideUpdate = rightDir * (float)GetOffset(Time.deltaTime);
            transform.position = position + sideUpdate;
            //gameObject.transform.position = transform.position + (moveDir * settings.speed * Time.deltaTime) + (rightDir * (float)GetOffset(Time.deltaTime));
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            GameObject gm = collision.gameObject;
            Damageable _damageable = gm.GetComponent<Damageable>();

            if (!_damageable) { return; }
            if (CanDamage(_damageable)) {
                if (gm.GetComponent<Enemy>() || gm.GetComponent<Ship.Ship>())
                    ApplyKnockback(gm, knockback * rb.linearVelocity.normalized);

                _damageable.Damage(hitDamage);
                Destroy(gameObject);
            }
        }

        #endregion

        #region Methods

        private double GetOffset(float dt) {
            timePassed += dt;
            return waveIntensity * Math.Sin(waveLength * timePassed);
        }

        #endregion

    }

}