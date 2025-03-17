using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[Serializable]
public class KeyDown : UnityEvent<bool> { }

public class InputHandler : MonoBehaviour
{

    #region Classes

    [Serializable]
    public class Control {
        public KeyCode keyCode1;
        public KeyCode keyCode2;

        //[Tooltip("Called once when key is pressed")]
        //public UnityAction onDown;
        //[Tooltip("Called once when key is released")]
        //public UnityAction onUp;
        //[Tooltip("Called Every Frame when down")]
        //public UnityAction onInput;

        public Control(KeyCode keyCode1, KeyCode keyCode2) {
            this.keyCode1 = keyCode1;
            this.keyCode2 = keyCode2;
        }

        public Control(KeyCode keyCode1) { this.keyCode1 = keyCode1; }

        public bool GetKey() {
            if (keyCode1 != KeyCode.None && Input.GetKey(keyCode1)) return true;
            if (keyCode2 != KeyCode.None && Input.GetKey(keyCode2)) return true;
            return false;
        }

        public bool GetKeyDown() {
            if (keyCode1 != KeyCode.None && Input.GetKeyDown(keyCode1)) return true;
            if (keyCode2 != KeyCode.None && Input.GetKeyDown(keyCode2)) return true;
            return false;
        }
        public bool GetKeyUp() {
            if (keyCode1 != KeyCode.None && Input.GetKeyUp(keyCode1)) return true;
            if (keyCode2 != KeyCode.None && Input.GetKeyUp(keyCode2)) return true;
            return false;
        }

        //public void Fire() {
        //    if (onDown != null && (Input.GetKeyDown(keyCode1) || Input.GetKeyDown(keyCode2))) onDown.Invoke();
        //    if (onUp != null && (Input.GetKeyUp(keyCode1) || Input.GetKeyUp(keyCode2))) onUp.Invoke();
        //    if (onInput != null && (Input.GetKey(keyCode1) || Input.GetKey(keyCode2))) onInput.Invoke();
        //}
    }

    [Serializable]
    public class ControlsClass {


        public Control ForwardGas = new Control(KeyCode.W);
        public Control BackwardGas = new Control(KeyCode.S);
        public Control SteerLeft = new Control(KeyCode.A);
        public Control SteerRight = new Control(KeyCode.D);

        public Control Boost = new Control(KeyCode.LeftShift);

        public Control Fire = new Control(KeyCode.K);

    }

    #endregion

    #region Variables

    public ControlsClass Controls = new ControlsClass();

    #endregion

}
