using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Projectile : MonoBehaviour
{
    public float Speed = 5f;
    public float LifeTime = 3f;
    public int Damage = 4;
    public float EnemyKnockback = 1f;
    public float PlayerKnockback = 0.5f;


    private void Start() {

        //GameObject player = GameObject.FindWithTag("Player");
        //player.GetComponent<Rigidbody2D>().AddForce(player.transform.up * PlayerKnockback * -1);

        GetComponent<Rigidbody2D>().velocity = new Vector2(transform.up.x, transform.up.y) * Speed;

        Invoke("DestroyBullet", LifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

        if (collision.gameObject.GetComponent<Enemy>()) {
            Enemy e = collision.gameObject.GetComponent<Enemy>();
            rb.AddForce(GetComponent<Rigidbody2D>().velocity.normalized * EnemyKnockback);

            e.Damage(Damage);
            DestroyBullet();

        } else if (collision.gameObject.CompareTag("Planet")) {
            DestroyBullet();
        }
    }

    private void DestroyBullet() {
        Destroy(gameObject);
    }

}
