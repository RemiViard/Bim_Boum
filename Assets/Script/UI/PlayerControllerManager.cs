using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerControllerManager : MonoBehaviour
{
    [SerializeField] List<InputControlScheme> controllerList;
    [SerializeField] List<PlayerConfig> playerConfigs;
    string[] schemeName;
    InputDevice[] playerDevices;
    List<TMPro.TMP_Dropdown.OptionData> optionDatas;
    public int NbPlayer;
    [SerializeField] PlayerControls playerControl;
    [SerializeField] GameObject playerPrefab;

    private void Awake()
    {
        playerControl = new PlayerControls();
        optionDatas = new List<TMPro.TMP_Dropdown.OptionData>();

    }
    void Start()
    {

        optionDatas.Add(new TMPro.TMP_Dropdown.OptionData(playerControl.Keyboard1Scheme.name));
        optionDatas.Add(new TMPro.TMP_Dropdown.OptionData(playerControl.Keyboard2Scheme.name));
        foreach (var device in InputSystem.devices)
        {
            if (device is Gamepad gamepad)
            {
                optionDatas.Add(new TMPro.TMP_Dropdown.OptionData(gamepad.name));
                controllerList.Add(playerControl.GamePadScheme);
            }
        }
        foreach (var config in playerConfigs)
        {
            config.Config(optionDatas);
        }
        NbPlayer = 2;
        ShowUnshowConfig();
    }
    public void AddPlayer()
    {
        if (NbPlayer < 4)
        {
            NbPlayer++;
            ShowUnshowConfig();
        }
    }
    public void RemovePlayer()
    {
        if (NbPlayer > 2)
        {
            NbPlayer--;
            ShowUnshowConfig();
        }
    }
    void ShowUnshowConfig()
    {

        for (int i = 0; i < 4; i++)
        {
            if (i < NbPlayer)
            {
                playerConfigs[i].gameObject.SetActive(true);
            }
            else
            {
                playerConfigs[i].gameObject.SetActive(false);
            }
        }
    }
    public void StartGame()
    {
        DontDestroyOnLoad(gameObject);
        schemeName = new string[NbPlayer];
        playerDevices = new InputDevice[NbPlayer];
        for (int i = 0; i < NbPlayer; i++)
        {
            if(playerConfigs[i].getOption().Contains("Keyboard"))
            {
                schemeName[i] = playerControl.controlSchemes[playerConfigs[i].getDropdownIndex()].name;
            }
            else
            {
                schemeName[i] = "GamePad";
            }
            Debug.Log(schemeName[i]);
            if (playerConfigs[i].getOption().Contains("Keyboard"))
            {
                playerDevices[i] = InputSystem.GetDevice("Keyboard");
            }
            else
            {
                playerDevices[i] = InputSystem.GetDevice(playerConfigs[i].getOption());
            }
            
        }
        
        SceneManager.LoadScene(1);
        SceneManager.sceneLoaded += InstanciatePlayer;

    }
    void InstanciatePlayer(Scene scene, LoadSceneMode loadSceneMode)
    {
        for (int i = 0; i < NbPlayer; i++)
        {
            GameObject player = Instantiate(playerPrefab, playerConfigs[i].pos, Quaternion.identity);
            player.GetComponent<PlayerInput>().SwitchCurrentControlScheme(schemeName[i], playerDevices[i]);
            player.GetComponent<PlayerInteract>().Id = i;
            player.GetComponent<PlayerInteract>().LoadMesh();
        }
        SceneManager.sceneLoaded -= InstanciatePlayer;
    }
    public void ChangeSprite(GameObject playerConfig, int value)
    {
        if (optionDatas[value].text.Contains("Keyboard"))
        {

        }
        else
        {

        }
    }
}
