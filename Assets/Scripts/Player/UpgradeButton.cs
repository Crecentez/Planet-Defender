using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UpgradeButton : MonoBehaviour
{
    private Upgrader upgrader;
    //private UnityEvent onSelectEvent;
    private int upgradeIndex;

    public void AttatchInfo(Upgrader upgrader, int upgradeIndex) { this.upgrader = upgrader; this.upgradeIndex = upgradeIndex; }


    public void OnSelect() {
        upgrader.UseUpgrade(upgradeIndex);
         //onSelectEvent.Invoke();
    }
}
