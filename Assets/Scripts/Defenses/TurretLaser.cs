using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretLaser : MonoBehaviour
{

    [Serializable]
    public class Settings {
        public float ShootRadius = 8f;
        public int Damage = 1;
        public float DamageRate = 0.1f;
        public float TurnSpeed = 100f;

        public LineRenderer lineRenderer;
    }

    public Settings settings;


    private Turret turret;
    private GameObject Targeting;

    

    private void Start() {
        turret = GetComponent<Turret>();
        if (turret == null ) {
            Debug.LogWarning("TURRET CLASS NOT FOUND");
        }
        fire();
    }

    private void FixedUpdate() {
        UpdateLaserPosition(0, settings.lineRenderer.gameObject.transform.position);
        if (Targeting != null) {
            float angle = Mathf.Atan2(Targeting.transform.position.y - transform.position.y, Targeting.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, settings.TurnSpeed * Time.fixedDeltaTime);
            UpdateLaserPosition(1, Targeting.transform.position);
            settings.lineRenderer.enabled = true;
        } else {
            Targeting = getClosestEnemy();
            settings.lineRenderer.enabled = false;
        }
    }

    private void UpdateLaserPosition(int index, Vector3 pos) {
        settings.lineRenderer.SetPosition(index, new Vector3(pos.x, pos.y, 10));
    }
    private GameObject getClosestEnemy() {
        GameObject closestEnemy = null;
        float closestDistance = settings.ShootRadius + 1f;
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Enemy")) {
            if ((go.transform.position - transform.position).magnitude < closestDistance) {
                closestEnemy = go;
                closestDistance = (go.transform.position - transform.position).magnitude;
            }
        }
        return closestEnemy;
    }

    private void fire() {
        //GameObject closestEnemy = getClosestEnemy();
        if (Targeting != null) {
            Enemy e = Targeting.GetComponent<Enemy>();
            if (e != null) {
                e.Damage(settings.Damage);
            }

        }
        //Targeting = getClosestEnemy();
        Invoke("fire", settings.DamageRate);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, settings.ShootRadius);
    }
}
