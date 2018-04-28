using System;
using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine;

public class CharacterControlls : ICharacterControlls
{
    KeyboardControlls keyboardControlls;
    InputDevice inputDevice;

    public bool IsCasting()
    {
        if(keyboardControlls != null && Input.GetKeyDown(keyboardControlls.Cast) ||
            inputDevice != null && inputDevice.Action1)
        {
            return true;
        }

        return false;
    }

    public Vector3 MoveDirection()
    {
        if(keyboardControlls != null)
        {
            Vector3 v3 = new Vector3();
            if(Input.GetKey(keyboardControlls.Up))
            {
                v3.z += 1;
            }

            if (Input.GetKey(keyboardControlls.Right))
            {
                v3.x += 1;
            }

            if (Input.GetKey(keyboardControlls.Down))
            {
                v3.z -= 1;
            }

            if (Input.GetKey(keyboardControlls.Left))
            {
                v3.x -= 1;
            }

            return v3;
        }
        else if(inputDevice != null)
        {
            return new Vector3(inputDevice.LeftStick.X, 0, inputDevice.LeftStick.Y);
        }

        return Vector3.zero;
    }

    public void SetController(InputDevice inputDevice)
    {
        if(this.inputDevice != null)
        {
            throw new Exception("Input Device already in use. Set null before.");
        }

        if(keyboardControlls != null)
        {
            throw new Exception("Input device cant be set when keyboard controlls is already in use.");
        }

        this.inputDevice = inputDevice;
    }

    public void SetMouseKeyboard(KeyboardControlls keyboardControlls)
    {
        if (this.keyboardControlls != null)
        {
            throw new Exception("Keyboard controlls already in use. Set null before.");
        }

        this.keyboardControlls = keyboardControlls;
    }

    public InputDevice GetDevice()
    {
        return inputDevice;
    }

    public KeyboardControlls GetKeyboardControlls()
    {
        return keyboardControlls;
    }
}
