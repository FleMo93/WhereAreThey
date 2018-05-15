using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathfindingShiftedMesh : MonoBehaviour, IPathfindingMesh
{
    [System.Serializable]
    private class DirectionDrawing
    {
        [SerializeField]
        public bool _ShowUpDirection = true;

        [SerializeField]
        public bool _ShowUpRightDirection = true;

        [SerializeField]
        public bool _ShowRightDirection = true;

        [SerializeField]
        public bool _ShowDownRightDirection = true;

        [SerializeField]
        public bool _ShowDownDirection = true;

        [SerializeField]
        public bool _ShowDownLeftDirection = true;

        [SerializeField]
        public bool _ShowLeftDirection = true;

        [SerializeField]
        public bool _ShowUpLeftDirection = true;
    }

    [SerializeField]
    private GameObject[] _Grounds = null;
    [SerializeField]
    private float _PlayerRadius = 0.5f;
    [SerializeField]
    private float _MeshYPosition = 1;
    [SerializeField]
    private float _RangeFromStatic = 0.6f;
    [SerializeField]
    private float _AddToRaycast = 0.1f;

    [Header("Visualizing")]
    [SerializeField]
    private bool _DrawMesh = false;
    [SerializeField]
    private bool _DrawState = true;
    [SerializeField]
    private bool _DrawPossibleMoveDirections = true;
    [SerializeField]
    private Color _FreeColor = Color.green;
    [SerializeField]
    private Color _BlockedColor = Color.red;
    [SerializeField]
    private Color _BlockedStaticColor = Color.blue;
    [SerializeField]
    private DirectionDrawing _DirectionDrawing = null;

    private List<MeshBox> mesh;
    private MeshBox[,] mesh2d;
    private List<Collider> moveableObstacles;
    private float diagonalLength;
    private float straigthLength;
    private float costDiagonal = 1f;
    private float costStraight = 1f;

    void Start()
    {
        LoadMesh();
    }

    void LoadMesh()
    {
        if (_Grounds.Length <= 0)
        {
            Debug.LogError("No grounds given");
            return;
        }

        float minX = float.MaxValue;
        float minZ = float.MaxValue;
        float maxX = float.MinValue;
        float maxZ = float.MinValue;

        #region get bounds
        foreach (GameObject ground in _Grounds)
        {
            if (ground == null)
            {
                Debug.LogError("Ground is null");
                return;
            }

            foreach (MeshRenderer mesh in ground.GetComponents<MeshRenderer>())
            {
                if (mesh.bounds.min.x < minX)
                {
                    minX = mesh.bounds.min.x;
                }

                if (mesh.bounds.min.z < minZ)
                {
                    minZ = mesh.bounds.min.z;
                }

                if (mesh.bounds.max.x > maxX)
                {
                    maxX = mesh.bounds.max.x;
                }

                if (mesh.bounds.max.z > maxZ)
                {
                    maxZ = mesh.bounds.max.z;
                }
            }
        }
        #endregion

        float playerDiameter = _PlayerRadius * 2;
        float posX = minX + playerDiameter;
        float posZ = minZ;
        int counterZ = 0;
        int counterX = 1;

        mesh = new List<MeshBox>();
        mesh2d = new MeshBox[
            Mathf.RoundToInt(((maxZ - minZ) / playerDiameter)),
            Mathf.RoundToInt(((maxX - minX) / playerDiameter))
            ];

        #region create points
        for (int z = 0; posZ < maxZ; z++)
        {
            for (int x = 0; posX < maxX; x++)
            {
                MeshBox box = new MeshBox(new Vector3(posX, this.transform.position.y + _MeshYPosition, posZ));
                box.MeshX = counterX;
                box.MeshZ = counterZ;
                mesh.Add(box);
                mesh2d[counterZ, counterX] = box;

                posX += playerDiameter * 2;
                counterX += 2;
            }
            counterZ++;
            posZ += playerDiameter;

            if (z % 2 == 0)
            {
                posX = minX;
                counterX = 0;
            }
            else
            {
                posX = minX + playerDiameter;
                counterX = 1;
            }
        }
        #endregion


        int mesh2dZ = mesh2d.GetLength(0);
        int mesh2dX = mesh2d.GetLength(1);


        #region setNeighbor
        for (int z = 0; z < mesh2dZ; z++)
        {
            for (int x = 0; x < mesh2dX; x++)
            {
                MeshBox box = mesh2d[z, x];

                if(box == null)
                {
                    continue;
                }

                if (z + 2 < mesh2dZ)
                {
                    box.UpNeighbor = mesh2d[z + 2, x];
                }

                if (z + 1 < mesh2dX && x + 1 < mesh2dX)
                {
                    box.UpRightNeighbor = mesh2d[z + 1, x + 1];
                }

                if (x + 2 < mesh2dX)
                {
                    box.RightNeighbor = mesh2d[z, x + 2];
                }

                if (x + 1 < mesh2dX && z - 1 >= 0)
                {
                    box.DownRightNeighbor = mesh2d[z - 1, x + 1];
                }

                if (z - 2 >= 0)
                {
                    box.DownNeighbor = mesh2d[z - 2, x];
                }

                if (z - 1 >= 0 && x - 1 >= 0)
                {
                    box.DownLeftNeighbor = mesh2d[z - 1, x - 1];
                }

                if (x - 2 >= 0)
                {
                    box.LeftNeighbor = mesh2d[z, x - 2];
                }

                if (x - 1 >= 0 && z + 1 < mesh2dZ)
                {
                    box.UpLeftNeighbor = mesh2d[z + 1, x - 1];
                }
            }
        }
        #endregion

        diagonalLength = Mathf.Sqrt(Mathf.Pow(playerDiameter, 2) + Mathf.Pow(playerDiameter, 2));
        straigthLength = playerDiameter * 2;

        costDiagonal = straigthLength / diagonalLength;
        costStraight = 1;
            
        #region check mesh vs level
        foreach (GameObject go in FindObjectsOfType<GameObject>())
        {
            if (go.layer != Layers.AntiPathMesh)
            {
                continue;
            }

            if(_Grounds.ToList().Contains(go))
            {
                continue;
            }

            foreach (Collider collider in go.GetComponents<Collider>())
            {
                foreach (MeshBox box in mesh)
                {
                    Vector3 closesPoint = collider.ClosestPoint(box.Position);

                    if (Vector3.Distance(closesPoint, box.Position) < _RangeFromStatic)
                    {
                        box.StaticFree = false;
                        continue;
                    }

                    if (IsInsideBounds(box.Position, (BoxCollider)collider))
                    {
                        box.StaticFree = false;
                        continue;
                    }
                }
            }
        }
        #endregion

        #region raycast
        foreach (MeshBox box in mesh)
        {
            if(!box.StaticFree)
            {
                continue;
            }

            Ray ray = new Ray(box.Position, Vector3.forward);
            if(!Physics.Raycast(ray, straigthLength + _AddToRaycast) && box.UpNeighbor != null && box.UpNeighbor.StaticFree)
            {
                box.CanMoveUp = true;
            }

            ray = new Ray(box.Position, Vector3.forward + Vector3.right);
            if(!Physics.Raycast(ray, diagonalLength + _AddToRaycast) && box.UpRightNeighbor != null && box.UpRightNeighbor.StaticFree)
            {
                box.CanMoveUpRight = true;
            }

            ray = new Ray(box.Position, Vector3.right);
            if(!Physics.Raycast(ray, straigthLength + _AddToRaycast) && box.RightNeighbor != null && box.RightNeighbor.StaticFree)
            {
                box.CanMoveRight = true;
            }

            ray = new Ray(box.Position, Vector3.back + Vector3.right);
            if(!Physics.Raycast(ray, diagonalLength + _AddToRaycast) && box.DownRightNeighbor != null && box.DownRightNeighbor.StaticFree)
            {
                box.CanMoveDownRight = true;
            }

            ray = new Ray(box.Position, Vector3.back);
            if(!Physics.Raycast(ray, straigthLength + _AddToRaycast) && box.DownNeighbor != null && box.DownNeighbor.StaticFree)
            {
                box.CanMoveDown = true;
            }

            ray = new Ray(box.Position, Vector3.back + Vector3.left);
            if(!Physics.Raycast(ray, diagonalLength + _AddToRaycast) && box.DownLeftNeighbor != null && box.DownLeftNeighbor.StaticFree)
            {
                box.CanMoveDownLeft = true;
            }

            ray = new Ray(box.Position, Vector3.left);
            if(!Physics.Raycast(ray, straigthLength + _AddToRaycast) && box.LeftNeighbor != null && box.LeftNeighbor.StaticFree)
            {
                box.CanMoveLeft = true;
            }

            ray = new Ray(box.Position, Vector3.forward + Vector3.left);
            if(!Physics.Raycast(ray, diagonalLength + _AddToRaycast) && box.UpLeftNeighbor != null && box.UpLeftNeighbor.StaticFree)
            {
                box.CanMoveUpLeft = true;
            }
        }
        #endregion
    }

    public void RegistrateMoveableObstacle(Collider obstacle)
    {
        if (moveableObstacles == null)
        {
            moveableObstacles = new List<Collider>();
        }

        moveableObstacles.Add(obstacle);
    }

    void Update()
    {
        CheckObstacles();
    }

    void CheckObstacles()
    {
        //if (moveableObstacles == null || moveableObstacles.Count == 0)
        //{
        //    return;
        //}

        //for (int z = 0; z < mesh.GetLength(0); z++)
        //{
        //    for (int x = 0; x < mesh.GetLength(1); x++)
        //    {
        //        MeshBox item = mesh[z, x];
        //        item.Free = true;
        //    }
        //}

        //for (int i = moveableObstacles.Count - 1; i >= 0; i--)
        //{
        //    Collider obstacle = moveableObstacles[i];

        //    if (obstacle == null || !obstacle.gameObject.activeSelf)
        //    {
        //        moveableObstacles.RemoveAt(i);
        //        continue;
        //    }

        //    for (int z = 0; z < mesh.GetLength(0); z++)
        //    {
        //        for (int x = 0; x < mesh.GetLength(1); x++)
        //        {
        //            MeshBox item = mesh[z, x];
        //            if (obstacle.bounds.Contains(item.Position))
        //            {
        //                item.Free = false;
        //            }
        //        }
        //    }
        //}
    }

    void OnDrawGizmos()
    {
        GizmoMesh();
    }

    void GizmoMesh()
    {
        if (_DrawMesh && mesh != null && mesh.Count > 0)
        {
            if (_DrawState)
            {
                foreach (MeshBox item in mesh)
                {
                    if (!item.StaticFree)
                    {
                        Gizmos.color = _BlockedStaticColor;
                    }
                    else if (!item.Free)
                    {
                        Gizmos.color = _BlockedColor;
                    }
                    else
                    {
                        Gizmos.color = _FreeColor;
                    }

                    Gizmos.DrawCube(item.Position, new Vector3(_PlayerRadius / 2, _PlayerRadius / 2, _PlayerRadius / 2));
                }
            }

            float diagonalOffset = _PlayerRadius * 2 * Mathf.Sin(45) / Mathf.Sin(90);

            if (_DrawPossibleMoveDirections)
            {
                Gizmos.color = Color.black;

                foreach(MeshBox item in mesh)
                {
                    if(!item.StaticFree)
                    {
                        continue;
                    }

                    if(item.CanMoveUp && _DirectionDrawing._ShowUpDirection)
                    {
                        Vector3 v3 = item.Position;
                        v3.z += straigthLength;

                        Gizmos.DrawLine(item.Position, v3);
                    }

                    if(item.CanMoveUpRight && _DirectionDrawing._ShowUpRightDirection)
                    {
                        Vector3 v3 = item.Position;
                        v3.z += diagonalOffset;
                        v3.x += diagonalOffset;
                        Gizmos.DrawLine(item.Position, v3);
                    }

                    if(item.CanMoveRight && _DirectionDrawing._ShowRightDirection)
                    {
                        Vector3 v3 = item.Position;
                        v3.x += straigthLength;

                        Gizmos.DrawLine(item.Position, v3);
                    }

                    if(item.CanMoveDownRight && _DirectionDrawing._ShowDownRightDirection)
                    {
                        Vector3 v3 = item.Position;
                        v3.x += diagonalOffset;
                        v3.z -= diagonalOffset;

                        Gizmos.DrawLine(item.Position, v3);
                    }

                    if(item.CanMoveDown && _DirectionDrawing._ShowDownDirection)
                    {
                        Vector3 v3 = item.Position;
                        v3.z -= straigthLength;
                        
                        Gizmos.DrawLine(item.Position, v3);
                    }

                    if(item.CanMoveDownLeft && _DirectionDrawing._ShowDownLeftDirection)
                    {
                        Vector3 v3 = item.Position;
                        v3.z -= diagonalOffset;
                        v3.x -= diagonalOffset;

                        Gizmos.DrawLine(item.Position, v3);
                    }

                    if(item.CanMoveLeft && _DirectionDrawing._ShowLeftDirection)
                    {
                        Vector3 v3 = item.Position;
                        v3.x -= straigthLength;

                        Gizmos.DrawLine(item.Position, v3);
                    }

                    if(item.CanMoveUpLeft && _DirectionDrawing._ShowUpLeftDirection)
                    {
                        Vector3 v3 = item.Position;
                        v3.z += diagonalOffset;
                        v3.x -= diagonalOffset;

                        Gizmos.DrawLine(item.Position, v3);
                    }
                }
            }
        }
    }

    static bool IsInsideBounds(Vector3 worldPos, BoxCollider bc)
    {
        Vector3 localPos = bc.transform.InverseTransformPoint(worldPos);
        Vector3 delta = localPos - bc.center + bc.size * 0.5f;
        return Vector3.Max(Vector3.zero, delta) == Vector3.Min(delta, bc.size);
    }

    public List<Vector3> GetPath(Vector3 start, Vector3 target)
    {
        List<Vector3> paths = new List<Vector3>();
        List<AStarElement> openList = new List<AStarElement>();
        List<AStarElement> closedList = new List<AStarElement>();
        MeshBox startBox = FindNextBox(start);
        MeshBox targetBox = FindNextBox(target);
        
        openList.Add(new AStarElement
        {
            MeshBox = startBox,
            PredictedCosts = Heuristic(startBox, targetBox)
        });

        AStarElement targetElement = null;
        while(targetElement == null)
        {
            AStarElement elem = openList.OrderBy(x => x.PredictedCosts).FirstOrDefault();
            closedList.Add(elem);
            openList.Remove(elem);

            foreach (MeshBox box in elem.MeshBox.Neighbors)
            {
                if(!box.StaticFree)
                {
                    continue;
                }

                if(closedList.Select(s => s.MeshBox).Contains(box) || openList.Select(s => s.MeshBox).Contains(box))
                {
                    continue;
                }

                if(box == targetBox)
                {
                    targetElement = new AStarElement
                    {
                        LastElement = elem,
                        MeshBox = box
                    };
                }

                openList.Add(new AStarElement
                {
                    MeshBox = box,
                    PredictedCosts = Heuristic(box, targetBox),
                    LastElement = elem
                });
            }
        }

        bool startBoxReached = false;
        AStarElement nexElement = targetElement;
        while(!startBoxReached)
        {
            paths.Add(nexElement.MeshBox.Position);

            if(nexElement.MeshBox == startBox)
            {
                startBoxReached = true;
                break;
            }

            nexElement = nexElement.LastElement;
        }


        List<Vector3> reversedList = new List<Vector3>();

        for(int i = paths.Count - 1; i >= 0; i--)
        {
            reversedList.Add(paths[i]);
        }

        return reversedList;
    }

    private MeshBox FindNextBox(Vector3 point)
    {
        MeshBox nearestStartBox = null;
        float minRange = float.MaxValue;

        foreach (MeshBox box in mesh)
        {
            if(!box.StaticFree)
            {
                continue;
            }

            float range = Vector3.Distance(box.Position, point);
            if (range < minRange)
            {
                minRange = range;
                nearestStartBox = box;
            }
        }

        return nearestStartBox;
    }

    private float Heuristic(MeshBox start, MeshBox target)
    {
        float dx = Mathf.Abs(start.MeshX - target.MeshX);
        float dz = Mathf.Abs(start.MeshZ - target.MeshZ);

        return costStraight * (dx + dz) + (costDiagonal - 2 * costStraight) * Mathf.Min(dx, dz);
    }

    public Vector3 GetRandomPoint()
    {
        Vector3 point = Vector3.zero;
        bool pointFound = false;

        while(!pointFound)
        {
            MeshBox box = mesh[UnityEngine.Random.Range(0, mesh.Count - 1)];

            if(box.StaticFree)
            {
                pointFound = true;
                point = box.Position;
            }
        }
        return point;
    }
}
