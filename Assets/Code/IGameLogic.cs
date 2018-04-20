using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameLogic
{
    void PlayersSpawned();
    GameLogicEnum.GameStates GetState();
}

public static class GameLogicEnum
{
    public enum GameStates { WaitForSpawn, Fight, End }
}
