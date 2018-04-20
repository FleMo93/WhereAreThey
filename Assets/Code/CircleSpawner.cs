using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSpawner : MonoBehaviour, ICircleSpawner
{
    private static ICircleSpawner circleSpawner;
    public static ICircleSpawner Get
    {
        get
        {
            if(circleSpawner == null)
            {
                circleSpawner = FindObjectOfType<CircleSpawner>();
            }

            return circleSpawner;
        }
    }


    [SerializeField]
    private float _MinSpawnTime = 20f;
    [SerializeField]
    private float _MaxSpawnTime = 30f;

    List<ParticleSystem> circles;

    private float timeToSpawn;

    void Start ()
    {
        circles = new List<ParticleSystem>();
        timeToSpawn = UnityEngine.Random.Range(_MinSpawnTime, _MaxSpawnTime);
	}
	
    

	void Update ()
    {
        if(GameLogic.Get.GetState() != GameLogicEnum.GameStates.Fight)
        {
            return;
        }

        timeToSpawn -= Time.deltaTime;

        if(timeToSpawn <= 0)
        {
            timeToSpawn = UnityEngine.Random.Range(_MinSpawnTime, _MaxSpawnTime);

            foreach (ParticleSystem ps in circles)
            {
                ps.Play();
            }
        }
	}

    public void RegisterCirle(ParticleSystem particleSystem)
    {
        if (!circles.Contains(particleSystem))
        {
            circles.Add(particleSystem);
        }
    }
}
