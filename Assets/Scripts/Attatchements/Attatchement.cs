using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attatchement : MonoBehaviour
{
    #region Classes

    public enum AttatchementType {
        NONE,
        Gun
    }

    [Serializable]
    public class Info_Class {
        public string name;
        public string description;
        public AttatchementType type = AttatchementType.NONE;

        public Info_Class() { }
    }

    #endregion

    #region Variables

    [SerializeField]
    private Info_Class info;

    #endregion

    #region Methods

    public void Attatch(Transform parent, Vector3 localPosition) {
        transform.parent = parent;
        transform.localPosition = localPosition;
    }

    //public Info_Class GetInfo() { return info; }

    public string GetName() { return info.name; }
    public string GetDescription() { return info.description; }
    public AttatchementType GetAttatchementType() {  return info.type; }

    #endregion
}
