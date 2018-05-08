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

    public event LevelChangeStatics.NavMeshLoaded NavMeshLoaded;
    public event LevelChangeStatics.LevelAnimated LevelAnimated;

    private enum States { LoadAnimation, UnloadAnimation, LoadNavMesh, Waiting }
    private States state = States.Waiting;
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
        if(state == States.LoadAnimation)
        {
            int animationsDone = 0;

            loadAnimationTimer += Time.deltaTime;
            
            foreach(LevelElement element in _LevelElements)
            {
                Animator animator = element.GameObject.GetComponent<Animator>();

                if (loadAnimationTimer >= element.TimeToRunOn &&
                    animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    animator.Play("Load");
                }
                else if(animator.GetCurrentAnimatorStateInfo(0).IsName("Load Finish"))
                {
                    animationsDone++;
                }
            }

            if(animationsDone == _LevelElements.Length - 1)
            {
                state = States.Waiting;

                if(LevelAnimated != null)
                {
                    LevelAnimated(this);
                }
            }
        }
    }
}
