using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarElement
{
    public MeshBox MeshBox { get; set; }
    public float PredictedCosts { get; set; }
    public AStarElement LastElement { get; set; }	
}
