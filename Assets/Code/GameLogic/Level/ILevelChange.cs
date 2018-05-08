using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelChange
{
    event LevelChangeStatics.NavMeshLoaded NavMeshLoaded;
    event LevelChangeStatics.LevelAnimated LevelAnimated;

    void LoadNavMesh();
    void AnimateToLevel();
    void BeginLevel();
}

public static class LevelChangeStatics
{
    public delegate void NavMeshLoaded(object sender);
    public delegate void LevelAnimated(object sender);
}