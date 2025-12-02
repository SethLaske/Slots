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
    private float[] playerBetAmountOptions = { .5f, 1, 2, 3, 5, 10 };
    public float playerBetAmount => playerBetAmountOptions[playerBetAmountIndex];

    public bool canIncrement => playerBetAmountIndex < playerBetAmountOptions.Length - 1;
    public bool canDecrement => playerBetAmountIndex > 0;

    private const string PLAYER_BANK_KEY = "PlayerBank";
    private const string PLAYER_BET_INDEX_KEY = "PlayerBetIndex";
    
    private void Awake()
    {
        instance = this;
        
        
    }

    private void Start()
    {
        LoadData();
        
        SlotUIManager.instance.SetPlayerBankText(playerBank);
        UpdateBetAmount();
    }
    
    public void SaveData()
    {
        PlayerPrefs.SetFloat(PLAYER_BANK_KEY, playerBank);
        PlayerPrefs.SetInt(PLAYER_BET_INDEX_KEY, playerBetAmountIndex);
    }

    private void LoadData()
    {
        playerBank = PlayerPrefs.GetFloat(PLAYER_BANK_KEY, SlotGameController.instance.gameConfig.defaultBankAmount);
        playerBetAmountIndex = Mathf.Clamp(PlayerPrefs.GetInt(PLAYER_BET_INDEX_KEY, 0), 0, playerBetAmountOptions.Length - 1);
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

        if (argAmount > playerBetAmount * 2)
        {
            SlotUIManager.instance.SetBankTextGradual(playerBank, Mathf.Log10(argAmount)/2);
        }
        else
        {
            SlotUIManager.instance.SetPlayerBankText(playerBank);
        }

    }
}
