using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class SlotGameController : MonoBehaviour
{
    public static SlotGameController instance;
    
    public List<SlotReelController> slotCells = new List<SlotReelController>();
    
    public List<SlotReelPayLineStartController> payLineControllers = new List<SlotReelPayLineStartController>();

    public float activeBetAmount = 0;

    private bool isInFreeSpinMode = false;

    private float storedWinnings = 0;
    
    private void Awake()
    {
        instance = this;
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
        float timeTilFinish = 2f;
        foreach (SlotReelController cell in slotCells)
        {
            cell.StopSpinning(timeTilFinish);
            timeTilFinish += .1f;
        }
        
        //Invoke(nameof(GetResults), 4);
    }

    [ContextMenu("Get Results")]
    public void GetResults()
    {
        List<SlotWinningResult> winningResults = new List<SlotWinningResult>();
        
        foreach (SlotReelPayLineStartController payLineController in payLineControllers)
        {
            winningResults.AddRange(payLineController.GetWinningResults(activeBetAmount));
        }

        bool isWildActive = false;
        List<SlotWinningPrizeResult> prizeResults = new List<SlotWinningPrizeResult>();
        foreach (SlotReelController cell in slotCells)
        {
            if (cell.GetSelectedCellOption() is WildCell)
            {
                isWildActive = true;
                continue;
            }
            
            if (cell.GetSelectedCellOption() is PrizeCell prizeCell)
            {
                prizeResults.Add(new SlotWinningPrizeResult(prizeCell, cell, activeBetAmount));
            }
        }

        if (isWildActive)
        {
            winningResults.AddRange(prizeResults);
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
        }
        else
        {
            //Invoke(nameof(TryStartSpinning), .5f);
            TryStartSpinning();
        }

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
