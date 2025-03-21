using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasicGun : Attatchement
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

    [SerializeField] private Settings_Class settings;

    [SerializeField] private GameObject _bulletPrefab;

    private GameInputMap _input;
    private InputAction _fireInput;

    private Planet planet;
    private bool _canFire = false;

    #endregion

    #region Unity Methods

    private void OnEnable() {
        _input = new GameInputMap();
        _input.Gun.Enable();
        _fireInput = _input.Gun.Fire;
        _canFire = true;
    }

    private void OnDisable() {
        _input.Gun.Disable();
        _input = null;
        _canFire = false;
    }

    private void Start() {
        //GameObject planet_gm = GameObject.FindGameObjectWithTag("Planet");
        //if (_bulletPrefab.GetComponent<Projectile>() != null || planet_gm != null) {
        //    planet = planet_gm.GetComponent<Planet>();
        //    _canFire = true;
        //} else { Debug.LogWarning("Planet not found!"); }
    }

    private void Update() {
        if (_canFire && _fireInput.IsPressed()) {
            StartCoroutine(Fire());
        }
    }

    #endregion

    #region Methods

    public IEnumerator Fire() {
        if (_canFire) {
            _canFire = false;

            GameObject b = Instantiate(_bulletPrefab);

            //GameObject player = GameObject.FindWithTag("Player");
            //player.GetComponent<Rigidbody2D>().AddForce(player.transform.up * b.GetComponent<Projectile>().PlayerKnockback * -1);
            b.transform.position = transform.position + (transform.up * settings.offset.y) + (transform.right * settings.offset.x);
            b.transform.rotation = new quaternion(transform.rotation.x + (Mathf.Deg2Rad * settings.rotation), transform.rotation.y, transform.rotation.z, transform.rotation.w);

            yield return new WaitForSeconds(settings.fireRate);
            _canFire = true;
        }
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
