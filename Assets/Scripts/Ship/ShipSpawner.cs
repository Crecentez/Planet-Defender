using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.VisualScripting;
using Ship.Attachments;

namespace Ship {

    [Serializable]
    public class ShipSpawnInfo {
        public GameObject ShipPrefab;
        public List<GameObject> Attachments = new();
    }

    public class ShipSpawner : MonoBehaviour {

        #region Variables
        [Header("Instant Spawn Settings")]
        [SerializeField] private bool InstantlySpawnShip = false;
        [SerializeField] private ShipSpawnInfo SpawningShip = new ShipSpawnInfo();

        #endregion

        #region Unity Methods

        private void Start() {
            if (InstantlySpawnShip) {
                SpawnShip(SpawningShip, new Vector3(0, 1, 0));
            }
        }

        #endregion

        #region Methods

        public void SpawnShip(ShipSpawnInfo info, Vector2 position) {
            GameObject ship = Instantiate(info.ShipPrefab); 
            ship.transform.position = position;
            if (!GameObject.FindGameObjectWithTag("Player"))
                ship.tag = "Player";
            if (info.Attachments.Count > 0 && ship.GetComponent<AttachmentHandler>()) {
                AttachmentHandler attachmentHandler = ship.GetComponent<AttachmentHandler>();
                foreach(GameObject attachmentPrefab in info.Attachments) {
                    Debug.Log(attachmentPrefab.name);
                    attachmentHandler.AddAttatchement(attachmentPrefab);
                }
            }
        }

        #endregion
    }
}
