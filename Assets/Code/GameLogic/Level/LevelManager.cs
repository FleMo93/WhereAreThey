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

    private ILevelChange _actualLevel;
    private ILevelChange _nextLevel;

    void Start()
    {
    }

    private void _nextLevel_LevelAnimated(object sender)
    {
        if(MenueLoaded != null)
        {
            MenueLoaded(this);
        }
    }

    public void LoadNextLevel()
    {
        
    }

    public void SwitchToNextLevel()
    {
        _actualLevel = _nextLevel;
        _nextLevel = null;
    }

    public void LoadMenue()
    {
        if(StartMenueLoading != null)
        {
            StartMenueLoading(this);
        }

        _MenueLevel.transform.position = new Vector3(0, 100, 0);
        _MenueLevel.SetActive(true);

        if(_nextLevel != null)
        {
            _nextLevel.LevelAnimated -= _nextLevel_LevelAnimated;
        }

        _nextLevel = _MenueLevel.GetComponent<ILevelChange>();
        _nextLevel.AnimateToLevel();

        _nextLevel.LevelAnimated += _nextLevel_LevelAnimated;
    }
}
