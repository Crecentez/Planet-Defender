using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [Serializable]
    public class OrbitSettings_Class {
        public float StartOrbitRadius = 2.5f;
        public float AddedOrbitDistance = 0.5f;
        public int MaxPerRow = 10;
        public int MaxTurrets = 60;
    }

    public int MaxHealth = 1000;
    public int Health = 1;

    //public int TotalOrbits = 1;
    public OrbitSettings_Class OrbitSettings = new OrbitSettings_Class();

    public List<Turret> Turrets = new List<Turret>();

    [Header("Testing")]
    [SerializeField]
    private List<GameObject> PreSpawnTurrets = new List<GameObject>();

    private void Start() {
        Health = MaxHealth;

        foreach (GameObject p in PreSpawnTurrets) {
            GameObject o = Instantiate(p);
            Turret t = o.GetComponent<Turret>();
            t.planet = this;
            Turrets.Add(t);
        }
        foreach (GameObject p in PreSpawnTurrets) {
            GameObject o = Instantiate(p);
            Turret t = o.GetComponent<Turret>();
            t.planet = this;
            Turrets.Add(t);
        }
        foreach (GameObject p in PreSpawnTurrets) {
            GameObject o = Instantiate(p);
            Turret t = o.GetComponent<Turret>();
            t.planet = this;
            Turrets.Add(t);
        }
        updateTurretPosition();
    }

    private void updateTurretPosition() {
        for(int i = 0; i < Turrets.Count; i++) {
            Turret t = Turrets[i];

            if (t != null && t.planet == this) {
                t.orbitPosition = i + 1;
            }
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, OrbitSettings.StartOrbitRadius);
        Gizmos.DrawWireSphere(transform.position, OrbitSettings.StartOrbitRadius + (OrbitSettings.AddedOrbitDistance * Mathf.Ceil((OrbitSettings.MaxTurrets/2) / OrbitSettings.MaxPerRow)));
        Gizmos.DrawWireSphere(transform.position, OrbitSettings.StartOrbitRadius + (OrbitSettings.AddedOrbitDistance * Mathf.Ceil(OrbitSettings.MaxTurrets / OrbitSettings.MaxPerRow)));

    }


}
