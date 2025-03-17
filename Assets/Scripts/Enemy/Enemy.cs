using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    protected int MaxHealth = 20;

    protected int Health = 1;

    [HideInInspector]
    public float playerDamageScale = 1;
    [HideInInspector]
    public float planetDamageScale = 1;

    public WaveHandler waveHandler;

    private void Start() {
        Health = MaxHealth;
    }


    public void Damage(int amount) {
        Debug.Log("Was at " + Health.ToString() + " | Now at " + (Health - amount).ToString());
        Health -= amount;
        //Debug.Log(Health);

        if (Health <= 0) { Kill(); }

    }

    public void Kill() {
        if (waveHandler != null) {
            waveHandler.EnemyKilled(gameObject);
        }

        Destroy(gameObject);
    }

    public void ScaleHealth(float scaleAmount) {
        if (scaleAmount > 0) {
            Health = Mathf.FloorToInt(MaxHealth * scaleAmount);
        }
    }

    public void DamagePlayer(int amount) {
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null) {
            PlayerController pc = player.GetComponent<PlayerController>();

            pc.Damage(amount);
        }
    }
}
