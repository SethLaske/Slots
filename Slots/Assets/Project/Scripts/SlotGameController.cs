using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotGameController : MonoBehaviour
{
    public List<SlotReelController> slotCells = new List<SlotReelController>();

    
    public List<SlotReelPayLineStartController> payLineControllers = new List<SlotReelPayLineStartController>();
    
    [ContextMenu("Start Spinning")]
    public void StartSpinning()
    {
        foreach (SlotReelController cell in slotCells)
        {
            cell.StartSpinning();
        }
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
    }

    [ContextMenu("Get Results")]
    public void GetResults()
    {
        List<SlotPayLineResult> winningPayLines = new List<SlotPayLineResult>();
        foreach (SlotReelPayLineStartController payLineController in payLineControllers)
        {
            winningPayLines.AddRange(payLineController.GetWinningResults());
        }
        Debug.Log("Winning Lines: " + winningPayLines.Count);
    }
}
