using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Projectile : MonoBehaviour
{
    public float Speed = 5f;
    public float LifeTime = 3f;
    public int Damage = 8;
    public float PlayerKnockback = 0.5f;

    public Enemy enemy;


    private void Start() {

        GetComponent<Rigidbody2D>().linearVelocity = new Vector2(transform.up.x, transform.up.y) * Speed;

        Invoke("DestroyBullet", LifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        

        if (collision.gameObject.tag == "Player") {

            

            Vector3 dir3 = (collision.gameObject.transform.position - transform.position).normalized;
            Vector2 dir = new Vector2(dir3.x, dir3.y);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(dir * PlayerKnockback);
            collision.gameObject.GetComponent<PlayerController>().Damage(Mathf.FloorToInt(Damage * enemy.playerDamageScale));

            Destroy(gameObject);

        } else if (collision.gameObject.tag == "Planet") {
            Planet planet = collision.gameObject.GetComponent<Planet>();
            //planet.Damage(Mathf.FloorToInt(Damage * enemy.planetDamageScale));
            Destroy(gameObject);
        }
    }

}
