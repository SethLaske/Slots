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
        activeBetAmount = SlotCurrencyController.instance.playerBetAmount;
        
        foreach (SlotReelController cell in slotCells)
        {
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
        List<SlotPayLineResult> winningPayLines = new List<SlotPayLineResult>();
        foreach (SlotReelPayLineStartController payLineController in payLineControllers)
        {
            winningPayLines.AddRange(payLineController.GetWinningResults());
        }

        float totalWinnings = 0;
        StringBuilder winningLines = new StringBuilder();
        foreach (SlotPayLineResult payLine in winningPayLines)
        {
            totalWinnings += payLine.payoutMultiplier * activeBetAmount;
            winningLines.AppendLine($"{payLine.cellCount} of {payLine.winningOption.uniqueID} pays {payLine.payoutMultiplier * activeBetAmount}");
        }
        
        Debug.Log($"Total payout amount: {totalWinnings} \n {winningLines}");
        
        SlotCurrencyController.instance.AdjustBank(totalWinnings);
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
