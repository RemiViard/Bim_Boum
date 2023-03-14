using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
    [SerializeField]
    GameObject selectPlayer;

    [SerializeField]
    GameObject credits;

    public void StartButton()
    {
        selectPlayer.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Credit()
    {
        credits.SetActive(true);
        gameObject.SetActive(false);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
