using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathVisualizer : MonoBehaviour
{
    [SerializeField]
    private GameObject _Start;
    [SerializeField]
    private GameObject _Target;

    List<Vector3> paths;
    IPathfindingMesh pathfindingMesh;

    private void Start()
    {
        pathfindingMesh = FindObjectOfType<PathfindingShiftedMesh>();
    }

    private void Update()
    {
        paths = pathfindingMesh.GetPath(_Start.transform.position, _Target.transform.position);
        Debug.Log(paths.Count); 
    }

    void OnDrawGizmos()
    {
        if(paths != null && paths.Count == 0)
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
}
