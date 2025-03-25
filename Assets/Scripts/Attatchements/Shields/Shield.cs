using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shield : Attatchement
{

    #region Varaibles

    public int _health;

    #endregion

    #region Methods

    public void Damage(int amount) {
        _health -= amount;
        if (_health < 0) {
            _health = 0;
        }
    }

    #endregion

}
