using InControl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(IPathfindingMesh))]
public class Spawner : MonoBehaviour, ISpawner
{
    [SerializeField]
    private GameObject _AIPrefab;
    [SerializeField]
    private int _AICount;

    private IPathfindingMesh pathFindingMesh;
    private IHumanColor humanColor;
    private IGameLogic gameLogic;

	void Start ()
    {
        pathFindingMesh = GetComponent<IPathfindingMesh>();
        humanColor = GetComponent<IHumanColor>();
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

    public void AwakePlayer(GameObject player)
    {
        player.SetActive(true);
        MeshRenderer mr = player.GetComponentInChildren<MeshRenderer>();
        mr.material.color = humanColor.GetColor();
    }
}
