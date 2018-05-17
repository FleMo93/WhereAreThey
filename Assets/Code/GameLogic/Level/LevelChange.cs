using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChange : MonoBehaviour, ILevelChange
{
    [SerializeField, ReadOnly]
    private States state = States.Waiting;

    [SerializeField]
    private LevelElement[] _LevelElements;
    [SerializeField]
    private float _OffsetStart = 0.5f;
    [SerializeField]
    private RuntimeAnimatorController _AnimatorController;

    public event LevelChangeStatics.NavMeshLoaded NavMeshLoaded;
    public event LevelChangeStatics.LevelInAnimated LevelInAnimated;
    public event LevelChangeStatics.LevelOutAnimated LevelOutAnimated;

    private enum States { LoadAnimation, UnloadAnimation, LoadNavMesh, Waiting }
    private bool loadAnimationDone = false;
    float loadAnimationTimer = 0;

    void Start()
    {
        foreach (LevelElement le in _LevelElements)
        {
            Animator myAnimator = le.GameObject.AddComponent<Animator>();
            myAnimator.runtimeAnimatorController = _AnimatorController;
            myAnimator.applyRootMotion = true;
        }
    }

    public void LoadNavMesh()
    {
        throw new NotImplementedException();
    }

    public void AnimateToLevel()
    {
        if(loadAnimationDone || state != States.Waiting)
        {
            return;
        }

        state = States.LoadAnimation;
        loadAnimationTimer = 0;
    }

    public void BeginLevel()
    {
        throw new NotImplementedException();
    }

    void Update()
    {
        switch(state)
        {
            case States.LoadAnimation:
                LoadLevelAnimation();
                break;

            case States.UnloadAnimation:
                UnloadLevelAnimation();
                break;
        }
    }

    private void LoadLevelAnimation()
    {
        int animationsDone = 0;

        loadAnimationTimer += Time.deltaTime;

        foreach (LevelElement element in _LevelElements)
        {
            Animator animator = element.GameObject.GetComponent<Animator>();

            if (loadAnimationTimer >= element.TimeToRunOn &&
                animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                animator.Play("Load");
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Load Finished"))
            {
                animationsDone++;
            }
        }

        if (animationsDone == _LevelElements.Length)
        {
            state = States.Waiting;

            foreach (LevelElement element in _LevelElements)
            {
                Animator animator = element.GameObject.GetComponent<Animator>();
                animator.Play("Idle");
            }

            if (LevelInAnimated != null)
            {
                LevelInAnimated(this);
            }
        }
    }

    private void UnloadLevelAnimation()
    {
        int animationsDone = 0;

        loadAnimationTimer += Time.deltaTime;

        foreach (LevelElement element in _LevelElements)
        {
            Animator animator = element.GameObject.GetComponent<Animator>();

            if (loadAnimationTimer >= element.TimeToRunOn &&
                animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                animator.Play("Unload");
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Unload Finished"))
            {
                animationsDone++;
            }
        }

        if (animationsDone == _LevelElements.Length)
        {
            state = States.Waiting;

            foreach (LevelElement element in _LevelElements)
            {
                Animator animator = element.GameObject.GetComponent<Animator>();
                animator.Play("Idle");
            }

            if (LevelOutAnimated != null)
            {
                LevelOutAnimated(this);
            }
        }
    }

    public void AnimateOutOfLevel()
    {
        if(state == States.Waiting)
        {
            state = States.UnloadAnimation;
        }
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
