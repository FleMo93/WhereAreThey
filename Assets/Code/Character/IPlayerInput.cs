using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerInput
{
    void SetDevice(InputDevice device);
    InputDevice GetDevice();
}
