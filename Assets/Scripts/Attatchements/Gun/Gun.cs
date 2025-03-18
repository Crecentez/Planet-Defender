using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Gun : Attatchement
{

    #region Classes

    [Serializable]
    public class Settings_Class {
        public float fireRate = 0.5f;
        public Vector2 offset = Vector2.zero;
        [Tooltip("In Degrees")]
        public float rotation = 0f;

        Settings_Class() { }
    }

    #endregion

    #region Variables

    public Settings_Class settings;

    [SerializeField]
    private GameObject BulletPrefab;

    private Planet planet;
    private bool CanFire = false;
    
    #endregion

    #region Unity Methods

    private void Start() {
        GameObject planet_gm = GameObject.FindGameObjectWithTag("Planet");
        if (BulletPrefab.GetComponent<Projectile>() != null || planet_gm != null) {
            planet = planet_gm.GetComponent<Planet>();
            CanFire = true;
        } else { Debug.LogWarning("Planet fot found!"); }
    }

    #endregion

    #region Methods

    public void Fire() {
        if (CanFire) {
            CanFire = false;

            GameObject b = Instantiate(BulletPrefab);

            //GameObject player = GameObject.FindWithTag("Player");
            //player.GetComponent<Rigidbody2D>().AddForce(player.transform.up * b.GetComponent<Projectile>().PlayerKnockback * -1);
            b.transform.position = transform.position + (transform.up * settings.offset.y) + (transform.right * settings.offset.x);
            b.transform.rotation = new quaternion(transform.rotation.x + (Mathf.Deg2Rad * settings.rotation), transform.rotation.y, transform.rotation.z, transform.rotation.w);

            Invoke("ResetFire", settings.fireRate);
        }
    }

    public void ResetFire() {
        CanFire = true;
    }

    #endregion

    #region Gizmos

    private void OnDrawGizmosSelected() {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(settings.offset, 0.02f);
    }

    #endregion
}
