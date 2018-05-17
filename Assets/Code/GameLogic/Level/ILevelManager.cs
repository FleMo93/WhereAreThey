using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelManager
{
    void LoadMenue();
    void LoadNextLevel();
    void UnloadLevel();

    event LevelManagerStatics.StartMenueLoading StartMenueLoading;
    event LevelManagerStatics.MenueLoaded MenueLoaded;

    event LevelManagerStatics.StartLevelChangingHandler StartLevelChanging;
    event LevelManagerStatics.LevelChangedHandler LevelChanged;

    event LevelManagerStatics.StartUnloadLevel StartUnloadLevel;
    event LevelManagerStatics.LevelUnloaded LevelUnloaded;
}

public static class LevelManagerStatics
{
    public delegate void StartMenueLoading(object sender);
    public delegate void MenueLoaded(object sender);

    public delegate void StartLevelChangingHandler(object sender);
    public delegate void LevelChangedHandler(object sender);

    public delegate void StartUnloadLevel(object sender);
    public delegate void LevelUnloaded(object sender);
}
