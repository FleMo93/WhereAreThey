using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFlickerLights
{
    void SetState(bool state, bool instant = false);
}
