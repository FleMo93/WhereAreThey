using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSpawner : MonoBehaviour, ICircleSpawner
{
    [SerializeField]
    private float _MinSpawnTime = 20f;
    [SerializeField]
    private float _MaxSpawnTime = 30f;

    private List<ParticleSystem> circles;
    private IGameLogic gameLogic;

    private float timeToSpawn;

    void Start ()
    {
        circles = new List<ParticleSystem>();
        timeToSpawn = UnityEngine.Random.Range(_MinSpawnTime, _MaxSpawnTime);
        gameLogic = GameObject.Find("ScriptHolder").GetComponent<IGameLogic>();
	}
	
    

	void Update ()
    {
        if(gameLogic.GetState() != GameLogicEnum.GameStates.Fight)
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
