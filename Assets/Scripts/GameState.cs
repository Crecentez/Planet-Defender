using Unity.Collections;
using UnityEngine;

public class GameState : MonoBehaviour
{

    #region Variables

    [SerializeField] private int Money = 0;

    #endregion

    #region Methods

    public void AddMoney(int money) {
        Money += money;
    }

    public void RemoveMoney(int money) {
        Money -= money;
    }

    public void SetMoney(int money) {
        Money = money;
    }

    #endregion
}
