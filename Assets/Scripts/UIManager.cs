using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Volume globalVolume;
    private DepthOfField blur;

    [SerializeField] private GameObject startScreen;

    [SerializeField] private TextMeshProUGUI lifeCounterText;

    [SerializeField] private TextMeshProUGUI coinsCounterText;

    [SerializeField] private bool isGameRunning;
    [SerializeField] private bool isGameOver;

    [SerializeField] private SOEvent restartGameEvent;

    [Header("Round over screen")]

    [SerializeField] private GameObject roundOverScreen;
    [SerializeField] private TextMeshProUGUI dayCounter;
    [SerializeField] private TextMeshProUGUI coinsEarned;
    [SerializeField] private TextMeshProUGUI coinsText;

    [SerializeField] private Button nextDayBtn;
    [SerializeField] private Button returnToMenuBtn;

    [SerializeField] private TextMeshProUGUI taxText;
    [SerializeField] private TextMeshProUGUI taxCounter;

    [SerializeField] private TextMeshProUGUI gameOverText;

    [SerializeField] private TextMeshProUGUI ratingText;
    [SerializeField] private TextMeshProUGUI ratingTempTextValue; //Заменить на картинки звёзд

    [Space(10)]

    [Header("Start timer screen")]
    [SerializeField] private GameObject startTimerScreen;
    [SerializeField] private TextMeshProUGUI startTimerText;
    [SerializeField] private TextMeshProUGUI dayCounterStart;
    [SerializeField] private TextMeshProUGUI weekCounterStart;


    void Start()
    {
        DOTween.Init();

        DepthOfField temp;
        if (globalVolume.sharedProfile.TryGet<DepthOfField>(out temp))
        {
            blur = temp;
        }

        blur.active = true;
        isGameOver = false;
    }

    public void OnLifeCounterChange(SOEventArgs e)
    {
        SOUpdateUILifeCounterEventArgs arg = (SOUpdateUILifeCounterEventArgs) e;
        lifeCounterText.text = $"Life count: {arg.countCurrentOfHealth}";
    }

    public void OnCoinsCounterChange(SOEventArgs e)
    {
        SOUpdateUICoinsCounterEventArgs arg = (SOUpdateUICoinsCounterEventArgs) e;
        coinsCounterText.text = $"Coins: {arg.countOfCurrentCoins}";
    }

    public void OnStartTimerChange(SOEventArgs e)
    {
        startTimerScreen.SetActive(true);
        SOUpdateUITimerEventArgs arg = (SOUpdateUITimerEventArgs) e;
        startTimerText.text = $"{arg.currentTimerSec}";

        dayCounterStart.text = $"Day {arg.currentWeekDay + 1}";
        weekCounterStart.text = $"Week {arg.currentWeek}";
    }

    public void OnStartGame()
    {
        DisableStartScreen();
        startTimerScreen.SetActive(false);
        isGameRunning = true;

        //
        gameOverText.enabled = false;
        isGameOver = false;
    }

    public void OnRestart()
    {
        DisableStartScreen();
        isGameRunning = true;
        isGameOver = false;
    }

    public void OnRoundOver(SOEventArgs e)
    {
        blur.active = true;
        nextDayBtn.interactable = false;
        returnToMenuBtn.interactable = false;
        taxText.enabled = false;
        taxCounter.enabled = false;
        ratingText.enabled = false;
        ratingTempTextValue.enabled = false;

        SORoundOverEventArgs arg = (SORoundOverEventArgs) e;

        dayCounter.text = $"DAY {arg.currentDay} IS OVER";
        
        isGameRunning = false;
        blur.active = true;
        //Animation goes here
        roundOverScreen.SetActive(true);

        StartCoroutine(CoinsAnimation(arg.coinsEarned));

        if(arg.isEndOfWeek)
        {
            //Показывать счёт на оплату за неделю
            taxText.enabled = true;
            taxCounter.enabled = true;

            taxCounter.text = $"{arg.taxes}";
        }
    }

    //TODO: Подумать над тем, как можно сделать такое заполнение за определённое количество секунд в любом случае
    private IEnumerator CoinsAnimation(int coins)
    {
        int currentCoins = 0;
        if(coins >= 0)
        {
            while(currentCoins != coins)
            {
                coinsEarned.text = $"{currentCoins}";
                currentCoins++;
                yield return new WaitForEndOfFrame();
            }

            coinsText.text = "Coins earned:";
        }
        else
        {
            while (currentCoins != coins)
            {
                coinsEarned.text = $"{currentCoins}";
                currentCoins--;
                yield return new WaitForEndOfFrame();
            }

            coinsText.text = "Coins loss:";
        }

        coinsEarned.text = $"{coins}";
        if (!isGameOver)
        {
            nextDayBtn.interactable = true;

            ratingText.enabled = true;
            ratingTempTextValue.enabled = true;
        }
        returnToMenuBtn.interactable = true;


    }

    public void OnRatingUpdate(SOEventArgs e)
    {
        var arg = (SOUpdateUIRatingEventArgs)e;
        ratingTempTextValue.text = $"{arg.rating}";
    }

    public void OnGameOver()
    {
        //nextDayBtn.interactable = false;
        //Отобразить надпись о поражении
        nextDayBtn.interactable = false;
        gameOverText.enabled = true;
        isGameOver = true;
    }

    public void RestartGame()
    {
        restartGameEvent.Raise();
    }

    public void DisableStartScreen()
    {
        blur.active = false;
        roundOverScreen.SetActive(false);
        startScreen.SetActive(false);
    }

    public void EnableStartScreen()
    {
        blur.active = true;
        roundOverScreen.SetActive(false);
        startScreen.SetActive(true);
    }
}
