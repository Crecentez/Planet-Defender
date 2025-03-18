using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttatchementHandler : MonoBehaviour
{

    #region Variables

    [SerializeField] private List<AttatchementConnection> _attatchements = new List<AttatchementConnection>();

    private bool _canUse = false;

    private const float GizmosSize = 0.1f;

    #endregion

    #region Unity Methods

    private void Start() {

        _canUse = true;
    }

    #endregion

    #region Methods


    #endregion

    #region Gizmos

    private void OnDrawGizmosSelected() {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.blue;
        foreach (AttatchementConnection ac in _attatchements) {

            Gizmos.DrawCube(ac.offset, new Vector3(GizmosSize, GizmosSize, GizmosSize));
        }
    }

    #endregion
}
