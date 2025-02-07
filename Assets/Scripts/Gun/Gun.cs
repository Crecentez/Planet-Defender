using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {


    [SerializeField]
    private GameObject BulletPrefab;
    [SerializeField]
    private float FireRate = 0.5f;
    [SerializeField]
    private Vector2 offset = Vector2.zero;

    private bool CanFire = true;

    public void Fire() {
        if (CanFire) {
            CanFire = false;
            

            GameObject b = Instantiate(BulletPrefab);

            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<Rigidbody2D>().AddForce(player.transform.up * b.GetComponent<Projectile>().PlayerKnockback * -1);

            b.transform.position = transform.position + (transform.up * offset.y) + (transform.right * offset.x);
            b.transform.rotation = transform.rotation;

            Invoke("resetFire", FireRate);
        }
    }

    public void resetFire() {
        CanFire = true;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(transform.position + (transform.up * offset.y) + (transform.right * offset.x) + new Vector3(0, 0, -1), new Vector3(0.05f, 0.1f, 0.1f));
    }

}
