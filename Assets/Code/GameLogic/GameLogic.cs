using InControl;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(ISpawner))]
[RequireComponent(typeof(ILevelManager))]
public class GameLogic : MonoBehaviour, IGameLogic
{
    [SerializeField, ReadOnly]
    private GameLogicStatics.GameStates _State = GameLogicStatics.GameStates.ChangeLevel;
    [SerializeField]
    private GameObject[] _Players;
    [SerializeField]
    private float _TimeToRestart = 5f;
    [SerializeField]
    private KeyboardControlls[] keyboardControlls;


    public event GameLogicStatics.GameStateChangedHandler GameStateChanged;

    private List<IPlayerInput> playerInputs;
    private ISpawner spawner;
    private ILevelManager levelManager;

    void Start()
    {
        playerInputs = new List<IPlayerInput>();
        spawner = GetComponent<ISpawner>();
        levelManager = GetComponent<ILevelManager>();

        foreach(GameObject player in _Players)
        {
            playerInputs.Add(player.GetComponent<IPlayerInput>());
        }

        levelManager.MenueLoaded += (sender) =>
        {
            _State = GameLogicStatics.GameStates.WaitForPlayers;
            FireGameStateChangedEvent();
        };

        levelManager.LoadMenue();
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
                foreach (IPlayerInput playerInput in playerInputs)
                {
                    if (playerInput.GetControlls() != null && playerInput.GetControlls().GetDevice() == device)
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

                    if(playerInput.GetControlls() == null)
                    {
                        ICharacterControlls cc = new CharacterControlls();
                        cc.SetController(device);

                        playerInput.SetControlls(cc);

                        if(_State == GameLogicStatics.GameStates.WaitForPlayers)
                        {
                            spawner.AwakePlayer(player);
                        }
                    }
                }
            }
        }


        foreach(KeyboardControlls kc in keyboardControlls)
        {
            if(Input.GetKeyDown(kc.Cast))
            {
                bool deviceInUse = false;

                foreach(GameObject player in _Players)
                {
                    IPlayerInput playerInput = player.GetComponent<IPlayerInput>();

                    if(playerInput.GetControlls() != null && playerInput.GetControlls().GetKeyboardControlls() == kc)
                    {
                        deviceInUse = true;
                        break;
                    }
                }

                if(deviceInUse)
                {
                    continue;
                }

                foreach (GameObject player in _Players)
                {
                    IPlayerInput playerInput = player.GetComponent<IPlayerInput>();
                    if(playerInput.GetControlls() == null)
                    {
                        ICharacterControlls cc = new CharacterControlls();
                        cc.SetMouseKeyboard(kc);

                        playerInput.SetControlls(cc);

                        if(_State == GameLogicStatics.GameStates.WaitForPlayers)
                        {
                            spawner.AwakePlayer(player);
                        }

                        break;
                    }
                }
            }
        }
    }

    void CheckPlayerStats()
    {
        if(_State == GameLogicStatics.GameStates.WaitForPlayers)
        {
            int playersReady = 0;

            foreach (GameObject player in _Players)
            {
                if(player.activeSelf)
                {
                    playersReady++;
                }
            }

            if (playersReady >= 2 && _State == GameLogicStatics.GameStates.WaitForPlayers)
            {
                _State = GameLogicStatics.GameStates.ReadyToStart;
                FireGameStateChangedEvent();
            }
        }

        if (_State == GameLogicStatics.GameStates.Fight)
        {
            int playersAlive = 0;

            foreach (GameObject player in _Players)
            {
                if (player.activeSelf)
                {
                    playersAlive++;
                }
            }

            if (playersAlive <= 1)
            {
                _State = GameLogicStatics.GameStates.End;
                FireGameStateChangedEvent();
            }
        }

        if (_State == GameLogicStatics.GameStates.End)
        {
            _TimeToRestart -= Time.deltaTime;

            if (_TimeToRestart <= 0)
            {
                string currentSceneName = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(currentSceneName);
            }
        }
    }

    public GameLogicStatics.GameStates GetState()
    {
        return _State;
    }

    public void StartGame()
    {
        _State = GameLogicStatics.GameStates.ChangeLevel;
        FireGameStateChangedEvent();
    }

    public void ExitGame()
    {
        if (Application.isPlaying && !Application.isEditor)
        {
            Environment.Exit(0);
        }
    }

    private void FireGameStateChangedEvent()
    {
        if(GameStateChanged != null)
        {
            GameStateChanged(this, _State);
        }
    }
}
