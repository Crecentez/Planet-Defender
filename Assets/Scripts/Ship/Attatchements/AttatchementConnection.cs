using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ship.Attatchment {
    [Serializable]
    public class AttatchementConnection {

        #region Varaibles

        public Vector3 offset;
        public AttatchementType type = AttatchementType.NONE;

        [SerializeField] private Attatchement attatchement;

        public AttatchementConnection() { attatchement = null; }

        #endregion

        #region Methods

        public bool Attatch(Transform transform, Attatchement attatchement) {
            if (IsAvailable(attatchement)) {
                this.attatchement = attatchement;
                attatchement.Attatch(transform, offset);
                return true;
            }
            return false;
        }

        public Attatchement Detatch() {
            Attatchement a = this.attatchement;
            this.attatchement = null;
            return a;
        }

        public bool IsAvailable(Attatchement attatchement) {
            return this.attatchement == null && CompareType(attatchement.GetAttatchementType());
        }

        public bool CompareType(AttatchementType attatchementType) {
            return type == AttatchementType.NONE || type == attatchementType;
        }

        #endregion
    }

}