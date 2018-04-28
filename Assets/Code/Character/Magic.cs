using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour, IMagic
{
    [SerializeField]
    private float _Cooldown = 2f;
    [SerializeField]
    private float _CastDiameter = 3f;
    [SerializeField]
    private CapsuleCollider _MagicRangeCollider;
    [SerializeField]
    private float _KillDelay = 0.5f; 

    private ParticleSystem particle;
    private float cooldownLeft = 0f;
    private bool readyToCast = true;
    private List<GameObject> playersInRange;

    void Start ()
    {
        foreach(ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
        {
            if(ps.gameObject.tag == Tags.Magic)
            {
                particle = ps;
                break;
            }
        }

        _MagicRangeCollider.radius = _CastDiameter / 2;
        _MagicRangeCollider.isTrigger = true;
        playersInRange = new List<GameObject>();
	}
	
    void Update()
    {
        if(cooldownLeft >= 0 && !readyToCast)
        {
            cooldownLeft -= Time.deltaTime;

            if (cooldownLeft <= 0)
            {
                cooldownLeft = _Cooldown;
                readyToCast = true;
            }
        }

        Kill();
    }


    private float killDelayLeft;
    private bool killed = true;
    public void Cast()
    {
        if (readyToCast)
        {
            particle.Play();
            readyToCast = false;
            killDelayLeft = _KillDelay;
            killed = false;
        }

        
    }

    void Kill()
    {
        if (!killed)
        {
            killDelayLeft -= Time.deltaTime;

            if (killDelayLeft <= 0)
            {
                killed = true;

                foreach (GameObject go in playersInRange)
                {
                    go.GetComponent<IPlayer>().Die();
                }
            }
        }
    }


    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == Tags.Player)
        {
            playersInRange.Add(collider.gameObject);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if(collider.gameObject.tag == Tags.Player)
        {
            playersInRange.Remove(collider.gameObject);
        }

    }
}
