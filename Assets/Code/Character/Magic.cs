﻿using System;
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
    }


    public void Cast()
    {
        if (readyToCast)
        {
            particle.Play();
            readyToCast = false;

            foreach(GameObject go in playersInRange)
            {
                go.SetActive(false);
            }
        }
    }


    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == Tags.Player)
        {
            playersInRange.Add(collider.gameObject);
            Debug.Log(playersInRange.Count);
        }

    }

    void OnTriggerExit(Collider collider)
    {
        if(collider.gameObject.tag == Tags.Player)
        {
            playersInRange.Remove(collider.gameObject);
            Debug.Log(playersInRange.Count);
        }

    }
}