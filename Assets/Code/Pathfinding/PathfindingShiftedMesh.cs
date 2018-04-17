using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathfindingShiftedMesh : MonoBehaviour
{
    [System.Serializable]
    private class test
    {
        [SerializeField]
        private bool t1;

        [SerializeField]
        private bool t2;
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
    private test _Test;

    private List<MeshBox> mesh;
    private List<Collider> moveableObstacles;
    private float diagonalLength;
    private float straigthLength;

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

        float posX = minX;
        float posZ = minZ;
        float playerDiameter = _PlayerRadius * 2;

        mesh = new List<MeshBox>();

        #region create points
        for (int z = 0; posZ < maxZ; z++)
        {
            for (int x = 0; posX < maxX; x++)
            {
                mesh.Add(new MeshBox(new Vector3(posX, _MeshYPosition, posZ)));
                posX += playerDiameter * 2;
            }

            posZ += playerDiameter;

            if (z % 2 == 0)
            {
                posX = minX;
            }
            else
            {
                posX = minX + playerDiameter;
            }
        }
        #endregion

        diagonalLength = Mathf.Sqrt(Mathf.Pow(playerDiameter, 2) + Mathf.Pow(playerDiameter, 2));
        straigthLength = playerDiameter * 2;

        #region check mesh vs statics
        foreach (GameObject go in FindObjectsOfType<GameObject>())
        {
            if (!go.isStatic)
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
            if(!Physics.Raycast(ray, straigthLength + _AddToRaycast))
            {
                box.CanMoveUp = true;
            }

            ray = new Ray(box.Position, Vector3.forward + Vector3.right);
            if(!Physics.Raycast(ray, diagonalLength + _AddToRaycast))
            {
                box.CanMoveUpRight = true;
            }

            ray = new Ray(box.Position, Vector3.right);
            if(!Physics.Raycast(ray, straigthLength + _AddToRaycast))
            {
                box.CanMoveRight = true;
            }

            ray = new Ray(box.Position, Vector3.back + Vector3.right);
            if(!Physics.Raycast(ray, diagonalLength + _AddToRaycast))
            {
                box.CanMoveDownRight = true;
            }

            ray = new Ray(box.Position, Vector3.back);
            if(!Physics.Raycast(ray, straigthLength + _AddToRaycast))
            {
                box.CanMoveDown = true;
            }

            ray = new Ray(box.Position, Vector3.back + Vector3.left);
            if(!Physics.Raycast(ray, diagonalLength + _AddToRaycast))
            {
                box.CanMoveDownLeft = true;
            }

            ray = new Ray(box.Position, Vector3.left);
            if(!Physics.Raycast(ray, straigthLength + _AddToRaycast))
            {
                box.CanMoveLeft = true;
            }

            ray = new Ray(box.Position, Vector3.forward + Vector3.left);
            if(!Physics.Raycast(ray, diagonalLength + _AddToRaycast))
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

                    if(item.CanMoveUp)
                    {
                        Vector3 v3 = item.Position;
                        v3.z += straigthLength;

                        Gizmos.DrawLine(item.Position, v3);
                    }

                    if(item.CanMoveUpRight)
                    {
                        Vector3 v3 = item.Position;
                        v3.z += diagonalOffset;
                        v3.x += diagonalOffset;
                        Gizmos.DrawLine(item.Position, v3);
                    }

                    if(item.CanMoveDown)
                    {
                        Vector3 v3 = item.Position;
                        v3.z -= straigthLength;
                        
                        Gizmos.DrawLine(item.Position, v3);
                    }

                }
            }
        }
    }

    public List<Vector3> GetPath()
    {
        return null;
    }

    static bool IsInsideBounds(Vector3 worldPos, BoxCollider bc)
    {
        Vector3 localPos = bc.transform.InverseTransformPoint(worldPos);
        Vector3 delta = localPos - bc.center + bc.size * 0.5f;
        return Vector3.Max(Vector3.zero, delta) == Vector3.Min(delta, bc.size);
    }
}
