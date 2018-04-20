using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayer : MonoBehaviour, IPlayer
{
    [SerializeField]
    private GameObject _DeathEffect;
    [SerializeField]
    private float _TimeToDie = 0.1f;

    private bool isDead = false;
    public bool IsDead
    {
        private set
        {
            isDead = value;
        }

        get
        {
            return isDead;
        }
    }

    private float timeToDieLeft;

	void Start ()
    {
        _DeathEffect.SetActive(false);	
	}
	
	public void Die()
    {
        _DeathEffect.SetActive(true);
        _DeathEffect.transform.SetParent(null);
        _DeathEffect.transform.rotation = new Quaternion();

        timeToDieLeft = _TimeToDie;
        IsDead = true;
	}

    void Update()
    {
        if(IsDead)
        {
            timeToDieLeft -= Time.deltaTime;

            if(timeToDieLeft <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public bool IsAlive()
    {
        return !IsDead;
    }
}
