using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUIManager : MonoBehaviour
{
    public GameObject TurrentPrefab;

    public void Turrent() {
        Debug.Log("Placing New Turrent");

        GameObject planetGO = GameObject.FindWithTag("Planet");

        Planet planet = planetGO.GetComponent<Planet>();

        GameObject turrent = Instantiate(TurrentPrefab);

    }
}
