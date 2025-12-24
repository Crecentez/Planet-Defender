using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ship.Attachments {
    public enum AttachmentType {
        NONE,
        Gun,
        Shield
    }   

    public class Attachment : MonoBehaviour {

        
        #region Variables

        [SerializeField] private string _name;
        [SerializeField][TextArea] private string _description;
        [SerializeField] private AttachmentType _type;

        #endregion

        #region Methods

        public virtual void Attatch(Transform parent, Vector3 localPosition) {
            transform.parent = parent;
            transform.localPosition = localPosition;
        }


        public string GetName() { return _name; }
        public virtual string GetDescription() { return _description; }
        public AttachmentType GetAttatchementType() { return _type; }

        #endregion


    }
}
