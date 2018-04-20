using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    [SerializeField]
    private float _DestroyAfter = 5;

    void Update()
    {
        _DestroyAfter -= Time.deltaTime;

        if(_DestroyAfter <= 0)
        {
            Destroy(gameObject);
        }
    }
}
