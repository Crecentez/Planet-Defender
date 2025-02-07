using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turrent : MonoBehaviour
{
    [Serializable]
    public class Settings {
        public float ShootRadius = 8f;
        public float FireRate = 1f;
        public Vector2 Offset = Vector2.zero;
        public float TurnSpeed = 1f;
    }

    public Settings settings;
    public GameObject BulletPrefab;

    private List<GameObject> enemiesInRadius = new List<GameObject>();

    private CircleCollider2D circleCollider;

    private GameObject Targeting;

    private void Start() {
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.radius = settings.ShootRadius;
    }

    private void FixedUpdate() {
        if (Targeting != null) {
            float angle = Mathf.Atan2(Targeting.transform.position.y - transform.position.y, Targeting.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, settings.TurnSpeed * Time.fixedDeltaTime);
        } else {
            Targeting = getClosestEnemy();
        }
    }

    private void fire() {
        if (circleCollider != null) {
            //GameObject closestEnemy = getClosestEnemy();
            if (Targeting != null) {
                GameObject b = Instantiate(BulletPrefab);
                b.transform.position = transform.position + (transform.up * settings.Offset.y) + (transform.right * settings.Offset.x);
                //b.transform.rotation = transform.rotation;
                Vector3 targ = Targeting.transform.position;
                targ.z = 0f;
                targ.x = targ.x - b.transform.position.x;
                targ.y = targ.y - b.transform.position.y;
                float angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg;
                b.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            }
        }
        Targeting = getClosestEnemy();
        Invoke("fire", settings.FireRate);
    }

    private GameObject getClosestEnemy() {
        GameObject closestEnemy = null;
        float closestDistance = settings.ShootRadius + 1f;
        foreach (GameObject go in enemiesInRadius) {
            if ((go.transform.position - transform.position).magnitude < closestDistance) {
                closestEnemy = go;
                closestDistance = (go.transform.position - transform.position).magnitude;
            }
        }
        return closestEnemy;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.GetComponent<Enemy>()) {
            enemiesInRadius.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
            if (collision.gameObject.GetComponent<Enemy>() && enemiesInRadius.Contains(collision.gameObject)) {
                enemiesInRadius.Remove(collision.gameObject);
            }
        }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, settings.ShootRadius);
    }
}
