using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IPathfindingMesh))]
public class AIInput : MonoBehaviour
{
    [SerializeField]
    private float _MinTimeToNextPoint = 0f;

    [SerializeField]
    private float _MaxTimeToNextPoint = 120f;

    private IPathfindingMesh pathFindingMesh;
    private List<Vector3> waypoints;
    private float _TimeToNextWaypointRequest = 0f;
    private Vector3 target;

    void Start()
    {
        pathFindingMesh = FindObjectOfType<PathfindingShiftedMesh>();
    }

	void Update () {
		if(_TimeToNextWaypointRequest > 0)
        {
            _TimeToNextWaypointRequest -= Time.deltaTime;
        }

        if(_TimeToNextWaypointRequest <= 0 && waypoints == null)
        {
            target = pathFindingMesh.GetRandomPoint();
        }

        waypoints = pathFindingMesh.GetPath(this.transform.position, target);
	}
}
