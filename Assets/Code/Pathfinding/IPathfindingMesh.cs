using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPathfindingMesh
{
    void RegistrateMoveableObstacle(Collider collider);
    List<Vector3> GetPath(Vector3 start, Vector3 target);
    Vector3 GetRandomPoint();
}
