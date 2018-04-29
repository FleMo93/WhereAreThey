using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorStart : MonoBehaviour
{
    public IGameLogic gameLogic;
    private Light[] lights;
    private bool lightsOn = false;
    private float timeToFlicker = -1;

    void Start()
    {
        gameLogic = GameObject.Find("ScriptHolder").GetComponent<IGameLogic>();
        lights = GetComponents<Light>();

        gameLogic.GameStateChanged += GameLogic_GameStateChanged;
    }

    private void GameLogic_GameStateChanged(object sender, GameLogicStatics.GameStates gameState)
    {
        if(gameState == GameLogicStatics.GameStates.ReadyToStart && lightsOn == false)
        {
            lightsOn = true;

        }
    }

    void Update()
    {
        if(timeToFlicker > 0 && lightsOn == true)
        {

        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == Tags.Player)
        {
            gameLogic.StartGame();
        }
    }
}
