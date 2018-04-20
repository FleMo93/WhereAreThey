using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour {
	void Awake ()
    {
        CircleSpawner.Get.RegisterCirle(GetComponent<ParticleSystem>());
	}
	
}
