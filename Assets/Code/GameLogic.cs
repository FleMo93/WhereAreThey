using InControl;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour, IGameLogic
{
    [SerializeField]
    private GameObject[] _Players;
    [SerializeField]
    private float _TimeToRestart = 5f;


    private GameObject[] players;

    private GameLogicEnum.GameStates state = GameLogicEnum.GameStates.Menue;

    void Start()
    {
        foreach(GameObject player in _Players)
        {
            player.SetActive(false);
        }

        InputManager.OnDeviceDetached += InputManager_OnDeviceDetached;
    }

    void Update ()
    {
        CheckPlayerStats();
        SetPlayerControlls();
	}

    void SetPlayerControlls()
    {
        foreach (InputDevice device in InputManager.Devices)
        {
            if (device.AnyButtonWasPressed)
            {
                bool deviceInUse = false;

                //check if device is already in use
                foreach (GameObject player in _Players)
                {
                    IPlayerInput playerInput = player.GetComponent<IPlayerInput>();

                    if (playerInput.GetDevice() == device)
                    {
                        deviceInUse = true;
                        break;
                    }
                }

                if(deviceInUse)
                {
                    continue;
                }

                //set device for player if free
                foreach(GameObject player in _Players)
                {
                    IPlayerInput playerInput = player.GetComponent<IPlayerInput>();

                    if(playerInput.GetDevice() == null)
                    {
                        playerInput.SetDevice(device);

                        if(state == GameLogicEnum.GameStates.Menue)
                        {
                            player.SetActive(true);
                        }
                        
                        break;
                    }
                }
            }
        }
    }

    private void InputManager_OnDeviceDetached(InputDevice device)
    {
        foreach(GameObject player in _Players)
        {
            IPlayerInput playerInput = player.GetComponent<IPlayerInput>();

            if(playerInput.GetDevice() == device)
            {
                playerInput.SetDevice(null);
                player.SetActive(false);
            }
        }
    }

    void CheckPlayerStats()
    {
        if (state == GameLogicEnum.GameStates.Fight)
        {
            int playersAlive = 0;

            foreach (GameObject player in players)
            {
                if (player.activeSelf)
                {
                    playersAlive++;
                }
            }

            if (playersAlive <= 1)
            {
                state = GameLogicEnum.GameStates.End;
            }
        }

        if (state == GameLogicEnum.GameStates.End)
        {
            _TimeToRestart -= Time.deltaTime;

            if (_TimeToRestart <= 0)
            {
                string currentSceneName = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(currentSceneName);
            }
        }
    }

    public GameLogicEnum.GameStates GetState()
    {
        return state;
    }
}
