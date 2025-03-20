using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public delegate void HealthUpdated(int old, int current);

public class PlayerController : MonoBehaviour
{

    #region Variables

    // Private
    [SerializeField] private int _maxHealth = 30;
    public int health { get; private set; } = 1;
    [SerializeField] public bool isDead { get; private set; } = false;

    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private AttatchementHandler _attatchement;
    [SerializeField] private PlaySpaceBorder _border;
    [SerializeField] private Camera _camera;

    public bool isPaused { get; private set; }  = false;

    // Events
    public event HealthUpdated OnHealthUpdated;
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
        
        OnHealthUpdated?.Invoke(old, health);

        if (health <= 0) Kill();
    }

    public void Kill() {
        if (!isDead)
            return;

        health = 0;
        isDead = true;

        OnKilled?.Invoke();

        StartCoroutine(RestartGame());
    }

    public IEnumerator RestartGame() {
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

    public int MaxHealth() { return _maxHealth; }


    #endregion
}
