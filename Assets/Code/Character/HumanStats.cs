using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanStats : MonoBehaviour
{
    [SerializeField]
    GameObject spawnOnDeath;

	void OnDisable()
    {
        GameObject go = Instantiate(spawnOnDeath);
        go.transform.position = this.transform.position;
    }
}
