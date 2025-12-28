using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Damage;

namespace Projectile {
    [System.Flags]
    public enum CanDamageTypes {
        Enemy,
        Player,
        Planet,
        Turret
    }

    public class Projectile : MonoBehaviour {

        #region Variables

        [EnumFlags] public CanDamageTypes canDamageTypes = CanDamageTypes.Enemy;

        [SerializeField][Tooltip("Value of -1 will result in infinite lifespan")]
        private float LifeTime = 8f;

        #endregion

        #region Unity Methods

        private void Start() {
            StartLifeTimer();
        }

        #endregion

        #region Methods

        protected void StartLifeTimer() {
            if (LifeTime >= 0)
                StartCoroutine(LifeTimeTimer(LifeTime));
        }
        protected void StartLifeTimer(float lifeTime) {
            StartCoroutine(LifeTimeTimer(lifeTime));
        }

        private IEnumerator LifeTimeTimer(float lifeTime) {
            yield return new WaitForSeconds(lifeTime);
            Destroy(gameObject);
        }

        protected void ApplyKnockback(GameObject gm, Vector2 Knockback) {
            Rigidbody2D other_rb = gm.GetComponent<Rigidbody2D>();
            if (other_rb) { other_rb.AddForce(Knockback); }
        }

        protected bool CanDamageEntity(EntityType type) {
            List<string> canDamage = EnumFlagsAttribute.GetSelectedStrings(canDamageTypes);
            for (int i = 0; i < canDamage.Count; i++) {
                if (canDamage[i].Equals(type.ToString())) { return true; }
            }
            return false;
        }

        protected bool CanDamage(Damageable damageable) {
            return CanDamageEntity(damageable.GetEntityType()) && !damageable.IsDead();
        }



        #endregion
    }
}
