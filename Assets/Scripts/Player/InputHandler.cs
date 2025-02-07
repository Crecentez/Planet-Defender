using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[Serializable]
public class KeyDown : UnityEvent<bool> { }

public class InputHandler : MonoBehaviour
{
    
    [Serializable]
    public class Control {
        public KeyCode keyCode1;
        public KeyCode keyCode2;
        public KeyDown keyDown;

        public Control(KeyCode keyCode1, KeyCode keyCode2, KeyDown keyDown) {
            this.keyCode1 = keyCode1;
            this.keyCode2 = keyCode2;
            this.keyDown = keyDown;
        }

        public Control(KeyCode keyCode1, KeyDown keyDown) {
            this.keyCode1 = keyCode1;
            this.keyDown = keyDown;
        }

        public Control(KeyCode keyCode1, KeyCode keyCode2) {
            this.keyCode1 = keyCode1;
            this.keyCode2 = keyCode2;
        }

        public Control(KeyCode keyCode1) { this.keyCode1 = keyCode1; }
    }

    [Serializable]
    public class ControlsClass {


        public Control ForwardGas = new Control(KeyCode.W);
        public Control BackwardGas = new Control(KeyCode.S);
        public Control SteerLeft = new Control(KeyCode.A);
        public Control SteerRight = new Control(KeyCode.D);

        public Control Boost = new Control(KeyCode.LeftShift);

        public Control Fire = new Control(KeyCode.Mouse0, KeyCode.K);

    }

    public ControlsClass Controls = new ControlsClass();

}
