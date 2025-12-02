using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tween
{
    private Transform transform;
    private Vector3 initialPos;
    private Vector3 finalPos;
    
    private Action onStart;
    private Action<float, float> onUpdate;
    private Action onComplete;

    private float initialTime;
    private float timeRemaining;

    private bool isActive;

    public Tween(float argTime, Transform argTransform, Vector3 argInitialPos, Vector3 argFinalPos, bool argStartOnCreation = true, Action argOnStart = null, Action<float, float> argOnUpdate = null, Action argOnComplete = null)
    {
        transform = argTransform;
        initialPos = argInitialPos;
        finalPos = argFinalPos;
        
        initialTime = argTime;
        timeRemaining = argTime;
        onStart = argOnStart;
        onUpdate = argOnUpdate;
        onComplete = argOnComplete;
        isActive = false;   
        
        TweenManager.instance.AddNewTween(this);

        if (argStartOnCreation) 
        { 
            StartTimer();
        }
    }

    public void StartTimer()
    {
        transform.position = initialPos;
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

        if (argInvokeOnComplete)
        {
            transform.position = finalPos;
            onComplete?.Invoke();
        }

        TweenManager.instance.AddEndedTween(this);
    }

    public void UpdateTweener(float delta)
    {
        if (isActive == false) 
        {
            return;
        }

        timeRemaining -= delta;
        if (timeRemaining > 0)
        {
            onUpdate?.Invoke(initialTime - timeRemaining, initialTime);

            float progress = initialTime - timeRemaining / initialTime;

            transform.position = Vector3.Lerp(initialPos, finalPos, progress);
        }
        else 
        {
            EndTimer(true);
        }
    }
}