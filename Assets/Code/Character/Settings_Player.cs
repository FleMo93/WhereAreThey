using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Setting", menuName = "Scriptable Objects/Player Setting")]
public class Settings_Player : ScriptableObject
{
    [SerializeField]
    public float Acceleration = 100;

    [SerializeField]
    public float MaxMovementSpeed = 2;

    [SerializeField]
    public float RotationSpeed = 2;
}
