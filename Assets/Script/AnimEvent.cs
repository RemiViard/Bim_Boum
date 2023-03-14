using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvent : MonoBehaviour
{
    [SerializeField] PlayerInteract playerInteract;
    public void PunchEvent()
    {
        playerInteract.Punch();
    }
    public void UseEvent()
    {
        playerInteract.Use();
    }
}
