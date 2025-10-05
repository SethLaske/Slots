using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotReelPayLineStartController : MonoBehaviour
{
    public const int MINIMUM_CONSECUTIVE_CELLS = 3;
    
    public SlotReelController startReel;
    
    public List<SlotReelPayLine> payLines;

    public List<SlotWinningLineResult> GetWinningResults(float argBetAmount)
    {
        List<SlotWinningLineResult> winningLineResults = new List<SlotWinningLineResult>();
        
        SlotCellOption startingCellOption = startReel.GetSelectedCellOption();

        if (startingCellOption is NumberCell startingNumberCell)
        {
            foreach (SlotReelPayLine payLine in payLines)
            {
                List<SlotReelController> winningReels = payLine.GetMatchingCellsCount(startingCellOption.uniqueID);
                winningReels.Insert(0, startReel);
                if (winningReels.Count >= MINIMUM_CONSECUTIVE_CELLS)
                {
                    winningLineResults.Add(new SlotWinningLineResult(startingNumberCell, winningReels, argBetAmount));
                }
            }
        }

        return winningLineResults;
    }
}

[Serializable]
public class SlotReelPayLine
{
    [SerializeField, Tooltip("Don't include the row start cell controller")]
    private List<SlotReelController> orderedSlotReels;

    /// Returns 1 if next cell doesn't match
    public List<SlotReelController> GetMatchingCellsCount(int firstCellID)
    {
        List<SlotReelController> matchingReels = new List<SlotReelController>();
        
        for (int i = 0; i < orderedSlotReels.Count; i++)
        {
            SlotCellOption nextCellOption = orderedSlotReels[i].GetSelectedCellOption();
            
            if (nextCellOption.uniqueID == firstCellID || nextCellOption is WildCell)
            {
                matchingReels.Add(orderedSlotReels[i]);
            }
            else
            {
                break;
            }
        }
        
        return matchingReels;
    }
}
