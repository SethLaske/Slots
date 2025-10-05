using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotUIManager : MonoBehaviour
{
    public static SlotUIManager instance;

    [SerializeField] private TextMeshProUGUI playerBankText;
    [SerializeField] private TextMeshProUGUI playerBetText;
    [SerializeField] private TextMeshProUGUI payoutText;
    [SerializeField] private Button spinButton;
    [SerializeField] private Button increaseBetButton;
    [SerializeField] private Button decreaseBetButton;

    [SerializeField] private SlotGameController gameController;
    
    private void Awake()
    {
        instance = this;
        
        spinButton.onClick.AddListener(OnSpinButtonPressed);
        increaseBetButton.onClick.AddListener(OnIncrementBetButtonPressed);
        decreaseBetButton.onClick.AddListener(OnDecrementBetButtonPressed);
    }

    public void SetInputEnabled(bool argEnabled)
    {
        spinButton.interactable = argEnabled;
        increaseBetButton.interactable = argEnabled && SlotCurrencyController.instance.canIncrement;
        decreaseBetButton.interactable = argEnabled && SlotCurrencyController.instance.canDecrement;
    }

    public void OnSpinButtonPressed()
    {
        if (SlotCurrencyController.instance.TryBet(out float betAmount))
        {
            gameController.StartSpinning(betAmount);
        }
    }
    
    public void OnIncrementBetButtonPressed()
    {
        SlotCurrencyController.instance.TryIncreaseBet();
    }

    public void OnDecrementBetButtonPressed()
    {
        SlotCurrencyController.instance.TryDecreaseBet();
    }

    public void SetBetAmountButtonEnabled(bool argCanIncrement, bool argCanDecrement)
    {
        increaseBetButton.interactable = argCanIncrement;
        decreaseBetButton.interactable = argCanDecrement;
    }

    public void SetPlayerBankText(float argValue)
    {
        playerBankText.text = $"Balance: ${argValue:0.00}";
    }
    
    public void SetPlayerBetText(float argValue)
    {
        playerBetText.text = $"${argValue:0.00}";
    }

    public void SetPayoutText(float argValue)
    {
        if (argValue <= 0)
        {
            payoutText.text = "Payout:";
            return;
        }

        payoutText.text = $"Payout: ${argValue:0.00}";
    }
}
