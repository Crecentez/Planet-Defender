using Ship;
using System;
using UnityEngine;

namespace Damage {

    public enum EntityType {
        Enemy,
        Player,
        Planet,
        Turret
    }

    public delegate void HealthUpdated(int old, int current);
    public class Damageable : MonoBehaviour {

        #region Variables

        [SerializeField] private int MaxHealth = 1000;
        [SerializeField] private EntityType entityType = EntityType.Player;

        public event HealthUpdated OnHealthUpdated;
        public event Action OnKilled;

        private int _health = 0;
        private bool _isDead = false;

        #endregion

        #region Unity Methods

        private void Start() {
            _health = MaxHealth;
            OnHealthUpdated?.Invoke(0, MaxHealth);
            _isDead = false;
        }

        #endregion

        #region Methods

        public void Damage(int damage) {
            int old = _health;
            _health -= damage;
            if (_health < 0) _health = 0;
            OnHealthUpdated?.Invoke(old, _health);
            if (_health <= 0) Kill();
        }

        public void Heal(int heal) {
            int old = _health;
            _health += heal;
            if (_health > MaxHealth) _health = MaxHealth;
            OnHealthUpdated?.Invoke(old, _health);
        }

        public void Kill() {
            if (_isDead) return;
            Debug.Log(gameObject.name + " Killed");
            int old = _health;
            _health = 0;
            _isDead = true;
            OnHealthUpdated?.Invoke(old, _health);
            OnKilled?.Invoke();
        }

        public int GetMaxHealth() { return MaxHealth; }
        public int GetHealth() { return _health; }
        public bool IsDead() { return _isDead; }

        public EntityType GetEntityType() { return entityType; }

        #endregion
    }
}

