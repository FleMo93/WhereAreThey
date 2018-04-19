using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IPlayerMotor))]
[RequireComponent(typeof(IMagic))]
public class PlayerController : MonoBehaviour, IPlayerController
{
    IPlayerMotor playerMotor;
    IMagic magic;

	void Start ()
    {
        playerMotor = GetComponent<IPlayerMotor>();
        magic = GetComponent<IMagic>();
	}
	

    public void Move(Vector3 direction)
    {
        playerMotor.Move(direction);
    }

    public void Cast()
    {
        magic.Cast();
    }
}
