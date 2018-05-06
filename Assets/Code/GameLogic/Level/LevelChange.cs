using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChange : MonoBehaviour, ILevelChange
{
    [SerializeField]
    private LevelElement[] _LevelElements;
    [SerializeField]
    private float _OffsetStart = 0.5f;
    [SerializeField]
    private RuntimeAnimatorController _AnimatorController;

    void Start()
    {

        //return;
        foreach(LevelElement le in _LevelElements)
        {

            Animator myAnimator = le.GameObject.AddComponent<Animator>();
            myAnimator.runtimeAnimatorController = _AnimatorController;
            myAnimator.applyRootMotion = true;
            myAnimator.Play("Load");
        }
    }

    public void LoadNavMesh()
    {
        throw new NotImplementedException();
    }

    public void AnimateToLevel()
    {

    }

    public void BeginLevel()
    {
        throw new NotImplementedException();
    }
}
