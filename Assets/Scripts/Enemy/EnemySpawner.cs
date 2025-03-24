using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    #region Classes

    [Serializable]
    private class SpawnTableItem {
        
        public int weight = 1;
        public GameObject enemy;

    }

    #endregion

    #region Varaibles

    [SerializeField] private float _minSpawnRadius = 10f;
    [SerializeField] private float _maxSpawnRadius = 15f;

    [SerializeField] private float _spawnRate = 5f;

    [SerializeField] private bool _canSpawn = true;

    [SerializeField] private List<SpawnTableItem> _enemies = new List<SpawnTableItem>();

    #endregion

    private void Start() {
        SpawnLoop();
    }

    public void SpawnLoop() {
        if (_canSpawn) {
            SpawnRandom();
        }
        
        Invoke("spawnLoop", _spawnRate);
    }

    public void SpawnRandom() {
        int totalWeight = 0;

        for (int i = 0; i < _enemies.Count; i++) {
            totalWeight += _enemies[i].weight;
        }
        
        int number = UnityEngine.Random.Range(0, totalWeight);

        for (int i = 0; i < _enemies.Count; i++) {
            
            if (_enemies[i].weight >= number) {
                Spawn(_enemies[i].enemy);
                break;
            } else {
                number -= _enemies[i].weight;
            }
        }

    }

    public void Spawn(GameObject Enemy) {

        float dir = UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad;
        float distance = UnityEngine.Random.Range(Mathf.FloorToInt(_minSpawnRadius * 100), Mathf.FloorToInt(_maxSpawnRadius * 100))/100;

        Vector2 Position = new Vector2(Mathf.Cos(dir) * distance, Mathf.Sin(dir) * distance) + new Vector2(transform.position.x, transform.position.y);

        GameObject e = Instantiate(Enemy);
        e.transform.position = Position;
    }


    //private void OnDrawGizmosSelected() {

    //    if (MinSpawnRadius > MaxSpawnRadius) {  MinSpawnRadius = MaxSpawnRadius; }

    //    if (MaxSpawnRadius == MinSpawnRadius) {
    //        Gizmos.color = Color.green;
    //        Gizmos.DrawWireSphere(gameObject.transform.position, MaxSpawnRadius);
    //    } else {
    //        Gizmos.color = Color.yellow;
    //        Gizmos.DrawWireSphere(gameObject.transform.position, MinSpawnRadius);
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawWireSphere(gameObject.transform.position, MaxSpawnRadius);
    //    }
    //}
}
