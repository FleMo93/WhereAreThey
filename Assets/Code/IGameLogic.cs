using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameLogic
{
    GameLogicEnum.GameStates GetState();
}

public static class GameLogicEnum
{
    public enum GameStates { Menue, ChangeLevel, Fight, End }
}
