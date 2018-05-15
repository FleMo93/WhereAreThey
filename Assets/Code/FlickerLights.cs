﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerLights : MonoBehaviour, IFlickerLights
{
    [SerializeField]
    private Light[] _Lights;
    [SerializeField]
    private float _MinTimeToFlicker = 0.5f;
    [SerializeField]
    private float _MaxTimeToFlicker = 1f;
    [SerializeField]
    private float _MinTimeLightState = 0.01f;
    [SerializeField]
    private float _MaxTimeLightState = 0.05f;
    [SerializeField]
    private bool _FlickerInfinity = false;
    [SerializeField]
    private bool _Flicker = false;

    private float timeToSwitchState = 0f;
    private float timeToFinish = 0f;
    private bool stateToReach = false;
    private bool state = true;

    void Start ()
    {
		if(_Lights.Length > 0)
        {
            state = _Lights[0].enabled;
        }

        timeToFinish = Random.Range(_MinTimeToFlicker, _MaxTimeToFlicker);
        timeToSwitchState = Random.Range(_MinTimeLightState, _MaxTimeLightState);
    }
	
	void Update ()
    {
		if(!_Flicker)
        {
            return;
        }

        if(timeToFinish <= 0 && !_FlickerInfinity)
        {
            SetLights(stateToReach);
            _Flicker = false;
            return;
        }

        timeToSwitchState -= Time.deltaTime;
        timeToFinish -= Time.deltaTime;

        if(timeToSwitchState <= 0)
        {
            SetLights(!state);
            state = !state;
            timeToSwitchState = Random.Range(_MinTimeLightState, _MaxTimeLightState);
        }
	}

    void SetLights(bool on)
    {
        foreach(Light light in _Lights)
        {
            light.enabled = on;
        }
    }

    public void SetState(bool state, bool instant = false)
    {
        stateToReach = state;
        if (!instant)
        {
            timeToFinish = Random.Range(_MinTimeToFlicker, _MaxTimeToFlicker);
            timeToSwitchState = Random.Range(_MinTimeLightState, _MaxTimeLightState);
            _Flicker = true;
        }
        else
        {
            SetLights(state);
        }
    }
}
