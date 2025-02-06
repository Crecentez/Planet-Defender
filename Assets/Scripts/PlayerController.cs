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

    [SerializeField]
    private float PlanetKnockback = 10f;


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

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Planet") {
            Debug.Log("Hit Planet");
            Vector3 dir3 = (collision.gameObject.transform.position - transform.position).normalized;
            Vector2 dir = new Vector2(dir3.x, dir3.y);
            GetComponent<Rigidbody2D>().velocity = dir * PlanetKnockback * -1;
        }
    }

}
