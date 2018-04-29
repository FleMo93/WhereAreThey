using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterControlls
{
    InputDevice GetDevice();
    void SetController(InputDevice device);
    KeyboardControlls GetKeyboardControlls();
    void SetMouseKeyboard(KeyboardControlls keyboardControlls);
    bool IsCasting();
    Vector3 MoveDirection();
}
