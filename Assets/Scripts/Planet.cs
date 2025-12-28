using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Damage;

[RequireComponent(typeof(Damageable))]
public class Planet : MonoBehaviour
{

    #region Classes

    [Serializable]
    public class OrbitSettings_Class {
        public float StartOrbitRadius = 2.5f;
        public float AddedOrbitDistance = 0.5f;
        public int MaxPerRow = 10;
        public int MaxTurrets = 60;
    }

    #endregion

    #region Variables

    [SerializeField] protected OrbitSettings_Class OrbitSettings = new OrbitSettings_Class();
    [SerializeField] protected List<Turret> Turrets = new List<Turret>();

    private int _health = 1;
    private Damageable _damageable;

    #endregion

    #region Unity Methods

    private void Start() {
        _damageable = GetComponent<Damageable>();
        _damageable.OnKilled += OnKilled;
    }

    #endregion

    #region Methods

    public void AddTurrent(GameObject TurrentPrefab) {
        GameObject o = Instantiate(TurrentPrefab);
        Turret t = o.GetComponent<Turret>();
        t.Orbit.planet = this;
        Turrets.Add(t);
        UpdateTurretPosition();
    }

    public void OnKilled() {
        Debug.Log("Restarting scene...");
        Invoke("RestartGame", 3);
    }

    public void RestartGame() {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    private void UpdateTurretPosition() {
        for(int i = 0; i < Turrets.Count; i++) {
            Turret t = Turrets[i];

            if (t != null && t.Orbit.planet == this) {
                t.Orbit.orbitPosition = i;
            }
        }
    }

    #endregion

    #region Gizmos

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, OrbitSettings.StartOrbitRadius);
        Gizmos.DrawWireSphere(transform.position, OrbitSettings.StartOrbitRadius + (OrbitSettings.AddedOrbitDistance * Mathf.Ceil((OrbitSettings.MaxTurrets/2) / OrbitSettings.MaxPerRow)));
        Gizmos.DrawWireSphere(transform.position, OrbitSettings.StartOrbitRadius + (OrbitSettings.AddedOrbitDistance * Mathf.Ceil(OrbitSettings.MaxTurrets / OrbitSettings.MaxPerRow)));
    }

    #endregion


}
