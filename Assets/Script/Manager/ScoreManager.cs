using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScoreManager : MonoBehaviour
{
    static ScoreManager instance;
    public static ScoreManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new GameObject("@ScoreManager").AddComponent<ScoreManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null || instance == this)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
            Destroy(gameObject);
    }
    public int nbPlayer;
    public int[] score = new int[4];
    public int scoreToWin = 5;
    public InputDevice[] inputDevice = new InputDevice[4];
    public string[] scheme = new string[4];
    public bool hasInput = false;
    public int inputGet = 0;
    public void SetInput(InputDevice device, string _scheme, int Id)
    {
        inputGet++;
        if (inputGet >= nbPlayer)
        {
            hasInput = true;
            inputDevice[Id] = device;
            scheme[Id] = _scheme;
        }
        Debug.Log(inputDevice[Id].name);
        Debug.Log(scheme[Id]);
    }
    public InputDevice GetInput(int Id)
    {
        return inputDevice[Id];
    }
    public string GetScheme(int Id)
    {
        return scheme[Id];
    }
   
}
