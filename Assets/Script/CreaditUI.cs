using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreaditUI : MonoBehaviour
{
    [SerializeField]
    GameObject menu;

    public void GoToMenu()
    {
        menu.SetActive(true);
        gameObject.SetActive(false);
    }
}
