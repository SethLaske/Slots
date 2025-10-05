using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SlotCurrencyController : MonoBehaviour
{
    public static SlotCurrencyController instance;
    
    public float playerBank = 100;

    private int playerBetAmountIndex = 0;
    private float[] playerBetAmountOptions = { 1, 2, 3, 5 };
    public float playerBetAmount => playerBetAmountOptions[playerBetAmountIndex];

    public bool canIncrement => playerBetAmountIndex < playerBetAmountOptions.Length - 1;
    public bool canDecrement => playerBetAmountIndex > 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SlotUIManager.instance.SetPlayerBankText(playerBank);
        UpdateBetAmount();
    }

    public bool TryBet(out float betAmount)
    {
        if (playerBank >= playerBetAmount)
        {
            AdjustBank(-playerBetAmount);
            betAmount = playerBetAmount;
            return true;
        }

        betAmount = 0;
        return false;
    }

    public void TryIncreaseBet()
    {
        playerBetAmountIndex++;

        UpdateBetAmount();
    }
    
    public void TryDecreaseBet()
    {
        playerBetAmountIndex--;

        UpdateBetAmount();
    }

    public void UpdateBetAmount()
    {
        playerBetAmountIndex = Mathf.Clamp(playerBetAmountIndex, 0, playerBetAmountOptions.Length - 1);

        SlotUIManager.instance.SetPlayerBetText(playerBetAmount);
        SlotUIManager.instance.SetBetAmountButtonEnabled(canIncrement, canDecrement);
    }

    public void AdjustBank(float argAmount)
    {
        playerBank += argAmount;
        SlotUIManager.instance.SetPlayerBankText(playerBank);
    }
}
