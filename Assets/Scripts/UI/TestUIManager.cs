using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUIManager : MonoBehaviour
{
    public GameObject TurretPrefab;

    public void Turret() {
        Debug.Log("Placing New Turret");

        GameObject planetGO = GameObject.FindWithTag("Planet");

        Planet planet = planetGO.GetComponent<Planet>();

        GameObject turret = Instantiate(TurretPrefab);

    }
}
