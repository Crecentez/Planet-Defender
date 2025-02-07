using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public int MaxHealth = 1000;
    public int Health = 1;

    public int TotalOrbits = 1;
    public float TurrentOrbitRadius = 2.5f;

    public List<GameObject> Turrents = new List<GameObject>();

    private void Start() {
        Health = MaxHealth;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, TurrentOrbitRadius);
    }


}
