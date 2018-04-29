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
    IGameLogic gameLogic;
    ICharacterControlls characterInput;

    void Start ()
    {
        myPlayer = GetComponent<IPlayer>();
        playerController = GetComponent<IPlayerController>();
        gameLogic = GameObject.Find("ScriptHolder").GetComponent<IGameLogic>();
	}

    public void SetControlls(ICharacterControlls device)
    {
        characterInput = device;
    }

    public ICharacterControlls GetControlls()
    {
        return characterInput;
    }

    void Update ()
    {
        if(characterInput == null || !myPlayer.IsAlive())
        {
            return;
        }

        if(characterInput.IsCasting() && gameLogic.GetState() == GameLogicStatics.GameStates.Fight)
        {
            playerController.Cast();
        }

        playerController.Move(characterInput.MoveDirection());
	}
}
