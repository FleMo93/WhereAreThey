using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorStart : MonoBehaviour
{
    private IFlickerLights flickerLights;
    private IGameLogic gameLogic;
    private ParticleSystem[] particleSystems;
    private bool triggerEnabled = false;

    void Start()
    {
        gameLogic = GameObject.Find("ScriptHolder").GetComponent<IGameLogic>();
        flickerLights = GetComponent<IFlickerLights>();
        gameLogic.GameStateChanged += GameLogic_GameStateChanged;
        particleSystems = GetComponentsInChildren<ParticleSystem>();
    }

    private void GameLogic_GameStateChanged(object sender, GameLogicStatics.GameStates gameState)
    {
        if(gameState == GameLogicStatics.GameStates.ReadyToStart)
        {
            triggerEnabled = true;
            flickerLights.SetState(true);
            
            foreach(var ps in particleSystems)
            {
                ps.Play(true);
            }
        }
        else if(gameState == GameLogicStatics.GameStates.WaitForPlayers)
        {
            triggerEnabled = false;
            flickerLights.SetState(false);

            foreach(var ps in particleSystems)
            {
                ps.Stop();
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if(!triggerEnabled)
        {
            return;
        }

        if(collider.tag == Tags.Player)
        {
            gameLogic.StartGame();
        }
    }
}
