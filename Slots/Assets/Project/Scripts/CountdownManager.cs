using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownManager : MonoBehaviour
{
    public static CountdownManager instance;

    private List<Countdown> allCountdowns = new List<Countdown>();

    private List<Countdown> newCountdowns = new List<Countdown>();
    private List<Countdown> endedCountdowns = new List<Countdown>();
    void Awake()
    {
        if (instance == null)
        {

            instance = this;
        }
        else if (instance != this)
        {

            Destroy(gameObject);
        }
    }

    public void DoUpdate(float delta) 
    {
        if (newCountdowns.Count > 0)
        {
            foreach (Countdown newCountdown in newCountdowns)
            {
                allCountdowns.Add(newCountdown);
            }
            newCountdowns.Clear();
        }

        if (endedCountdowns.Count > 0) 
        {
            foreach (Countdown endedCountdown in endedCountdowns)
            {
                if (allCountdowns.Contains(endedCountdown))
                {
                    allCountdowns.Remove(endedCountdown);
                }
            }
            endedCountdowns.Clear();
        }

        foreach (Countdown countdown in allCountdowns)
        {
            countdown.UpdateTimer(delta);
        }
    }

    //New and ended countdowns are handled seperately to avoid errors with foreach loops
    public void AddNewCountdown(Countdown argCountdown) 
    { 
        newCountdowns.Add(argCountdown);
    }

    public void AddEndedCountdown(Countdown argEndedCountdown)
    {
        endedCountdowns.Add(argEndedCountdown);
    }


}