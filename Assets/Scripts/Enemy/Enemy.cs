using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    protected int MaxHealth = 20;

    protected int Health = 1;

    public WaveHandler waveHandler;

    private void Start() {
        Health = MaxHealth;
    }

    public void Damage(int amount) {

        Health -= amount;

        if (Health < 0) { Kill(); }

    }

    public void Kill() {
        if (waveHandler != null) {
            waveHandler.EnemyKilled(gameObject);
        }

        Destroy(gameObject);
    }

    public void DamagePlayer(int amount) {
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null) {
            PlayerController pc = player.GetComponent<PlayerController>();

            pc.Damage(amount);
        }
    }
}
