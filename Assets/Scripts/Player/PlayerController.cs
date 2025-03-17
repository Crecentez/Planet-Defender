using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    #region Variables

    [SerializeField]
    private int maxHealth = 30;

    private int health = 1;

    [SerializeField]
    private bool isDead = false;

    #endregion

    #region Unity Methods

    private void Start() {
        health = maxHealth;
    }

    #endregion

    #region Methods

    public void Damage(int damage) {
        health -= damage;

        if (health < 0) Kill();
    }

    public void Kill() {
        Debug.Log("Died");
        health = maxHealth;

        //Destroy(gameObject);
        isDead = true;

        Invoke("RestartGame", 3);
    }

    public void RestartGame() {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public bool IsDead() { return isDead; }

    #endregion
}
