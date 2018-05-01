using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelChange
{
    void LoadNavMesh();
    void AnimateToLevel();
    void BeginLevel();
}

public static class LevelChangeStatics
{
    public delegate void StartLevelChangingHandler(object sender);
    public delegate void LevelChangedHandler(object sender);
}