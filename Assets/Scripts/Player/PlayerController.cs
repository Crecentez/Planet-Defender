using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public delegate void HealthUpdated(int old, int current);
public delegate void MoneyUpdated(int old, int current);
public class PlayerController : MonoBehaviour
{

    #region Variables

    //Public
    public int health = 1;
    public int _maxHealth = 1000;
    public bool isDead = false;
    public bool isPaused { get; private set; } = false;
    
    // Private

    [SerializeField] private Ship.Ship _movement;
    //[SerializeField] private AttatchementHandler _attatchement;
    [SerializeField] private PlaySpaceBorder _border;
    [SerializeField] private Camera _camera;

    //// Events
    public event HealthUpdated OnHealthUpdated;
    public event MoneyUpdated OnMoneyUpdated;
    public event Action OnKilled;
    public event Action<bool> OnPauseStateUpdated;


    #endregion

    #region Unity Methods

    private void Start() {
        health = _maxHealth;
    }

    #endregion

    #region Methods

    public void Damage(int damage) {
        int old = health;
        health -= damage;
        if (health < 0) health = 0;
        OnHealthUpdated?.Invoke(old, health);
        if (health <= 0) Kill();
    }

    public void Kill() {
        if (isDead)
            return;

        int old = health;
        health = 0;
        isDead = true;

        OnHealthUpdated?.Invoke(old, health);
        OnKilled?.Invoke();

        StartCoroutine(RestartGame());
    }

    public IEnumerator RestartGame() {
        Debug.Log("Restarting Scene");
        yield return new WaitForSeconds(3f);
        yield return SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public void Pause() {
        isPaused = true;
        OnPauseStateUpdated?.Invoke(isPaused);
    }
    public void Resume() {
        isPaused = false;
        OnPauseStateUpdated?.Invoke(isPaused);
    }

    public int GetMaxHealth() { return _maxHealth; }

    #region Money

    //public void SetMoney(int amount) {
    //    OnMoneyUpdated?.Invoke(money, amount);
    //    money = amount;
    //}

    //public void AddMoney(int amount) {
    //    OnMoneyUpdated?.Invoke(money, money + amount);
    //    money += amount;
    //}

    #endregion

    #endregion
}
