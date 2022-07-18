using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatingSystem : MonoBehaviour
{
    [SerializeField] private int maxRatingStars = 5;

    [SerializeField] private int countOfItems;
    [SerializeField] private int countOfRightWay;
    [SerializeField] private int countOfWrongWay;
    [SerializeField] private int countOfPass;

    [SerializeField] private SOEvent updateRating;

    [SerializeField] private bool isRoundRunning;

    public void OnRoundStart()
    {
        countOfItems = 0;
        countOfRightWay = 0;
        countOfWrongWay = 0;
        countOfPass = 0;

        isRoundRunning = true;
    }

    public void OnRoundEnd()
    {
        isRoundRunning = false;
        //Вычисляем рейтинг
        int rating = countOfRightWay / (countOfItems / maxRatingStars);

        //Вызываем событие и передаём количество звёзд обслуживания
        updateRating.Raise(new SOUpdateUIRatingEventArgs(rating));
    }

    public void OnItemAppear()
    {
        if(!isRoundRunning)
        {
            return;
        }

        countOfItems++;
    }

    public void OnItemGoesRightWay()
    {
        if (!isRoundRunning)
        {
            return;
        }

        countOfRightWay++;
    }

    public void OnItemGoesWrongWay()
    {
        if (!isRoundRunning)
        {
            return;
        }

        countOfWrongWay++;
    }

    public void OnItemDisappear()
    {
        if (!isRoundRunning)
        {
            return;
        }

        countOfPass++;
    }
}


public class SOUpdateUIRatingEventArgs : SOEventArgs
{
    public int rating;

    public SOUpdateUIRatingEventArgs(int rating)
    {
        this.rating = rating;
    }
}