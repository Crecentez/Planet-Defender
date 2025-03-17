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

    [SerializeField]
    private Upgrader upgrader;

    [Header("Waves")]
    [SerializeField]
    private List<Wave> waves = new List<Wave>();
    [SerializeField]
    private Wave defaultWave = new Wave();
    [SerializeField]
    private List<LinearScaling> scaling = new List<LinearScaling>(); 
    [SerializeField]
    private int wave = -1;

    [Header("Spawning")]
    public float MinSpawnRadius = 10f;
    public float MaxSpawnRadius = 15f;
    [SerializeField]
    private List<GameObject> enemies = new List<GameObject>();

    #endregion

    #region Unity Methods

    private void Start() {
        upgrader.OfferUpgrade(2);
        //Invoke("StartNextWave", 5f);
    }

    #endregion

    #region Methods

    public void StartNextWave() {
        if (enemies.Count <= 0) {
            wave++;
            Debug.Log("Starting Wave " + (wave + 1).ToString());

            StartCoroutine(SpawnEnemies());
        } else {
            Debug.LogWarning("Wave Not Finished");
        }
    }

    public void CheckWaveStatus() {
        if (enemies.Count <= 0) {
            Debug.Log("Wave " + (wave + 1).ToString() + " Finished");
            if (upgrader != null) {
                upgrader.OfferUpgrade(wave);
            } 
            //Invoke("StartNextWave", 3f);
        }
    }

    public void EnemyKilled(GameObject enemy) {
        if (enemies.Contains(enemy)) {
            enemies.Remove(enemy);
        }
        CheckWaveStatus();
    }

    public EnemyScaling GetEnemyScaling() {
        LinearScaling ls = GetLinearScale();
        return (ls != null) ? ls.enemyScaling : null;
    }

    public LinearScaling GetLinearScale() {
        foreach (LinearScaling ls in scaling) {
            if (wave >= ls.MinWave) {
                return ls;
            }
        }
        return null;
    }

    public int GetWave() {
        return wave + 1;
    }

    private IEnumerator SpawnEnemies() {

        if (wave < waves.Count) {
            Wave currentWave = waves[wave];
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

                for (int i = 0; i < Mathf.FloorToInt(linearScaling.enemiesSpawnAmount * linearScaling.enemiesSpawnScale * (wave + 1)); i++) {
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
        float distance = UnityEngine.Random.Range(Mathf.FloorToInt(MinSpawnRadius * 100), Mathf.FloorToInt(MaxSpawnRadius * 100)) / 100;

        Vector2 Position = new Vector2(Mathf.Cos(dir) * distance, Mathf.Sin(dir) * distance) + new Vector2(transform.position.x, transform.position.y);

        GameObject e = Instantiate(Enemy);
        Enemy enemy = e.GetComponent<Enemy>();
        enemy.waveHandler = this;
        e.transform.position = Position;
        enemies.Add(e);
    }
    private void Spawn(GameObject Enemy, EnemyScaling enemyScaling) {

        float dir = UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad;
        float distance = UnityEngine.Random.Range(Mathf.FloorToInt(MinSpawnRadius * 100), Mathf.FloorToInt(MaxSpawnRadius * 100)) / 100;

        Vector2 Position = new Vector2(Mathf.Cos(dir) * distance, Mathf.Sin(dir) * distance) + new Vector2(transform.position.x, transform.position.y);

        GameObject e = Instantiate(Enemy);
        Enemy enemy = e.GetComponent<Enemy>();
        enemy.waveHandler = this;
        enemy.ScaleHealth(enemyScaling.healthScale * wave);
        e.transform.position = Position;
        enemies.Add(e);
    }

    private GameObject GetRandom(List<SpawnTableItem> spawnableEnemies,  int totalWeight) {
        

        int number = UnityEngine.Random.Range(0, totalWeight);

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
        if (MinSpawnRadius > MaxSpawnRadius) { MaxSpawnRadius = MinSpawnRadius + 1; }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, MaxSpawnRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, MinSpawnRadius);
    }

    #endregion

}
