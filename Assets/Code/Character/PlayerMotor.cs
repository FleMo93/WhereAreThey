using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour, IPlayerMotor
{
    Rigidbody myRigidbody;

    [SerializeField]
    private Settings_Player _PlayerSettings;

	void Start ()
    {
        myRigidbody = GetComponent<Rigidbody>();
	}
	
	public void Move(Vector3 direction)
    {
        myRigidbody.AddForce(direction * _PlayerSettings.Acceleration);
        
        if(myRigidbody.velocity.magnitude > _PlayerSettings.MaxMovementSpeed)
        {
            myRigidbody.velocity = Vector3.ClampMagnitude(myRigidbody.velocity, _PlayerSettings.MaxMovementSpeed);
        }

        RotateTowards(direction);
    }

    private void RotateTowards(Vector3 direction)
    {
        Quaternion oldRotation = new Quaternion(
            this.transform.rotation.x,
            this.transform.rotation.y,
            this.transform.rotation.z,
            this.transform.rotation.w);

        this.transform.LookAt(this.transform.position + direction);

        Quaternion targetRotation = new Quaternion(
            this.transform.rotation.x,
            this.transform.rotation.y,
            this.transform.rotation.z,
            this.transform.rotation.w);

        this.transform.rotation = Quaternion.RotateTowards(oldRotation, targetRotation, _PlayerSettings.RotationSpeed);
    }
}
