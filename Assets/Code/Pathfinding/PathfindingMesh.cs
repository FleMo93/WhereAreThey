using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingMesh : MonoBehaviour, IPathfindingMesh
{
    [SerializeField]
    private GameObject[] _Grounds = null;
    [SerializeField]
    private float _PlayerRadius = 0.5f;
    [SerializeField]
    private float _MeshYPosition = 1;

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
    
    private float boxLength = 0.5f;
    private MeshBox[,] mesh;
    private List<Collider> moveableObstacles;

    void Start ()
    {
        LoadMesh();
	}

    void LoadMesh()
    {
        if(_Grounds.Length <= 0)
        {
            Debug.LogError("No grounds given");
            return;
        }

        float minX = float.MaxValue;
        float minZ = float.MaxValue;
        float maxX = float.MinValue;
        float maxZ = float.MinValue;

        foreach (GameObject ground in _Grounds)
        {
            if(ground == null)
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

        float playerDiameter = _PlayerRadius * 2;
        boxLength = playerDiameter * Mathf.Sin(45) / Mathf.Sin(90);
        
        int meshXLength = Mathf.RoundToInt((maxX - minX) / boxLength);
        int meshZLength = Mathf.RoundToInt((maxZ - minZ) / boxLength);
        mesh = new MeshBox[meshZLength, meshXLength];

        float posX = minX + (_PlayerRadius / 2);
        float posZ = minZ + (_PlayerRadius / 2);

        for (int x = 0; x < meshXLength; x++)
        {
            for (int z = 0; z < meshZLength; z++)
            {
                mesh[z, x] = new MeshBox (new Vector3(posX, _MeshYPosition, posZ));
                posZ += boxLength;
            }

            posX += boxLength;
            posZ = minZ + (_PlayerRadius / 2);
        }




        foreach(GameObject go in FindObjectsOfType<GameObject>())
        {
            if(!go.isStatic)
            {
                continue;
            }

            foreach (Collider collider in go.GetComponents<Collider>())
            {
                for (int z = 0; z < mesh.GetLength(0); z++)
                {
                    for (int x = 0; x < mesh.GetLength(1); x++)
                    {
                        MeshBox item = mesh[z, x];

                        if(collider.bounds.Contains(item.Position))
                        {
                            item.StaticFree = false;
                            continue;
                        }

                        if (item.StaticFree)
                        {
                            SetMoveDirections(item, z, x);
                        }
                    }
                }
            }
        }
    }
	
    private void SetMoveDirections(MeshBox meshBox, int z, int x)
    {
        if(z + 1 < mesh.GetLength(0))
        {
            Ray ray = new Ray(meshBox.Position, Vector3.forward);

            if(!Physics.Raycast(ray, boxLength))
            {
                meshBox.CanMoveUp = true;
            }
        }


        if (z + 1 < mesh.GetLength(0) && x + 1 < mesh.GetLength(1))
        {
            Ray ray = new Ray(meshBox.Position, Vector3.forward + Vector3.right);

            if (!Physics.Raycast(ray, boxLength))
            {
                meshBox.CanMoveUpRight = true;
            }
        }

        if (x + 1 < mesh.GetLength(1))
        {
            Ray ray = new Ray(meshBox.Position, Vector3.right);

            if (!Physics.Raycast(ray, boxLength))
            {
                meshBox.CanMoveRight = true;
            }
        }

        if (x + 1 < mesh.GetLength(1) && z - 1 > 0)
        {
            Ray ray = new Ray(meshBox.Position, Vector3.right + Vector3.back);

            if (!Physics.Raycast(ray, boxLength))
            {
                meshBox.CanMoveDownRight = true;
            }
        }

        if (z - 1 > 0)
        {
            Ray ray = new Ray(meshBox.Position, Vector3.back);

            if (!Physics.Raycast(ray, boxLength))
            {
                meshBox.CanMoveDown = true;
            }
        }

        if (x - 1 > 0 && z - 1 > 0)
        {
            Ray ray = new Ray(meshBox.Position, Vector3.back + Vector3.left);

            if (!Physics.Raycast(ray, boxLength))
            {
                meshBox.CanMoveDownLeft= true;
            }
        }

        if (x - 1 > 0)
        {
            Ray ray = new Ray(meshBox.Position, Vector3.left);

            if (!Physics.Raycast(ray, boxLength))
            {
                meshBox.CanMoveLeft = true;
            }
        }

        if (x - 1 > 0 && z + 1 < mesh.GetLength(0))
        {
            Ray ray = new Ray(meshBox.Position, Vector3.forward + Vector3.left);

            if (!Physics.Raycast(ray, boxLength))
            {
                meshBox.CanMoveUpLeft = true;
            }
        }
    }

    public void RegistrateMoveableObstacle(Collider obstacle)
    {
        if(moveableObstacles == null)
        {
            moveableObstacles = new List<Collider>();
        }

        moveableObstacles.Add(obstacle);
    }

	void Update ()
    {
        CheckObstacles();
	}

    void CheckObstacles()
    {
        if(moveableObstacles == null || moveableObstacles.Count == 0)
        {
            return;
        }

        for (int z = 0; z < mesh.GetLength(0); z++)
        {
            for (int x = 0; x < mesh.GetLength(1); x++)
            {
                MeshBox item = mesh[z, x];
                item.Free = true;
            }
        }

        for(int i = moveableObstacles.Count -1; i >= 0; i--) 
        {
            Collider obstacle = moveableObstacles[i];

            if (obstacle == null || !obstacle.gameObject.activeSelf)
            {
                moveableObstacles.RemoveAt(i);
                continue;
            }

            for (int z = 0; z < mesh.GetLength(0); z++)
            {
                for (int x = 0; x < mesh.GetLength(1); x++)
                {
                    MeshBox item = mesh[z, x];
                    if (obstacle.bounds.Contains(item.Position))
                    {
                        item.Free = false;
                    }
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        if (_DrawMesh && mesh != null && mesh.Length != 0 &&
            (_DrawPossibleMoveDirections || _DrawState))
        {

            for(int z = 0; z < mesh.GetLength(0); z++)
            {
                for(int x = 0; x < mesh.GetLength(1); x++)
                {
                    MeshBox item = mesh[z, x];

                    if (_DrawState)
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

                        Gizmos.DrawCube(new Vector3(item.Position.x, _MeshYPosition, item.Position.z), 
                            new Vector3(_PlayerRadius / 2, _PlayerRadius / 2, _PlayerRadius / 2));
                    }

                    if (_DrawPossibleMoveDirections)
                    {
                        Gizmos.color = Color.black;

                        if (item.CanMoveUp)
                        {
                            Vector3 target = item.Position;
                            target.z += boxLength;
                            Gizmos.DrawLine(item.Position, target);
                        }

                        if(item.CanMoveUpRight)
                        {
                            Vector3 target = item.Position;
                            target.x += boxLength;
                            target.z += boxLength;
                            Gizmos.DrawLine(item.Position, target);
                        }

                        if(item.CanMoveRight)
                        {
                            Vector3 target = item.Position;
                            target.x += boxLength;
                            Gizmos.DrawLine(item.Position, target);
                        }
                        
                        if (item.CanMoveDownRight)
                        {
                            Vector3 target = item.Position;
                            target.x += boxLength;
                            target.z -= boxLength;
                            Gizmos.DrawLine(item.Position, target);
                        }

                        if (item.CanMoveDown)
                        {
                            Vector3 target = item.Position;
                            target.z -= boxLength;
                            Gizmos.DrawLine(item.Position, target);
                        }

                        if (item.CanMoveDownLeft)
                        {
                            Vector3 target = item.Position;
                            target.x -= boxLength;
                            target.z -= boxLength;
                            Gizmos.DrawLine(item.Position, target);
                        }

                        if (item.CanMoveLeft)
                        {
                            Vector3 target = item.Position;
                            target.x -= boxLength;
                            Gizmos.DrawLine(item.Position, target);
                        }

                        if (item.CanMoveUpLeft)
                        {
                            Vector3 target = item.Position;
                            target.x -= boxLength;
                            target.z += boxLength;
                            Gizmos.DrawLine(item.Position, target);
                        }
                    }
                }
            }
        }
    }

    public List<Vector3> GetPath()
    {
        return null;
    }
}
