using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawner
{
    void SpawnAI(ICollection<InputDevice> playerInputDevices);
    void AwakePlayer(GameObject player);
}
