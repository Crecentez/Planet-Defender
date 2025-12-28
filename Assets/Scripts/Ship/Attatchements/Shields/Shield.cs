using UnityEngine;
using Damage;

namespace Ship.Attachments.Shields {

    [RequireComponent(typeof(Damageable))]
    public class Shield : Attachment {

        #region Varaibles

        [SerializeField] protected bool AttatchAtCenter = false;
        public bool EnemyShield { get; protected set; } = false;


        #endregion

        #region Methods

        public override void Attatch(Transform parent, Vector3 localPosition) {
            if (AttatchAtCenter) {
                transform.parent = parent;
                transform.position = Vector3.zero;
            } else {
                base.Attatch(parent, localPosition);
            }
        }


        #endregion

    }
}
