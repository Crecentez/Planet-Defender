using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public int MaxHealth = 1000;
    public int Health = 1;

    public List<GameObject> Turrents = new List<GameObject>();

    private void Start() {
        Health = MaxHealth;
    }



}
