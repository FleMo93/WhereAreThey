using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IPlayerController))]
public class PlayerInput : MonoBehaviour
{
    IPlayerController playerController;

	void Start ()
    {
        playerController = GetComponent<IPlayerController>();
	}
	
	void Update ()
    {
        float z = Input.GetAxisRaw("Vertical");
        float x = Input.GetAxisRaw("Horizontal");

        playerController.Move(new Vector3(x, 0, z).normalized);
	}
}
