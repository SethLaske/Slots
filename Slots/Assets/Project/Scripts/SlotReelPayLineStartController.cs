using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotReelPayLineStartController : MonoBehaviour
{
    public const int MINIMUM_CONSECUTIVE_CELLS = 3;
    
    public SlotReelController startReel;
    
    public List<SlotReelPayLine> payLines;

    public List<SlotPayLineResult> GetWinningResults()
    {
        List<SlotPayLineResult> winningLineResults = new List<SlotPayLineResult>();
        
        SlotCellOption startingCellOption = startReel.GetSelectedCellOption();
        
        foreach (SlotReelPayLine payLine in payLines)
        {
            int matchingCells = payLine.GetMatchingCellsCount(startingCellOption.uniqueID);
            if (matchingCells >= MINIMUM_CONSECUTIVE_CELLS)
            {
                winningLineResults.Add(new SlotPayLineResult(startingCellOption, matchingCells));
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
    public int GetMatchingCellsCount(int firstCellID)
    {
        for (int i = 0; i < orderedSlotReels.Count; i++)
        {
            SlotCellOption nextCellOption = orderedSlotReels[i].GetSelectedCellOption();
            
            if (nextCellOption.uniqueID != firstCellID && nextCellOption.isWild == false)
            {
                return i + 1;
            }
        }
        
        return orderedSlotReels.Count + 1;
    }
}

public class SlotPayLineResult
{
    public SlotCellOption winningOption;
    public int cellCount;
    public float payoutMultiplier;

    public SlotPayLineResult(SlotCellOption argOption, int argCount)
    {
        winningOption = argOption;
        cellCount = argCount;
        payoutMultiplier = argOption.GetPayoutMultiplier(argCount);
    }
}
