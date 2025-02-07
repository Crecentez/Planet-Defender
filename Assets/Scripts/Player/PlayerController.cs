using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private int MaxHealth = 30;

    private int Health = 1;

    [SerializeField]
    public bool IsDead = false;


    private void Start() {
        Health = MaxHealth;
    }

    public void Damage(int damage) {
        Health -= damage;

        if (Health < 0) Kill();
    }

    public void Kill() {
        Debug.Log("Died");
        Health = MaxHealth;

        //Destroy(gameObject);
        IsDead = true;

        Invoke("RestartGame", 3);
    }

    public void RestartGame() {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }


}
