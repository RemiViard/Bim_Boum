using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WeatherAndTime;
public class GameManager : MonoBehaviour
{
    #region Fields
    
    [SerializeField]  TMPro.TMP_Text [] scoretext = new TMPro.TMP_Text[4];
    [SerializeField] Sprite [] playerSprite = new Sprite[4];
    [SerializeField] GameObject ScorePanel;
    [SerializeField] GameObject VictoryPanel;
    [SerializeField] GameObject EgalityPanel;
    [SerializeField] GameObject EndGamePanel;
    [SerializeField] Image WinnerRoundSprite;
    [SerializeField] Image WinnerGameSprite;
    #endregion

    #region Methods
    public void EndRound(int PlayerId)
    {
        ScorePanel.SetActive(true);
        if (PlayerId != -1)
        {
            ScoreManager.Instance.score[PlayerId]++;
            if (ScoreManager.Instance.score[PlayerId] == ScoreManager.Instance.scoreToWin)
            {
                WinnerGameSprite.sprite = playerSprite[PlayerId];
                EndGamePanel.SetActive(true);
                TimeManager.CreateNewTimer(ResetGame, 3f, this, true);
            }
            else
            {
                WinnerRoundSprite.sprite = playerSprite[PlayerId];
                
                VictoryPanel.SetActive(true);
                TimeManager.CreateNewTimer(ResetRound, 3f, this, true);
            }
        }
        else
        {
            EgalityPanel.SetActive(true);
            TimeManager.CreateNewTimer(ResetRound, 3f, this, true);
        }
        UpdateScore();
    }
    void ResetRound()
    {
        SceneManager.LoadScene(1);
    }
    void ResetGame()
    {
        SceneManager.LoadScene(0);
    }
    public void UpdateScore()
    {
        for (int i = 0; i < 4; i++)
        {
            if(i < ScoreManager.Instance.nbPlayer)
            {
                scoretext[i].text = ScoreManager.Instance.score[i].ToString();
            }
            else
            {
                scoretext[i].gameObject.SetActive(false);
            }
        }
    }
    
    #endregion
}
