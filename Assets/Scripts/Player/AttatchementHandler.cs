using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class AttatchementHandler : MonoBehaviour
{

    #region Variables

    [SerializeField] private List<AttatchementConnection> _attatchements = new List<AttatchementConnection>();
    
    //[SerializeField] PlayerController _controller;

    [Header("Testing")]
    [SerializeField] private List<GameObject> _attatchementPrefabs = new List<GameObject>();

    private const float GizmosSize = 0.1f;



    #endregion

    #region Unity Methods

    private void Start() {
        foreach (GameObject go in _attatchementPrefabs) {
            if (AddAttatchement(go) != null) {
                Debug.Log("Attatchement Added");
            } else {
                Debug.Log("Attatchement not Added");
            }
        }
    }

    #endregion

    #region Methods

    public GameObject AddAttatchement(GameObject prefab) {

        Attatchement attatchement = prefab.GetComponent<Attatchement>();
        if (attatchement == null)
            return null;
        AttatchementConnection connection = GetConnection(attatchement);
        if (connection == null)
            return null;

        GameObject obj = Instantiate(prefab);
        attatchement = obj.GetComponent<Attatchement>();

        connection.Attatch(transform, attatchement);

        return obj;
    }

    private AttatchementConnection GetConnection(Attatchement attatchement) {

        foreach(AttatchementConnection connection in _attatchements) {
            if (connection.IsAvailable(attatchement)) {
                return connection;
            }
        }

        return null;
    }

    #endregion

    #region Gizmos

    private void OnDrawGizmosSelected() {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.blue;
        foreach (AttatchementConnection ac in _attatchements) {
            switch (ac.type) {
                case Attatchement.AttatchementType.Gun:
                    Gizmos.color = Color.red;
                    break;
                case Attatchement.AttatchementType.Shield:
                    Gizmos.color = Color.yellow;
                    break;
                default:
                    Gizmos.color = Color.white;
                    break;
            }
            Gizmos.DrawCube(ac.offset, new Vector3(GizmosSize, GizmosSize, GizmosSize));
        }
    }

    #endregion
}
