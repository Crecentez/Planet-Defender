using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Serializable]
    private class SpawnTableItem {
        
        public int weight = 1;
        public GameObject enemy;

    } 

    public float MinSpawnRadius = 10f;
    public float MaxSpawnRadius = 15f;

    public float SpawnRate = 5f;

    public bool canSpawn = true;

    [SerializeField]
    private List<SpawnTableItem> Enemies = new List<SpawnTableItem>();

    private void Start() {
        spawnLoop();
    }

    public void spawnLoop() {
        if (canSpawn) {
            SpawnRandom();
        }
        
        Invoke("spawnLoop", SpawnRate);
    }

    public void SpawnRandom() {
        int totalWeight = 0;

        for (int i = 0; i < Enemies.Count; i++) {
            totalWeight += Enemies[i].weight;
        }
        
        int number = UnityEngine.Random.Range(0, totalWeight);

        for (int i = 0; i < Enemies.Count; i++) {
            
            if (Enemies[i].weight >= number) {
                Spawn(Enemies[i].enemy);
                break;
            } else {
                number -= Enemies[i].weight;
            }
        }

    }

    public void Spawn(GameObject Enemy) {

        float dir = UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad;
        float distance = UnityEngine.Random.Range(Mathf.FloorToInt(MinSpawnRadius * 100), Mathf.FloorToInt(MaxSpawnRadius * 100))/100;

        Vector2 Position = new Vector2(Mathf.Cos(dir) * distance, Mathf.Sin(dir) * distance) + new Vector2(transform.position.x, transform.position.y);

        GameObject e = Instantiate(Enemy);
        e.transform.position = Position;
    }


    private void OnDrawGizmosSelected() {

        if (MinSpawnRadius > MaxSpawnRadius) {  MinSpawnRadius = MaxSpawnRadius; }

        if (MaxSpawnRadius == MinSpawnRadius) {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(gameObject.transform.position, MaxSpawnRadius);
        } else {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(gameObject.transform.position, MinSpawnRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(gameObject.transform.position, MaxSpawnRadius);
        }
    }
}
