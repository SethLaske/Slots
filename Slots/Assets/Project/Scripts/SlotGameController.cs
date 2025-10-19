using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

public class SlotGameController : MonoBehaviour
{
    public static SlotGameController instance;

    public GameConfig gameConfig = null;
    
    public List<SlotReelController> slotCells = new List<SlotReelController>();
    
    public List<SlotReelPayLineStartController> payLineControllers = new List<SlotReelPayLineStartController>();

    public float activeBetAmount = 0;

    private bool isInFreeSpinMode = false;

    private float storedWinnings = 0;
    
    private void Awake()
    {
        instance = this;
    }

    public void SaveData()
    {
        SlotCurrencyController.instance.SaveData();
        ProgressiveManager.instance.SaveData();
    }

    [MenuItem("Seth/Clear Data")]
    public static void ClearAllData()
    {
        PlayerPrefs.DeleteAll();
    }

    public void TryStartSpinning()
    {
        if (ProgressiveManager.instance.numberOfFreeSpinsRemaining > 0)
        {
            isInFreeSpinMode = true;
            ProgressiveManager.instance.StartedFreeSpin();
            StartSpinning(SlotCurrencyController.instance.playerBetAmount);
            return;
        }

        if (SlotCurrencyController.instance.TryBet(out float betAmount))
        {
            SlotUIManager.instance.SetPayoutText(0);
            StartSpinning(betAmount);
        }
    }

    [ContextMenu("Start Spinning")]
    public void StartSpinning(float argBetAmount)
    {
        SlotUIManager.instance.SetInputEnabled(false);
        activeBetAmount = SlotCurrencyController.instance.playerBetAmount;
        
        foreach (SlotReelController cell in slotCells)
        {
            cell.SetHighlightVisibility(false);
            cell.StartSpinning();
        }
        
        StopSpinning();
    }

    [ContextMenu("Stop Spinning")]
    public void StopSpinning()
    {
        float timeTilFinish = gameConfig.spinTime;
        foreach (SlotReelController cell in slotCells)
        {
            cell.StopSpinning(timeTilFinish);
            timeTilFinish += gameConfig.timeBetweenReels;
        }
        
        //Invoke(nameof(GetResults), 4);
    }

    [ContextMenu("Get Results")]
    public void GetResults()
    {
        bool isWildActive = false;
        
        foreach (SlotReelController cell in slotCells)
        {
            if (cell.GetSelectedCellOption() is WildCell)
            {
                isWildActive = true;
                break;
            }
        }

        List<SlotWinningResult> winningResults = GetWinningResults(isWildActive);

        if (isWildActive)
        {
            ProgressiveManager.instance.OnWildShown();
        }
        
        float roundWinnings = 0;
        StringBuilder winningLines = new StringBuilder();
        foreach (SlotWinningResult winner in winningResults)
        {
            roundWinnings += winner.payout;
            winningLines.AppendLine(winner.GetDescription());

            winner.SetHighlightVisibility(true);
        }

        storedWinnings += roundWinnings;
        Debug.Log($"Total payout amount: {roundWinnings} \n {winningLines}");
        SlotUIManager.instance.SetPayoutText(storedWinnings);

        if (isInFreeSpinMode == false || ProgressiveManager.instance.numberOfFreeSpinsRemaining == 0)
        {
            SlotCurrencyController.instance.AdjustBank(storedWinnings);
            storedWinnings = 0;
            SlotUIManager.instance.SetInputEnabled(true);
            activeBetAmount = 0;
            isInFreeSpinMode = false;

            SaveData();
        }
        else
        {
            TryStartSpinning();
        }
    }

    private List<SlotWinningResult> GetWinningResults(bool isWildActive)
    {
        List<SlotWinningResult> winningResults = new List<SlotWinningResult>();
        
        foreach (SlotReelPayLineStartController payLineController in payLineControllers)
        {
            winningResults.AddRange(payLineController.GetWinningResults(activeBetAmount));
        }
        
        foreach (SlotReelController cell in slotCells)
        {
            
            if (cell.GetSelectedCellOption() is PrizeCell prizeCell)
            {
                winningResults.Add(new SlotWinningPrizeResult(prizeCell, cell, activeBetAmount));
            }
        }

        return winningResults;
    }

    public void OnReelStoppedSpinning()
    {
        foreach (SlotReelController cell in slotCells)
        {
            if (cell.isSpinning)
            {
                return;
            }
        }
        
        //All reels stopped spinning
        
        GetResults();
    }
}
