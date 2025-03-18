using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AttatchementConnection {

    #region Varaibles

    public Vector3 offset;
    public Attatchement.AttatchementType type = Attatchement.AttatchementType.NONE;

    public Attatchement attatchement { get; private set; }

    public AttatchementConnection() { attatchement = null; }
    
    #endregion

    #region Methods

    public bool Attatch(Attatchement attatchement) {
        if (this.attatchement == null && CompareType(attatchement)) {
            this.attatchement = attatchement;
            return true;
        }
        return false;
    }

    public Attatchement Detatch() {
        Attatchement a = this.attatchement;
        this.attatchement = null;
        return a;
    }

    public bool CompareType(Attatchement attatchement) {
        return type == Attatchement.AttatchementType.NONE || type == attatchement.GetAttatchementType();
    }

    #endregion
}
