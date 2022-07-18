using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockController : MonoBehaviour
{
    [SerializeField] private float timeToFullRotationForDay;

    [SerializeField] private float timeToFullRotationForHour;

    [SerializeField] private GameObject dayClockHand;
    [SerializeField] private GameObject hourClockHand;

    private float day;

    private float hourInWorkDay = 8;

    private float rotationDegreesPerDay = 360f;


    public void StartClock(SOEventArgs e)
    {
        SOUpdateUITimerEventArgs arg = (SOUpdateUITimerEventArgs)e;

        timeToFullRotationForDay = arg.currentTimerSec;

        timeToFullRotationForHour = timeToFullRotationForDay / hourInWorkDay;

        StartCoroutine(DayClockTick());
        StartCoroutine(HourClockTick());
    }

    private IEnumerator DayClockTick()
    {
        float currentTime = 0;

        while (currentTime <= timeToFullRotationForDay)
        {
            day = Time.deltaTime / timeToFullRotationForDay;
            float dayNormalized = day % 1f;


            dayClockHand.transform.Rotate(Vector3.back * rotationDegreesPerDay * dayNormalized);

            yield return new WaitForEndOfFrame();
            currentTime += Time.deltaTime;
        }
    }

    private IEnumerator HourClockTick()
    {
        float currentHour = 0;
        
        while (currentHour < hourInWorkDay)
        {
            float currentTime = 0;
            while (currentTime <= timeToFullRotationForHour)
            {
                day = Time.deltaTime / timeToFullRotationForHour;
                float dayNormalized = day % 1f;


                hourClockHand.transform.Rotate(Vector3.back * rotationDegreesPerDay * dayNormalized);

                yield return new WaitForEndOfFrame();
                currentTime += Time.deltaTime;
            }
            currentHour++;
        }
    }
}
