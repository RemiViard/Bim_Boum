using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractUI : MonoBehaviour
{
    [SerializeField]
    GameObject ui;

    private void Update()
    {
        transform.forward = Camera.main.transform.forward;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.TryGetComponent(out PlayerInteract player))
        {
            if (player.Interacts[0] == GetComponent<Interactible>())
            {
                ui.active = true;
            }
            else
                ui.active = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerInteract player))
        {
            ui.active = false;
        }
    }
}
