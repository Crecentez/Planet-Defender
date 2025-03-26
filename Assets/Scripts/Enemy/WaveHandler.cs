using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class WaveHandler : MonoBehaviour
{

    #region Classes

    [Serializable]
    public class SpawnTableItem {

        public int weight = 1;
        public GameObject enemy;

    }

    [Serializable]
    public class EnemyScaling {
        public float healthScale = 1.01f;
        public float playerDamageScale = 1.02f;
        public float planetDamageScale = 1.03f;

        public EnemyScaling() {

        }
    }

    [Serializable]
    public class LinearScaling {

        public EnemyScaling enemyScaling = new EnemyScaling();
        public float enemiesSpawnScale = 1.2f;
        public float enemiesSpawnAmount = 10;
        public float spawnSpeed = 0.5f;
        public int spawnAmountOnInterval = 2;
        public List<SpawnTableItem> enemySpawnTable = new List<SpawnTableItem> ();

        public int MinWave = 0;

        public LinearScaling() {

        }

    }

    [Serializable]
    private class Wave {

        //public EnemyScaling scaling = new EnemyScaling();
        public List<SpawnTableItem> spawnTable = new List<SpawnTableItem>();
        public int spawnIntervals = 5;
        public float spawnSpeed = 1f;
        public int intervalSpawnAmount = 2; // Amount to spawn for each spawn interval

        public Wave() { }
    }

    #endregion

    #region Variables

    //[SerializeField] private Upgrader upgrader;

    [Header("Waves")]
    [SerializeField] private List<Wave> _waves = new List<Wave>();
    [SerializeField] private Wave _defaultWave = new Wave();
    [SerializeField] private List<LinearScaling> _scaling = new List<LinearScaling>(); 
    [SerializeField] private int _wave = -1;

    [Header("Spawning")]
    [SerializeField] private float _minSpawnRadius = 10f;
    [SerializeField] private float _maxSpawnRadius = 15f;
    [SerializeField] private List<GameObject> _enemies = new List<GameObject>();

    #endregion

    #region Unity Methods

    private void Start() {
        Invoke("StartNextWave", 5f);
    }

    #endregion

    #region Methods

    public void StartNextWave() {
        if (_enemies.Count <= 0) {
            _wave++;
            Debug.Log("Starting Wave " + (_wave + 1).ToString());

            StartCoroutine(SpawnEnemies());
        } else {
            Debug.LogWarning("Wave Not Finished");
        }
    }

    public void CheckWaveStatus() {
        if (_enemies.Count <= 0) {
            Debug.Log("Wave " + (_wave + 1).ToString() + " Finished");
            Invoke("StartNextWave", 3f);
        }
    }

    public void EnemyKilled(GameObject enemy) {
        if (_enemies.Contains(enemy)) {
            _enemies.Remove(enemy);
        }
        CheckWaveStatus();
    }

    public EnemyScaling GetEnemyScaling() {
        LinearScaling ls = GetLinearScale();
        return (ls != null) ? ls.enemyScaling : null;
    }

    public LinearScaling GetLinearScale() {
        foreach (LinearScaling ls in _scaling) {
            if (_wave >= ls.MinWave) {
                return ls;
            }
        }
        return null;
    }

    public int GetWave() {
        return _wave + 1;
    }

    private IEnumerator SpawnEnemies() {

        if (_wave < _waves.Count) {
            Wave currentWave = _waves[_wave];
            if (currentWave != null) {
                int totalWeight = 0;
                for (int i = 0; i < currentWave.spawnTable.Count; i++) {
                    totalWeight += currentWave.spawnTable[i].weight;
                }

                for (int i = 0; i < currentWave.spawnIntervals; i++) {
                    yield return new WaitForSeconds(currentWave.spawnSpeed);
                    for (int j = 0; j < currentWave.intervalSpawnAmount; j++) {
                        Spawn(GetRandom(currentWave.spawnTable, totalWeight));
                    }
                }
            }
        } else {
            LinearScaling linearScaling = GetLinearScale();
            if (linearScaling != null) {

                int totalWeight = 0;
                for(int i = 0; i < linearScaling.enemySpawnTable.Count; i++) {
                    totalWeight += linearScaling.enemySpawnTable[i].weight;
                }

                for (int i = 0; i < Mathf.FloorToInt(linearScaling.enemiesSpawnAmount * linearScaling.enemiesSpawnScale * (_wave + 1)); i++) {
                    yield return new WaitForSeconds(linearScaling.spawnSpeed);
                    for (int j = 0; j < linearScaling.spawnAmountOnInterval; j++) {
                        Spawn(GetRandom(linearScaling.enemySpawnTable, totalWeight));
                    }
                }
            }
        }
    }

    private void Spawn(GameObject Enemy) {

        float dir = UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad;
        float distance = UnityEngine.Random.Range(Mathf.FloorToInt(_minSpawnRadius * 100), Mathf.FloorToInt(_maxSpawnRadius * 100)) / 100;

        Vector2 Position = new Vector2(Mathf.Cos(dir) * distance, Mathf.Sin(dir) * distance) + new Vector2(transform.position.x, transform.position.y);

        GameObject e = Instantiate(Enemy);
        Enemy enemy = e.GetComponent<Enemy>();
        enemy.waveHandler = this;
        e.transform.position = Position;
        _enemies.Add(e);
    }
    private void Spawn(GameObject Enemy, EnemyScaling enemyScaling) {

        float dir = UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad;
        float distance = UnityEngine.Random.Range(Mathf.FloorToInt(_minSpawnRadius * 100), Mathf.FloorToInt(_maxSpawnRadius * 100)) / 100;

        Vector2 Position = new Vector2(Mathf.Cos(dir) * distance, Mathf.Sin(dir) * distance) + new Vector2(transform.position.x, transform.position.y);

        GameObject e = Instantiate(Enemy);
        Enemy enemy = e.GetComponent<Enemy>();
        enemy.waveHandler = this;
        enemy.ScaleHealth(enemyScaling.healthScale * _wave);
        e.transform.position = Position;
        _enemies.Add(e);
    }

    private GameObject GetRandom(List<SpawnTableItem> spawnableEnemies,  int totalWeight) {
        

        int number = UnityEngine.Random.Range(0, totalWeight + 1);

        for (int i = 0; i < spawnableEnemies.Count; i++) {

            if (spawnableEnemies[i].weight >= number) {
                return spawnableEnemies[i].enemy;
            } else {
                number -= spawnableEnemies[i].weight;
            }
        }
        return null;

    }

    #endregion

    #region Gizmos

    private void OnDrawGizmosSelected() {
        if (_minSpawnRadius > _maxSpawnRadius) { _maxSpawnRadius = _minSpawnRadius + 1; }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _maxSpawnRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _minSpawnRadius);
    }

    #endregion

}
