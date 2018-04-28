using InControl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(IPathfindingMesh))]
[RequireComponent(typeof(ICircleSpawner))]
public class Spawner : MonoBehaviour, ISpawner
{
    [SerializeField, Range(1f, 10f)]
    private float _WaitBevorSpawn = 3f;
    [SerializeField]
    private GameObject _AIPrefab;
    [SerializeField]
    private int _AICount;
    [SerializeField]
    private GameObject _PlayerPrefab;

    private IPathfindingMesh pathFindingMesh;
    private IHumanColor humanColor;
    private ICircleSpawner circleSpawner;
    private IGameLogic gameLogic;

	void Start ()
    {
        pathFindingMesh = GetComponent<IPathfindingMesh>();
        humanColor = GetComponent<IHumanColor>();
        circleSpawner = GetComponent<ICircleSpawner>();
        gameLogic = GameObject.Find("ScriptHolder").GetComponent<IGameLogic>();
	}

    public void SpawnAI(ICollection<InputDevice> playerInputDevices)
    {
        for (int i = 0; i < _AICount; i++)
        {
            GameObject go = Instantiate(_AIPrefab);
            go.transform.position = pathFindingMesh.GetRandomPoint();

            MeshRenderer mr = go.GetComponentInChildren<MeshRenderer>();
            mr.material.color = humanColor.GetColor();
        }
    }
}
