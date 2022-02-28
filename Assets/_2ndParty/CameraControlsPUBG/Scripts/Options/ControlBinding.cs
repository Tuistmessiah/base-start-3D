using UnityEngine;

namespace CameraControlPUBG {
    [System.Serializable]
    public class ControlBinding
    {
        public KeyCode[] primary = new KeyCode[1], secondary;
        bool pressed = false;

        public bool IsPressBind() {
            bool primaryPressed = false, secondaryPressed = false; 

            // Primary
            if(primary.Length == 1) {
                if(Input.GetKey(primary[0])) primaryPressed = true;
            }
            else if(primary.Length == 2) {
                if(Input.GetKey(primary[0]) && Input.GetKey(primary[1])) primaryPressed = true;

            }
            // Secondary
        if(secondary.Length == 1) {
                if(Input.GetKey(secondary[0])) secondaryPressed = true;
            }
            else if(secondary.Length == 2) {
                if(Input.GetKey(secondary[0]) && Input.GetKey(primary[1])) secondaryPressed = true;
            }

            // Check KeyBindings
            if(primaryPressed || secondaryPressed) return true;

            return false;
        }

        public bool IsDownBind() {
            bool primaryPressed = false, secondaryPressed = false; 

            // Primary
            if(primary.Length == 1) {
                if(Input.GetKey(primary[0])) primaryPressed = true;
            }
            else if(primary.Length == 2) {
                if(Input.GetKey(primary[0]) && Input.GetKey(primary[1])) primaryPressed = true;

            }
            // Secondary
        if(secondary.Length == 1) {
                if(Input.GetKey(secondary[0])) secondaryPressed = true;
            }
            else if(secondary.Length == 2) {
                if(Input.GetKey(secondary[0]) && Input.GetKey(primary[1])) secondaryPressed = true;
            }

            // Check KeyBindings
            if(!pressed) {
                if(primaryPressed || secondaryPressed) {
                    pressed = true;
                    return true;
                }
            }
            else {
                if(!primaryPressed && !secondaryPressed) {
                    pressed = false;
                }   
            }

            return false;
        }
    }
}
