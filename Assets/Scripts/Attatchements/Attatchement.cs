using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Attatchement : MonoBehaviour
{

    #region Classes

    public enum AttatchementType {
        NONE,
        Gun,
        Shield
    }

    #endregion

    #region Variables

    [SerializeField] private string _name;
    [SerializeField][TextArea] private string _description;
    [SerializeField] private AttatchementType _type;

    #endregion

    #region Methods

    public virtual void Attatch(Transform parent, Vector3 localPosition) {
        transform.parent = parent;
        transform.localPosition = localPosition;
    }


    public string GetName() { return _name; }
    public virtual string GetDescription() { return _description; }
    public AttatchementType GetAttatchementType() {  return _type; }

    #endregion

    
}
