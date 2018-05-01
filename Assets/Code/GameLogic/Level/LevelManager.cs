using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour, ILevelManager
{
    [SerializeField]
    GameObject _MenueLevel;
    [SerializeField]
    GameObject[] _LevelsPrefabs;


    public event LevelManagerStatics.StartMenueLoading StartMenueLoading;
    public event LevelManagerStatics.MenueLoaded MenueLoaded;

    public event LevelManagerStatics.LevelChangedHandler LevelChanged;
    public event LevelManagerStatics.StartLevelChangingHandler StartLevelChanging;

    void Start()
    {

    }


    public void LoadNextLevel()
    {
        
    }

    public void SwitchToNextLevel()
    {

    }

    public void LoadMenue()
    {
        if(StartMenueLoading != null)
        {
            StartMenueLoading(this);
        }

        _MenueLevel.SetActive(true);

        if(MenueLoaded != null)
        {
            MenueLoaded(this);
        }
    }
}
