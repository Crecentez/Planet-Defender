using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Upgrader : MonoBehaviour
{
    [Serializable]
    private class Upgrade {
        [Header("Wave Settings")]
        public int MinimumWave = 0;
        public int MaximumWave = 100;

        [Header("Rolling")]
        public int Weight = 1;
        public int MaximumUses = 5;

        [Header("Functionalidy")]
        public UnityEvent OnSelect;
        public GameObject UpgradePrefab;

        private int TotalUses = 0;
        

        Upgrade(int weight) { this.Weight = weight; }

        public bool IsUsable(int wave) {
            //return upgrade.MinimumWave <= wave && upgrade.MaximumWave >= wave;
            return (MinimumWave <= wave) && (MaximumWave >= wave) && (TotalUses < MaximumUses);
        }

        public void AddUse() { TotalUses++; }
    }

    //public GameObject UpgradeSelectPrefab;

    [SerializeField]
    private List<Upgrade> Upgrades = new List<Upgrade>();

    [SerializeField]
    private WaveHandler waveHandler;

    [SerializeField]
    private const int OFFER_COUNT = 3;

    [SerializeField]
    private int optionsPerWave = 25;

    private void Start() {
        
    }

    public void UseUpgrade(int upgradeIndex) {
        Upgrade upgrade = Upgrades[upgradeIndex];
        upgrade.AddUse();
        upgrade.OnSelect.Invoke();
        Close();
        waveHandler.StartNextWave();
    }

    public void AddUse(int upgradeIndex) {
        Upgrade upgrade = Upgrades[upgradeIndex];
        upgrade.AddUse();
    }

    public void Close() {
        for (int i = 0; i < gameObject.transform.childCount; i++) {
            Destroy(gameObject.transform.GetChild(i).gameObject);
        }
    }
    
    public void OfferUpgrade(int wave) {
        for (int i = 0; i < (wave - (wave % optionsPerWave)) / optionsPerWave + 3; i++) {
            int upgradeIndex = GetRandomUpgrade(wave);
            if (upgradeIndex == -1) { Debug.LogWarning("NO UPGRADE FOUND"); continue; }
            Upgrade upgrade = Upgrades[upgradeIndex];
            GameObject gm = Instantiate(upgrade.UpgradePrefab, gameObject.transform);
            UpgradeButton ub = gm.GetComponent<UpgradeButton>();
            ub.AttatchInfo(this, upgradeIndex);
        }
    }


    private int GetRandomUpgrade(int wave) {
        int totalWeight = 0;
        foreach (Upgrade upgrade in Upgrades) {
            //Debug.Log(upgrade.Weight.ToString());
            //Debug.Log(upgrade.MinimumWave.ToString() + " <= " + wave.ToString() + ">=" + upgrade.MaximumWave.ToString() + " :: " + (upgrade.MinimumWave <= wave).ToString());
            if (upgrade.IsUsable(wave)) {
                totalWeight += upgrade.Weight;
            }
        }
        
        int randomWeight = UnityEngine.Random.Range(1, totalWeight + 1);
        
        for (int i = 0; i < Upgrades.Count; i++) {
            Upgrade upgrade = Upgrades[i];
            if (upgrade.IsUsable(wave)) {
                if (upgrade.Weight >= randomWeight) {
                    return i;
                }
                randomWeight -= upgrade.Weight; 
            }
        }
        return -1;
    }
}
