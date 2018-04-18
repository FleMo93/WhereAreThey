using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshBox
{
    public bool Free { get; set; }
    private bool staticFree;
    public bool StaticFree
    {
        get
        {
            return staticFree;
        }

        set
        {
            staticFree = value;
            
            if(!value)
            {
                CanMoveUp = false;
                CanMoveUpRight = false;
                CanMoveRight = false;
                CanMoveDownRight = false;
                CanMoveDown = false;
                CanMoveDownLeft = false;
                CanMoveLeft = false;
                CanMoveUpLeft = false;
            }
        }
    }
    public Vector3 Position { get; private set; }

    public bool CanMoveUp { get; set; }
    public bool CanMoveUpRight { get; set; }
    public bool CanMoveRight { get; set; }
    public bool CanMoveDownRight { get; set; }
    public bool CanMoveDown { get; set; }
    public bool CanMoveDownLeft { get; set; }
    public bool CanMoveLeft { get; set; }
    public bool CanMoveUpLeft { get; set; }

    public MeshBox UpNeighbor { get; set; }
    public MeshBox UpRightNeighbor { get; set; }
    public MeshBox RightNeighbor { get; set; }
    public MeshBox DownRightNeighbor { get; set; }
    public MeshBox DownNeighbor { get; set; }
    public MeshBox DownLeftNeighbor { get; set; }
    public MeshBox LeftNeighbor { get; set; }
    public MeshBox UpLeftNeighbor { get; set; }

    public MeshBox(Vector3 position)
    {
        Position = position;
        Free = true;
        StaticFree = true;
        CanMoveUp = false;
        CanMoveUpRight = false;
        CanMoveRight = false;
        CanMoveDownRight = false;
        CanMoveDown = false;
        CanMoveDownLeft = false;
        CanMoveLeft = false;
        CanMoveUpLeft = false;
    }
}
