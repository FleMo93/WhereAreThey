using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(IPathfindingMesh))]
[RequireComponent(typeof(IPlayerController))]
[RequireComponent(typeof(IPathVisualizer))]
public class AIInput : MonoBehaviour
{
    [SerializeField]
    private float _MinTimeToNextPoint = 0f;
    [SerializeField]
    private float _MaxTimeToNextPoint = 120f;
    [SerializeField]
    private float _WayPointReachedInRange = 0.2f;

    private IPathfindingMesh pathFindingMesh;
    private IPlayerController playerController;
    private IPathVisualizer pathVisualizer;
    private List<Vector3> waypoints;
    private float _TimeToNextWaypointRequest = 0f;
    private Vector3 target;

    private enum Status { Wait, Move }
    private Status status;

    void Start()
    {
        pathFindingMesh = FindObjectOfType<PathfindingShiftedMesh>();
        status = Status.Wait;
        playerController = GetComponent<IPlayerController>();
        pathVisualizer = GetComponent<IPathVisualizer>();
        _TimeToNextWaypointRequest = Random.Range(_MinTimeToNextPoint, _MaxTimeToNextPoint);
    }

	void Update () {
		if(status == Status.Wait && _TimeToNextWaypointRequest > 0)
        {
            _TimeToNextWaypointRequest -= Time.deltaTime;
        }
        
        if(_TimeToNextWaypointRequest <= 0)
        {
            target = pathFindingMesh.GetRandomPoint();
            _TimeToNextWaypointRequest = Random.Range(_MinTimeToNextPoint, _MaxTimeToNextPoint);
            waypoints = pathFindingMesh.GetPath(this.transform.position, target);
            status = Status.Move;
        }

        if(status == Status.Move)
        {
            Move();
        }       
	}

    void Move()
    {
        bool nextWayPointFound = false;
        Vector3 nextWaypoint = Vector3.zero;

        while(!nextWayPointFound)
        {
            Vector3? waypoint = null;

            if (waypoints.Count > 0)
            {
                waypoint = waypoints.First();
            }
            
            if(waypoint.HasValue && Vector3.Distance(this.transform.position, waypoint.Value) < _WayPointReachedInRange)
            {
                waypoints.Remove(waypoint.Value);
                waypoint = waypoints.FirstOrDefault();
            }

            if (!waypoint.HasValue)
            {
                status = Status.Wait;
                return;
            }

            else
            {
                nextWaypoint = waypoint.Value;
                nextWayPointFound = true;
            }
        }

        pathVisualizer.SetPaths(waypoints);
        playerController.Move((nextWaypoint - this.transform.position).normalized);
    }
}
