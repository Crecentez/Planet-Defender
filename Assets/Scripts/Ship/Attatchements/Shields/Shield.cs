using System;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ship.Attachments.Shields {

    public delegate void ShieldHealthUpdated(int current, int maxHealth);
    public class Shield : Attachment {

        #region Varaibles

        [SerializeField] protected int MaxHealth = 50;
        [SerializeField] protected bool AttatchAtCenter = false;
        [SerializeField] protected bool EnemyShield = false;

        private int _health;
        public event ShieldHealthUpdated OnHealthUpdated;

        #endregion

        #region Methods

        public override void Attatch(Transform parent, Vector3 localPosition) {
            if (AttatchAtCenter) {
                transform.parent = parent;
                transform.position = Vector3.zero;
            } else {
                base.Attatch(parent, localPosition);
            }
        }

        public void Damage(int amount) {
            _health -= amount;
            if (_health < 0) {
                _health = 0;
            }
            OnHealthUpdated?.Invoke(_health, MaxHealth);
        }

        public void Heal(int amount) {
            _health += amount;
            if (_health > MaxHealth) _health = MaxHealth;
            OnHealthUpdated?.Invoke(_health, MaxHealth);
        }

        public void SetHealth(int amount) {
            _health = amount;
            if (_health > MaxHealth) _health = MaxHealth;
            OnHealthUpdated?.Invoke(_health, MaxHealth);
        }

        #endregion

    }
}
