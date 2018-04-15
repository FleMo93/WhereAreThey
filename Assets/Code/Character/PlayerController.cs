using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IPlayerMotor))]
public class PlayerController : MonoBehaviour, IPlayerController
{
    IPlayerMotor playerMotor;

	void Start ()
    {
        playerMotor = GetComponent<IPlayerMotor>();
	}
	

    public void Move(Vector3 direction)
    {
        playerMotor.Move(direction);
    }
}
