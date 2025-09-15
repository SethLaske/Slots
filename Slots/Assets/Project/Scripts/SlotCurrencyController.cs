using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SlotCurrencyController : MonoBehaviour
{
    public static SlotCurrencyController instance;
    
    public float playerBank = 100;
    public float playerBetAmount = 1;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SlotUIManager.instance.SetPlayerBankText(playerBank);
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

    public void AdjustBank(float argAmount)
    {
        playerBank += argAmount;
        SlotUIManager.instance.SetPlayerBankText(playerBank);
    }
}
