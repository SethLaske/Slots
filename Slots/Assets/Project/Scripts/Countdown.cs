using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown
{
    private Action onStart;
    private Action<float, float> onUpdate;
    private Action onComplete;

    private float initialTime;
    private float timeRemaining;

    private bool isActive;

    public Countdown(float argTime, bool argStartOnCreation = true, Action argOnStart = null, Action<float, float> argOnUpdate = null, Action argOnComplete = null)
    {
        initialTime = argTime;
        timeRemaining = argTime;
        onStart = argOnStart;
        onUpdate = argOnUpdate;
        onComplete = argOnComplete;
        isActive = false;   
        
        CountdownManager.instance.AddNewCountdown(this);

        if (argStartOnCreation) 
        { 
            StartTimer();
        }
    }

    public void StartTimer() 
    { 
        isActive = true;
        onStart?.Invoke();
    }

    public void PauseTimer() 
    {
        isActive = false;
    }

    public void ResumeTimer() 
    {
        isActive = true;
    }

    public void EndTimer(bool argInvokeOnComplete) {
        isActive = false;

        if (argInvokeOnComplete) { 
            onComplete?.Invoke();
        }

        CountdownManager.instance.AddEndedCountdown(this);
    }

    public void UpdateTimer(float delta)
    {
        if (isActive == false) 
        {
            return;
        }

        timeRemaining -= delta;
        if (timeRemaining > 0)
        {
            onUpdate?.Invoke(initialTime - timeRemaining, initialTime);
        }
        else 
        {
            EndTimer(true);
        }
    }
}