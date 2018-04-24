using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreGame : MonoBehaviour
{
    [SerializeField]
    GameObject[] _Players;


    ISpawner spawner;
    List<InputDevice> registerDevices;
    bool started = false;

	void Start ()
    {
        spawner = GetComponent<ISpawner>();
        registerDevices = new List<InputDevice>();
        
    }

    private void InputManager_OnDeviceDetached(InputDevice obj)
    {
        if(registerDevices.Contains(obj))
        {
            registerDevices.Remove(obj);
        }
    }

    void Update ()
    {
        if(started)
        {
            return;
        }


		//if(InputManager.ActiveDevice.CommandIsPressed && registerDevices.Count > 1 && !started)
  //      {
  //          started = true;
  //          spawner.SpawnPlayersAndAI(registerDevices);
  //      }

  //      var device = InputManager.ActiveDevice;
  //      var x = InputManager.Devices;

  //      if (device == null)
  //      {
  //          return;
  //      }

  //      //Tarou hat hier eine fehlerhafte Zeile vor dem Coder entdeckt :)
  //      if (device.AnyButton)
  //      {
  //          if (!registerDevices.Contains(device))
  //          {
  //              registerDevices.Add(device);

  //              _TextPlayers.text = registerDevices.Count + " Players";

  //              if (registerDevices.Count > 1)
  //              {
  //                  _PressStartText.text = "Press 'start' to begin";
  //              }
  //          }
  //      }
    }
}
