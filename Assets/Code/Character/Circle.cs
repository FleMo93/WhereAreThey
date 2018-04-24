using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour {
	void Start()
    {
        GameObject.Find("ScriptHolder").GetComponent<ICircleSpawner>().RegisterCirle(GetComponent<ParticleSystem>());
	}
	
}
