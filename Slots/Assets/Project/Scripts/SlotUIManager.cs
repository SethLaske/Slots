using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SlotUIManager : MonoBehaviour
{
    public static SlotUIManager instance;

    [SerializeField] private TextMeshProUGUI playerBankText;
    [SerializeField] private TextMeshProUGUI playerBetText;
    [SerializeField] private TextMeshProUGUI payoutText;
    [SerializeField] private TextMeshProUGUI freeSpinsText;
    [SerializeField] private Button spinButton;
    [SerializeField] private Button increaseBetButton;
    [SerializeField] private Button decreaseBetButton;
    [SerializeField] private Button exitBetButton;
    
    [SerializeField] private GameObject freeSpinsContainer;
    
    [SerializeField] private SlotGameController gameController;
    
    private float lastPayoutValue = 0;
    private Countdown gradualPayoutCountdown = null;
    
    private float lastBankValue = 0;
    private Countdown gradualBankCountdown = null;
    
    private void Awake()
    {
        instance = this;
        
        spinButton.onClick.AddListener(OnSpinButtonPressed);
        increaseBetButton.onClick.AddListener(OnIncrementBetButtonPressed);
        decreaseBetButton.onClick.AddListener(OnDecrementBetButtonPressed);
        exitBetButton.onClick.AddListener(OnExitButtonPressed);
        
        SetPayoutText(0);
    }

    public void SetInputEnabled(bool argEnabled)
    {
        spinButton.interactable = argEnabled;
        increaseBetButton.interactable = argEnabled && SlotCurrencyController.instance.canIncrement && ProgressiveManager.instance.numberOfFreeSpinsRemaining == 0;
        decreaseBetButton.interactable = argEnabled && SlotCurrencyController.instance.canDecrement && ProgressiveManager.instance.numberOfFreeSpinsRemaining == 0;
    }

    public void OnSpinButtonPressed()
    {
        gameController.TryStartSpinning();
        
    }
    
    public void OnIncrementBetButtonPressed()
    {
        SlotCurrencyController.instance.TryIncreaseBet();
    }

    public void OnDecrementBetButtonPressed()
    {
        SlotCurrencyController.instance.TryDecreaseBet();
    }
    
    public void OnExitButtonPressed()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        return;
#endif
        Application.Quit();
    }

    public void SetBetAmountButtonEnabled(bool argCanIncrement, bool argCanDecrement)
    {
        increaseBetButton.interactable = argCanIncrement;
        decreaseBetButton.interactable = argCanDecrement;
    }

    public void SetPlayerBankText(float argValue, bool argCancelGradual = true)
    {
        if (argCancelGradual)
        {
            if (gradualPayoutCountdown != null)
            {
                gradualPayoutCountdown.EndTimer(false);
                gradualPayoutCountdown = null;
            }
        }

        lastBankValue = argValue;
        
        playerBankText.text = $"Balance: ${argValue:0.00}";
    }
    
    public void SetBankTextGradual(float argFinalValue, float argTime)
    {
        float startingValue = lastBankValue;

        if (gradualBankCountdown != null)
        {
            gradualBankCountdown.EndTimer(false);
        }

        gradualBankCountdown = new Countdown(argTime, true, null, (runTime, totalTime) =>
            {
                float progress = Mathf.Clamp01(runTime / totalTime);
                float newValue = Mathf.Lerp(startingValue, argFinalValue, progress);
                SetPlayerBankText(newValue, false);
            },
            () =>
            {
                SetPlayerBankText(argFinalValue);
            });
    }
    
    public void SetPlayerBetText(float argValue)
    {
        playerBetText.text = $"${argValue:0.00}";
    }
    
    public void SetPayoutText(float argValue, bool argCancelGradual = true)
    {
        if (argCancelGradual)
        {
            if (gradualPayoutCountdown != null)
            {
                gradualPayoutCountdown.EndTimer(false);
                gradualPayoutCountdown = null;
            }
        }

        if (argValue <= 0)
        {
            payoutText.text = "Payout: $0.00";
            lastPayoutValue = 0;
            return;
        }

        lastPayoutValue = argValue;
        payoutText.text = $"Payout: ${argValue:0.00}";
    }
    
    public void SetPayoutTextGradual(float argFinalValue, float argTime)
    {
        float startingValue = lastPayoutValue;

        if (gradualPayoutCountdown != null)
        {
            gradualPayoutCountdown.EndTimer(false);
        }

        gradualPayoutCountdown = new Countdown(argTime, true, null, (runTime, totalTime) =>
            {
                float progress = Mathf.Clamp01(runTime / totalTime);
                float newValue = Mathf.Lerp(startingValue, argFinalValue, progress);
                SetPayoutText(newValue, false);
            },
            () =>
            {
                SetPayoutText(argFinalValue);
            });
    }

    public void SetFreeSpinsVisible(bool argVisible)
    {
        freeSpinsContainer.SetActive(argVisible);
    }
    
    public void SetFreeSpinsText(int argValue)
    {
        freeSpinsText.text = $"{argValue}";
    }
}
