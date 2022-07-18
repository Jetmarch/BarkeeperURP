using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private bool isGameRunning;

    [SerializeField] private int lifeCount;

    [SerializeField] private int maxLifeCount;

    [SerializeField] private int currentCoinsCount;
    [SerializeField] private int previousCoinsCount;

    [SerializeField] private int startTimerSecToPrepare;

    [SerializeField] private int roundTimeInSec;

    [SerializeField] private List<GameDifficultyPreset> weekDays;

    [SerializeField] private int currentWeekDay;
    [SerializeField] private int currentWeek;


    [Space(10)]
    [Header("Events")]

    [SerializeField] private SOEvent roundStartEvent;
    [SerializeField] private SOEvent roundOverEvent;
    [SerializeField] private SOEvent gameOverEvent;
    [SerializeField] private SOEvent updateUILifeCounterEvent;
    [SerializeField] private SOEvent updateUICoinsCounterEvent;
    [SerializeField] private SOEvent disableStartScreenEvent;
    [SerializeField] private SOEvent updateUIStartTimerEvent;

    [SerializeField] private SOEvent updateUIRoundTimerEvent;

    [Header("Taxes")]
    [SerializeField] private float startTax;
    [SerializeField] private float currentTax;

    public void StartGame()
    {
        ResetAll();
        StartNextWeekDay();
    }

    public void StartNextWeekDay()
    {
        previousCoinsCount = currentCoinsCount;
        StartDay(weekDays[currentWeekDay]);
    }

    public void StartDay(GameDifficultyPreset preset)
    {
        lifeCount = maxLifeCount;
        updateUILifeCounterEvent.Raise(new SOUpdateUILifeCounterEventArgs(lifeCount));
        updateUICoinsCounterEvent.Raise(new SOUpdateUICoinsCounterEventArgs(currentCoinsCount));
        disableStartScreenEvent.Raise();

        StartCoroutine(StartTimerUpdate(preset));
    }

    private IEnumerator StartTimerUpdate(GameDifficultyPreset preset)
    {
        int secToPrepare = startTimerSecToPrepare;

        while (secToPrepare > 0)
        {
            updateUIStartTimerEvent.Raise(new SOUpdateUITimerEventArgs(secToPrepare) { currentWeek = this.currentWeek, currentWeekDay = this.currentWeekDay });
            yield return new WaitForSeconds(1);
            secToPrepare--;
        }

        roundStartEvent.Raise(new SOStartGameEventArgs(preset));
        isGameRunning = true;

        StartCoroutine(RoundTimerUpdate());
    }

    private IEnumerator RoundTimerUpdate()
    {
        updateUIRoundTimerEvent.Raise(new SOUpdateUITimerEventArgs(roundTimeInSec));
        int currentTime = roundTimeInSec;
        while (currentTime > 0)
        { 
            yield return new WaitForSeconds(1);
            currentTime--;
        }

        RoundOver();
    }

    public void RoundOver()
    {
        isGameRunning = false;

        int taxes = 0;
        bool isEndOfWeek = false;

        currentWeekDay++;
        if (currentWeekDay >= weekDays.Count)
        {
            currentWeek++;
            currentWeekDay = 0;
            taxes = (int)(currentTax * 1.7f); //Формула увеличения стоимости оплаты здесь
            currentTax *= 1.7f;
            isEndOfWeek = true;

            if (currentCoinsCount - taxes < 0)
            {
                GameOver();
            }
        }

        /*
         * Первый параметр текущий день. Если текущий день равен нулю, то это значит, что это крайний день недели. Иначе возвращается текущее значение.
         * Второй параметр вычисляемое количество заработанных за раунд монет
         * Третий параметр игровой налог
         * Четвёртый параметр флаг конца недели
         */
        roundOverEvent.Raise(new SORoundOverEventArgs(currentWeekDay == 0 ? weekDays.Count : currentWeekDay, (currentCoinsCount - previousCoinsCount) - taxes, taxes, isEndOfWeek));
        currentCoinsCount -= taxes;
    }

    private void GameOver()
    {
        ResetAll();
        gameOverEvent.Raise();
    }

    public void OnRestart()
    {
        ResetAll();
        isGameRunning = true;
        updateUILifeCounterEvent.Raise(new SOUpdateUILifeCounterEventArgs(lifeCount));
    }

    private void ResetAll()
    {
        lifeCount = maxLifeCount;
        currentWeek = 1;
        currentWeekDay = 0;
        currentCoinsCount = 0;
        previousCoinsCount = 0;
        currentTax = startTax;
    }

    public void OnItemDestroyByDZone()
    {
        if(!isGameRunning)
        {
            return;
        }

        lifeCount--;

        updateUILifeCounterEvent.Raise(new SOUpdateUILifeCounterEventArgs(lifeCount));

        //if (lifeCount <= 0)
        //{
        //    GameOver();
        //}
    }

    public void OnCoinsCountChange(SOEventArgs e)
    {
        var arg = (SOUpdateCoinsCounterEventArgs)e;
        currentCoinsCount += arg.coin;

        updateUICoinsCounterEvent.Raise(new SOUpdateUICoinsCounterEventArgs(currentCoinsCount));
    }
}


public class SOUpdateUILifeCounterEventArgs : SOEventArgs
{
    public int countCurrentOfHealth;

    public SOUpdateUILifeCounterEventArgs(int health)
    {
        countCurrentOfHealth = health;
    }
}

public class SOUpdateCoinsCounterEventArgs : SOEventArgs
{
    public int coin;

    public SOUpdateCoinsCounterEventArgs(int coin)
    {
        this.coin = coin;
    }
}

public class SOUpdateUICoinsCounterEventArgs : SOEventArgs
{
    public int countOfCurrentCoins;

    public SOUpdateUICoinsCounterEventArgs(int coins)
    {
        countOfCurrentCoins = coins;
    }
}

public class SOUpdateUITimerEventArgs : SOEventArgs
{
    public int currentTimerSec;
    public int currentWeekDay;
    public int currentWeek;

    public SOUpdateUITimerEventArgs(int timerSec)
    {
        currentTimerSec = timerSec;
    }
}

public class SOStartGameEventArgs : SOEventArgs
{
    public GameDifficultyPreset preset;

    public SOStartGameEventArgs(GameDifficultyPreset preset)
    {
        this.preset = preset;
    }
}

public class SORoundOverEventArgs : SOEventArgs
{
    public int currentDay;
    public int coinsEarned;
    public int taxes;
    public bool isEndOfWeek;

    public SORoundOverEventArgs(int currentDay, int coinsEarned, int taxes, bool isEndOfWeek)
    {
        this.currentDay = currentDay;
        this.coinsEarned = coinsEarned;
        this.isEndOfWeek = isEndOfWeek;
        this.taxes = taxes;
    }
}


