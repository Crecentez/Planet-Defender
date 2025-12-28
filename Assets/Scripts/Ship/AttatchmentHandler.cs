using Ship.Attachments;
using System.Collections.Generic;
using UnityEngine;

namespace Ship {
    [RequireComponent(typeof(Ship))]
    public class AttachmentHandler : MonoBehaviour {

        #region Variables

        // Private

        [SerializeField] private List<AttachmentConnection> Attachments = new();
        private const float GizmosSize = 0.02f;

        #endregion

        #region Methods

        public GameObject AddAttatchement(GameObject prefab) {

            Attachment a = prefab.GetComponent<Attachment>();
            if (a == null) {
                Debug.LogWarning("Attachment Prefab (" + prefab.name + ") did not contain a Attachment Component!");
                return null;
            }

            AttachmentConnection connection = GetConnection(a);
            if (connection == null) {
                Debug.LogWarning("Attachment Connection not found.");
                return null;
            }
                

            GameObject obj = Instantiate(prefab);
            a = obj.GetComponent<Attachment>();

            connection.Attatch(transform, a);

            return obj;
        }

        private AttachmentConnection GetConnection(Attachment attatchement) {

            foreach (AttachmentConnection connection in Attachments) {
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
            foreach (AttachmentConnection ac in Attachments) {
                switch (ac.type) {
                    case AttachmentType.Gun:
                        Gizmos.color = Color.red;
                        break;
                    case AttachmentType.Shield:
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
}

