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
    private float _LevelHeight = 0;


    public event LevelManagerStatics.StartMenueLoading StartMenueLoading;
    public event LevelManagerStatics.MenueLoaded MenueLoaded;

    public event LevelManagerStatics.LevelChangedHandler LevelChanged;
    public event LevelManagerStatics.StartLevelChangingHandler StartLevelChanging;

    private ILevelChange _actualLevel;
    private ILevelChange _nextLevel;


    private void LevelAnimated(object sender)
    {
        if (_nextLevel.GetGameObject() == _MenueLevel)
        {
            if (MenueLoaded != null)
            {
                MenueLoaded(this);
            }
        }
        else
        {
            if(LevelChanged != null)
            {
                LevelChanged(this);
            }
        }

        _actualLevel = _nextLevel;
        _nextLevel = null;
    }

    public void LoadNextLevel()
    {
        GameObject level = Instantiate(_LevelsPrefabs[0]);
        level.transform.position = new Vector3(0, _LevelHeight, 0);
        _nextLevel = level.GetComponent<ILevelChange>();
    }

    public void StartNextLevel()
    {
        _actualLevel = _nextLevel;
        _nextLevel = null;
        LoadNextLevel();
    }

    public void LoadMenue()
    {
        if (StartMenueLoading != null)
        {
            StartMenueLoading(this);
        }

        if(_nextLevel != null)
        {
            Destroy(_nextLevel.GetGameObject());
        }

        _nextLevel = _MenueLevel.GetComponent<ILevelChange>();
        _nextLevel.LevelInAnimated += LevelAnimated;

        if(_actualLevel != null)
        {
            UnloadLevel();

            _actualLevel.LevelOutAnimated += (object sender) =>
            {
                _nextLevel.AnimateToLevel();
            }
        }
        else
        {
            _nextLevel.AnimateToLevel();
        }
    }

    private void UnloadLevel()
    {
        _actualLevel.AnimateOutOfLevel();
    }
}