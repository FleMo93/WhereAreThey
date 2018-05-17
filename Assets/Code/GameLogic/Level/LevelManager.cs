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
    [SerializeField]
    private float _LoadLevelHeight = 100;


    public event LevelManagerStatics.StartMenueLoading StartMenueLoading;
    public event LevelManagerStatics.MenueLoaded MenueLoaded;

    public event LevelManagerStatics.LevelChangedHandler LevelChanged;
    public event LevelManagerStatics.StartLevelChangingHandler StartLevelChanging;

    public event LevelManagerStatics.StartUnloadLevel StartUnloadLevel;
    public event LevelManagerStatics.LevelUnloaded LevelUnloaded;

    private ILevelChange _actualLevel;
    private ILevelChange _nextLevel;


    private void _nextLevel_LevelAnimated(object sender)
    {
        if(MenueLoaded != null)
        {
            MenueLoaded(this);
            _actualLevel = _nextLevel;
            _nextLevel = null;
        }
    }

    public void LoadNextLevel()
    {
        GameObject level = Instantiate(_LevelsPrefabs[0]);
        level.transform.position = new Vector3(0, _LoadLevelHeight, 0);
        _nextLevel = level.GetComponent<ILevelChange>();
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

        _MenueLevel.transform.position = new Vector3(0, _LoadLevelHeight, 0);
        _MenueLevel.SetActive(true);

        if(_nextLevel != null)
        {
            _nextLevel.LevelInAnimated -= _nextLevel_LevelAnimated;
        }

        _nextLevel = _MenueLevel.GetComponent<ILevelChange>();
        _nextLevel.AnimateToLevel();

        _nextLevel.LevelInAnimated += _nextLevel_LevelAnimated;
    }

    public void UnloadLevel()
    {
        if(StartUnloadLevel != null)
        {
            StartUnloadLevel(this);
        }

        _actualLevel.LevelOutAnimated += (object sender) =>
        {
            if(LevelUnloaded != null)
            {
                LevelUnloaded(this);
            }

            Destroy(_actualLevel.GetGameObject());

            _actualLevel = _nextLevel;
            _nextLevel = null;
        };

        _actualLevel.AnimateOutOfLevel();
    }
}
