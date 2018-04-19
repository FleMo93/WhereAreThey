using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IPathfindingMesh))]
[RequireComponent(typeof(ICircleSpawner))]
public class Spawner : MonoBehaviour
{
    [SerializeField, Range(1f, 10f)]
    private float _WaitBevorSpawn = 3f;
    [SerializeField]
    private GameObject _AIPrefab;
    [SerializeField]
    private int _AICount;
    [SerializeField]
    private GameObject _PlayerPrefab;
    [SerializeField]
    private int _PlayerCount;

    private IPathfindingMesh pathFindingMesh;
    private IHumanColor humanColor;
    private ICircleSpawner circleSpawner;

	void Start ()
    {
        pathFindingMesh = GetComponent<IPathfindingMesh>();
        humanColor = GetComponent<IHumanColor>();
        circleSpawner = GetComponent<ICircleSpawner>();
        
        StartCoroutine(LateStart(_WaitBevorSpawn));
	}

    IEnumerator LateStart(float wait)
    {
        yield return new WaitForSeconds(wait);

        for (int i = 0; i < _AICount; i++)
        {
            GameObject go = Instantiate(_AIPrefab);
            go.transform.position = pathFindingMesh.GetRandomPoint();

            MeshRenderer mr = go.GetComponentInChildren<MeshRenderer>();
            mr.material.color = humanColor.GetColor();
        }

        for(int i = 0; i < _PlayerCount; i++)
        {
            GameObject go = Instantiate(_PlayerPrefab);
            go.transform.position = pathFindingMesh.GetRandomPoint();
            MeshRenderer mr = go.GetComponentInChildren<MeshRenderer>();
            mr.material.color = humanColor.GetColor();

        }

        foreach(GameObject go in GameObject.FindGameObjectsWithTag(Tags.Circle))
        {
            circleSpawner.RegisterCirle(go.GetComponent<ParticleSystem>());
        }
    }
}
