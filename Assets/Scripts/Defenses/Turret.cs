using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{

    [Serializable]
    public class Orbit_Class {
        public Planet planet;
        public int orbitPosition = 1;
        public float OrbitSpeed = 1f;
    }



    public Orbit_Class Orbit = new Orbit_Class();

    [Header("Health")]
    [SerializeField]
    private int maxHealth = 100;
    private int health = 0;

    [SerializeField]
    private bool IsWorking = true;

    private void Start() {
        health = maxHealth;
    }

    private void FixedUpdate() {
        
        updatePosition();
    }

    private int damage(int damage) {
        int damageDelt = damage + (health < damage ? health - damage : 0);
        health = Mathf.Max(health - damage, 0);
        return damageDelt;
    }

    private bool getIsWorking() {
        return IsWorking;
    }

    private void updatePosition() {
        if (Orbit.planet != null) {
            Vector3 posDir = Vector2.zero;
            float ring = ((Orbit.orbitPosition - (Orbit.orbitPosition % Orbit.planet.OrbitSettings.MaxPerRow)) / Orbit.planet.OrbitSettings.MaxPerRow);
            float orbitRadius = Orbit.planet.OrbitSettings.AddedOrbitDistance * (ring - 1) + Orbit.planet.OrbitSettings.StartOrbitRadius;
            float offset = 2 * MathF.PI * (Orbit.orbitPosition % Orbit.planet.OrbitSettings.MaxPerRow);
            if (ring % 2 == 0) {
                offset += MathF.PI;
            }

            posDir.x = Mathf.Cos(Orbit.OrbitSpeed * Time.time + offset / Orbit.planet.OrbitSettings.MaxPerRow) * orbitRadius;
            posDir.y = Mathf.Sin(Orbit.OrbitSpeed * Time.time + offset / Orbit.planet.OrbitSettings.MaxPerRow) * orbitRadius;
            Debug.Log(posDir);
            transform.position = posDir + Orbit.planet.gameObject.transform.position;
        }
    }

    

    

    
}
