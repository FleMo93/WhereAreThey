using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour, IGameLogic
{
    private static IGameLogic _gameLogic;
    public static IGameLogic Get
    {
        get
        {
            if(_gameLogic == null)
            {
                _gameLogic = FindObjectOfType<GameLogic>();
            }

            return _gameLogic;
        }
    }


    [SerializeField]
    private float _TimeToRestart = 5f;


    private GameObject[] players;

    private GameLogicEnum.GameStates state = GameLogicEnum.GameStates.WaitForSpawn;

    public void PlayersSpawned()
    {
        if(state != GameLogicEnum.GameStates.WaitForSpawn)
        {
            return;
        }

        state = GameLogicEnum.GameStates.Fight;
        players = GameObject.FindGameObjectsWithTag(Tags.Player);
    }

    void Update ()
    {
		if(state == GameLogicEnum.GameStates.Fight)
        {
            int playersAlive = 0;

            foreach(GameObject player in players)
            {
                if(player.activeSelf)
                {
                    playersAlive++;
                }
            }

            if(playersAlive <= 1)
            {
                state = GameLogicEnum.GameStates.End;
            }
        }

        if(state == GameLogicEnum.GameStates.End)
        {
            _TimeToRestart -= Time.deltaTime;

            if(_TimeToRestart <= 0)
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
