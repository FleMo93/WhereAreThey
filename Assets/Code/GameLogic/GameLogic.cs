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
    [SerializeField]
    private KeyboardControlls[] keyboardControlls;

    public event GameLogicStatics.GameStateChangedHandler GameStateChanged;

    private GameObject[] players;
    private List<IPlayerInput> playerInputs;

    private GameLogicStatics.GameStates state = GameLogicStatics.GameStates.WaitForPlayers;


    void Start()
    {
        playerInputs = new List<IPlayerInput>();

        foreach(GameObject player in _Players)
        {
            playerInputs.Add(player.GetComponent<IPlayerInput>());
        }
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

                int playersReady = 0;

                //set device for player if free
                foreach(GameObject player in _Players)
                {
                    IPlayerInput playerInput = player.GetComponent<IPlayerInput>();

                    if(playerInput.GetControlls() == null)
                    {
                        ICharacterControlls cc = new CharacterControlls();
                        cc.SetController(device);

                        playerInput.SetControlls(cc);

                        if(state == GameLogicStatics.GameStates.WaitForPlayers)
                        {
                            player.SetActive(true);
                        }

                        playersReady++;

                        if (playersReady >= 2 && state == GameLogicStatics.GameStates.WaitForPlayers)
                        {
                            state = GameLogicStatics.GameStates.ReadyToStart;

                            if(GameStateChanged != null)
                            {
                                GameStateChanged(this, state);
                            }
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

                foreach(GameObject player in _Players)
                {
                    IPlayerInput playerInput = player.GetComponent<IPlayerInput>();
                    if(playerInput.GetControlls() == null)
                    {
                        ICharacterControlls cc = new CharacterControlls();
                        cc.SetMouseKeyboard(kc);

                        playerInput.SetControlls(cc);

                        if(state == GameLogicStatics.GameStates.WaitForPlayers)
                        {
                            player.SetActive(true);
                        }

                        break;
                    }
                }
            }
        }
    }

    void CheckPlayerStats()
    {
        if (state == GameLogicStatics.GameStates.Fight)
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
                state = GameLogicStatics.GameStates.End;

                if (GameStateChanged != null)
                {
                    GameStateChanged(this, state);
                }
            }
        }

        if (state == GameLogicStatics.GameStates.End)
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
        return state;
    }

    public void StartGame()
    {
        state = GameLogicStatics.GameStates.ChangeLevel;

        if (GameStateChanged != null)
        {
            GameStateChanged(this, state);
        }

        Debug.Log("Start");
    }

    public void ExitGame()
    {
        Environment.Exit(0);
    }
}
