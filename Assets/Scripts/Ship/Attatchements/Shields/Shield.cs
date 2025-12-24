using System;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ship.Attachments.Shields {

    public delegate void ShieldHealthUpdated(int current, int maxHealth);
    public class Shield : Attachment {

        #region Varaibles

        [SerializeField] protected int _maxHealth = 50;
        [SerializeField] protected bool _attatchAtCenter = false;

        private int _health;
        public event ShieldHealthUpdated OnHealthUpdated;

        #endregion

        #region Methods

        public override void Attatch(Transform parent, Vector3 localPosition) {
            if (_attatchAtCenter) {
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
            OnHealthUpdated?.Invoke(_health, _maxHealth);
        }

        public void Heal(int amount) {
            _health += amount;
            if (_health > _maxHealth) _health = _maxHealth;
            OnHealthUpdated?.Invoke(_health, _maxHealth);
        }

        public void SetHealth(int amount) {
            _health = amount;
            if (_health > _maxHealth) _health = _maxHealth;
            OnHealthUpdated?.Invoke(_health, _maxHealth);
        }

        #endregion

    }
}
