using System;
using UnityEngine;

namespace Ship.Attachments {
    [Serializable]
    public class AttachmentConnection {

        #region Varaibles

        public Vector3 offset;
        public AttachmentType type = AttachmentType.NONE;

        [SerializeField] private Attachment attatchement;

        public AttachmentConnection() { attatchement = null; }

        #endregion

        #region Methods

        public bool Attatch(Transform transform, Attachment attatchement) {
            if (IsAvailable(attatchement)) {
                this.attatchement = attatchement;
                attatchement.Attatch(transform, offset);
                return true;
            }
            return false;
        }

        public Attachment Detatch() {
            Attachment a = this.attatchement;
            this.attatchement = null;
            return a;
        }

        public bool IsAvailable(Attachment attatchement) {
            return this.attatchement == null && CompareType(attatchement.GetAttatchementType());
        }

        public bool CompareType(AttachmentType attatchementType) {
            return type == AttachmentType.NONE || type == attatchementType;
        }

        #endregion
    }

}

