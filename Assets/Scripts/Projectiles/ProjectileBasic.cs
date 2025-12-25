using Damage;
using Ship.Attachments.Shields;
using UnityEngine;

namespace Projectile {
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class ProjectileBasic : Projectile {


        #region Variables

        // Private
        [SerializeField] private float speed = 5f;
        [SerializeField] private int hitDamage = 4;
        [SerializeField] private float knockback = 1f;

        private Rigidbody2D rb;

        #endregion

        #region Unity Methods

        private void Start() {

            rb = GetComponent<Rigidbody2D>();
            //GameObject planet_gm = GameObject.FindGameObjectWithTag("Planet");
            //if (planet_gm != null) {
            //    planet = planet_gm.GetComponent<Planet>();
            //} else {
            //    Debug.LogWarning("Planet not found!");
            //    Destroy(gameObject);
            //}

            rb.linearVelocity = new Vector2(transform.up.x, transform.up.y) * speed;
            StartLifeTimer();
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

    }
}
