using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttatchementConnection {

    #region Varaibles

    public Vector3 offset;
    public Attatchement.AttatchementType type = Attatchement.AttatchementType.NONE;

    private Attatchement attatchement;

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

    public Attatchement GetAttatchement() { return attatchement; }

    public bool CompareType(Attatchement attatchement) {
        return type == Attatchement.AttatchementType.NONE || type == attatchement.GetAttatchementType();
    }

    #endregion
}
