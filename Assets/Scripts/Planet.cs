using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    //Public
    public int MaxHealth = 1000;
    public int Health = 1;
    public OrbitSettings_Class OrbitSettings = new OrbitSettings_Class();
    public List<Turret> Turrets = new List<Turret>();

    #endregion

    #region Unity Methods

    private void Start() {
        Health = MaxHealth;
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

    public void Damage(int amount) {
        Health -= amount;
        if (Health < 0) {
            BlowUp();
        }
    }

    public void BlowUp() {
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
