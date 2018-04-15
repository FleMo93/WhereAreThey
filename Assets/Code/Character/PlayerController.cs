using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IPlayerMotor))]
public class PlayerController : MonoBehaviour, IPlayerController
{
    IPlayerMotor playerMotor;
    IPathfindingMesh pathFindingMesh;

	void Start ()
    {
        playerMotor = GetComponent<IPlayerMotor>();
        pathFindingMesh = FindObjectOfType<PathfindingMesh>();
        foreach (Collider collider in GetComponents<Collider>())
        {
            pathFindingMesh.RegistrateMoveableObstacle(collider);
        }
	}
	

    public void Move(Vector3 direction)
    {
        playerMotor.Move(direction);
    }
}
