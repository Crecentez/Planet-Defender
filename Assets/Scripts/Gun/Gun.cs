using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {
   
    
    public GameObject BulletPrefab;
    [SerializeField]
    private float FireRate = 0.5f;

    private bool CanFire = true;

    public void Fire() {
        if (CanFire) {
            CanFire = false;
            GameObject b = Instantiate(BulletPrefab);
            b.transform.position = transform.position;
            b.transform.rotation = transform.rotation;

            Invoke("resetFire", FireRate);
        }
    }

    public void resetFire() {
        CanFire = true;
    }
        
}
