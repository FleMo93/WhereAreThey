using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelChange
{
    event LevelChangeStatics.NavMeshLoaded NavMeshLoaded;
    event LevelChangeStatics.LevelInAnimated LevelInAnimated;
    event LevelChangeStatics.LevelOutAnimated LevelOutAnimated;

    void LoadNavMesh();
    void AnimateToLevel();
    void BeginLevel();
    void AnimateOutOfLevel();
    GameObject GetGameObject();
}

public static class LevelChangeStatics
{
    public delegate void NavMeshLoaded(object sender);
    public delegate void LevelInAnimated(object sender);
    public delegate void LevelOutAnimated(object sender);
}