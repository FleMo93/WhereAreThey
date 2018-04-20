using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(IPlayer))]
[RequireComponent(typeof(IPlayerController))]
public class PlayerInput : MonoBehaviour, IPlayerInput
{
    IPlayer myPlayer;
    IPlayerController playerController;
    InputDevice inputDevice;

    void Start ()
    {
        myPlayer = GetComponent<IPlayer>();
        playerController = GetComponent<IPlayerController>();
	}

    public void SetDevice(InputDevice device)
    {
        inputDevice = device;
    }
	
	void Update ()
    {
        if(inputDevice == null || !myPlayer.IsAlive())
        {
            return;
        }

        if(inputDevice.Action1)
        {
            playerController.Cast();
        }

        Vector3 v3 = new Vector3(
            inputDevice.LeftStick.Value.x,
            0,
            inputDevice.LeftStick.Value.y);

        if(inputDevice.DPadUp.IsPressed)
        {
            v3.z = +1;
        }

        if(inputDevice.DPadRight.IsPressed)
        {
            v3.x = +1;
        }

        if(inputDevice.DPadDown.IsPressed)
        {
            v3.z = -1;
        }

        if(inputDevice.DPadLeft.IsPressed)
        {
            v3.x = -1;
        }

        playerController.Move(v3);
	}
}
