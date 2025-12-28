using UnityEngine;
using UnityEngine.InputSystem;

namespace Ship.Attachments.Guns {
    public class Gun : Attachment {

        #region Variables

        [Header("Input")]
        [SerializeField] protected InputActionAsset InputActions;

        protected InputActionMap _gunInputMap;
        protected InputAction _fireInput;

        #endregion

        #region Unity Methods

        protected virtual void Awake() {
            if (!InputActions) {
                Debug.LogError(gameObject.name + "does not have a InputActionAsset");
                return;
            }

            _gunInputMap = InputActions.FindActionMap("Gun");
            _fireInput = _gunInputMap.FindAction("Fire");
            Debug.Log("Gun input added");
        }

        protected virtual void OnEnable() {
            _fireInput.Enable();
        }
        protected virtual void OnDisable() {
            _fireInput.Disable();
        }

        #endregion

    }
}

