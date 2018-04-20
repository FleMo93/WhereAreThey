using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawner
{
    void SpawnPlayersAndAI(ICollection<InputDevice> playerInputDevices);	
}
