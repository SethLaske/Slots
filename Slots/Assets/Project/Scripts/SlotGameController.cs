using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;

public class SlotGameController : MonoBehaviour
{
    public static SlotGameController instance;
    
    public List<SlotReelController> slotCells = new List<SlotReelController>();

    
    public List<SlotReelPayLineStartController> payLineControllers = new List<SlotReelPayLineStartController>();

    [FormerlySerializedAs("betAmount")] public float activeBetAmount = 0;
    
    private void Awake()
    {
        instance = this;
    }

    [ContextMenu("Start Spinning")]
    public void StartSpinning(float argBetAmount)
    {
        SlotUIManager.instance.SetInputEnabled(false);
        SlotUIManager.instance.SetPayoutText(0);
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
        float timeTilFinish = 3;
        foreach (SlotReelController cell in slotCells)
        {
            cell.StopSpinning(timeTilFinish);
            timeTilFinish += .2f;
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
        }

        float totalWinnings = 0;
        StringBuilder winningLines = new StringBuilder();
        foreach (SlotWinningResult winner in winningResults)
        {
            totalWinnings += winner.payout;
            winningLines.AppendLine(winner.GetDescription());

            winner.SetHighlightVisibility(true);
        }

        Debug.Log($"Total payout amount: {totalWinnings} \n {winningLines}");
        
        SlotCurrencyController.instance.AdjustBank(totalWinnings);
        SlotUIManager.instance.SetPayoutText(totalWinnings);
        SlotUIManager.instance.SetInputEnabled(true);
        activeBetAmount = 0;
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
