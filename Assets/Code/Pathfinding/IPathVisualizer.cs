using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPathVisualizer
{
    void SetPaths(List<Vector3> paths);
    void ShowPath(bool show);
}
