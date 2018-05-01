using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IGameLogic))]
public class DoorExit : MonoBehaviour
{
    IGameLogic gameLogic;

    void Start()
    {
        gameLogic = GameObject.Find("ScriptHolder").GetComponent<IGameLogic>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == Tags.Player)
        {
            gameLogic.ExitGame();
        }
    }

}
