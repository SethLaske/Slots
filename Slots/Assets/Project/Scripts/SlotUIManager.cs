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
        increaseBetButton.interactable = argEnabled;
        decreaseBetButton.interactable = argEnabled;
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
        
    }

    public void OnDecrementBetButtonPressed()
    {
        
    }

    public void SetPlayerBankText(float argValue)
    {
        playerBankText.text = $"Balance: ${argValue:0.00}";
    }
}
