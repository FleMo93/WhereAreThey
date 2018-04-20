using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreGame : MonoBehaviour
{
    [SerializeField]
    Text _PressStartText;
    [SerializeField]
    GameObject _StartUI;
    [SerializeField]
    Text _TextPlayers;

    ISpawner spawner;
    List<InputDevice> devices;
    bool started = false;

	void Start ()
    {
        spawner = GetComponent<ISpawner>();
        devices = new List<InputDevice>();
        _PressStartText.gameObject.SetActive(false);
        _TextPlayers.text = "";
        InputManager.OnDeviceDetached += InputManager_OnDeviceDetached;
	}

    private void InputManager_OnDeviceDetached(InputDevice obj)
    {
        if(devices.Contains(obj))
        {
            devices.Remove(obj);
        }
    }

    void Update ()
    {
        if(started)
        {
            return;
        }
        
        foreach(var device in InputManager.Devices)
        {
            if(device.AnyButton)
            {
                if(!devices.Contains(device))
                {
                    devices.Add(device);

                    _TextPlayers.text = devices.Count + " Players";

                    if(devices.Count > 1)
                    {
                        _PressStartText.gameObject.SetActive(true);
                    }
                }
            }
        }

		if(InputManager.ActiveDevice.CommandIsPressed && devices.Count > 1 && !started)
        {
            started = true;
            spawner.SpawnPlayersAndAI(devices);
            _StartUI.SetActive(false);
        }
    }
}
