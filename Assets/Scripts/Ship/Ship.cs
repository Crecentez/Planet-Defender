using System;
using UnityEngine;

public delegate void HealthUpdated(int old, int current);
public class Ship : MonoBehaviour
{

    #region Veriables

    // Constants
    public const int MaxHealth = 1000;

    // Protected
    protected int _health = 0;
    protected bool _isDead = false;


    // Private


    // Events
    public event HealthUpdated OnHealthUpdated;
    public event Action OnKilled;

    #endregion

    #region Unity Methods

    private void Start() {
        _health = MaxHealth;
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
        int old = _health;
        _health = 0;
        _isDead = true;
        OnHealthUpdated?.Invoke(old, _health);
        OnKilled?.Invoke();
    }

    public int GetMaxHealth() { return MaxHealth; }
    public int GetHealth() { return _health; }
    public bool IsDead() { return _isDead; }


    #endregion
}
