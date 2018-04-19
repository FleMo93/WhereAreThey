using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathVisualizer : MonoBehaviour, IPathVisualizer
{
    [SerializeField]
    private bool Visualize = true;
    [SerializeField]
    private GameObject _Start;
    [SerializeField]
    private GameObject _Target;

    private List<Vector3> paths;
    IPathfindingMesh pathfindingMesh;


    private void Start()
    {
        pathfindingMesh = FindObjectOfType<PathfindingShiftedMesh>();
    }

    private void Update()
    {
        if (_Start != null && _Target != null)
        {
            paths = pathfindingMesh.GetPath(_Start.transform.position, _Target.transform.position);
        }
    }

    void OnDrawGizmos()
    {
        if(paths == null || paths.Count == 0)
        {
            return;
        }

        Vector3 lastPoint = paths[0];
        for(int i = 1; i < paths.Count; i++)
        {
            Gizmos.DrawLine(lastPoint, paths[i]);
            lastPoint = paths[i];
        }
    }

    public void SetPaths(List<Vector3> paths)
    {
        this.paths = paths;
    }

    public void ShowPath(bool show)
    {
        Visualize = show;
    }
}
