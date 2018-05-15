using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerInput
{
    void SetControlls(ICharacterControlls device);
    ICharacterControlls GetControlls();
    GameObject GetGameObject();
}
