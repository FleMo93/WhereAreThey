using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameLogic
{
    GameLogicStatics.GameStates GetState();
    void StartGame();
    void ExitGame();
    event GameLogicStatics.GameStateChangedHandler GameStateChanged;
}

public static class GameLogicStatics
{
    public enum GameStates { WaitForPlayers, ReadyToStart, ChangeLevel, Fight, End }
    public delegate void GameStateChangedHandler(object sender, GameStates gameState);
}
