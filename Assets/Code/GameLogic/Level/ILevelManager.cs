using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelManager
{
    void LoadMenue();
    void LoadNextLevel();
    void StartNextLevel();

    event LevelManagerStatics.StartMenueLoading StartMenueLoading;
    event LevelManagerStatics.MenueLoaded MenueLoaded;

    event LevelManagerStatics.StartLevelChangingHandler StartLevelChanging;
    event LevelManagerStatics.LevelChangedHandler LevelChanged;
}

public static class LevelManagerStatics
{
    public delegate void StartMenueLoading(object sender);
    public delegate void MenueLoaded(object sender);

    public delegate void StartLevelChangingHandler(object sender);
    public delegate void LevelChangedHandler(object sender);
}
