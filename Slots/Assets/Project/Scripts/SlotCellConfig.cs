using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Slot Cell Config", menuName = "Slots/Cell Config")]
public class SlotCellConfig : ScriptableObject
{
    public List<SlotCellOption> cellOptions = new List<SlotCellOption>();

    public SlotCellOption GetRandomSlotCellWithoutRepeats(List<SlotCellController> currentControllers)
    {
        HashSet<int> usedIDs = new HashSet<int>();

        foreach (SlotCellController cellController in currentControllers)
        {
            usedIDs.Add(cellController.cellOption.uniqueID);
        }

        return GetRandomSlotCell(usedIDs);
    }

    public SlotCellOption GetRandomSlotCell(HashSet<int> nonViableIDs = null)
    {
        if (nonViableIDs == null)
        {
            nonViableIDs = new HashSet<int>();
        }

        float weightSum = 0;
        List<SlotCellOption> viableCells = new List<SlotCellOption>();

        foreach (SlotCellOption cell in cellOptions)
        {
            if (cell.weight > 0 && nonViableIDs.Contains(cell.uniqueID) == false)
            {
                weightSum += cell.weight;
                viableCells.Add(cell);
            }
        }

        if (viableCells.Count == 0)
        {
            Debug.LogError("[SlotCellConfig] No viable cells, returning null");
            return null;
        }

        float randomSum = Random.Range(0, weightSum);
        foreach (SlotCellOption validCell in viableCells)
        {
            randomSum -= validCell.weight;
            
            if (randomSum <= 0)
            {
                return validCell;
            }
        }
        
        
        Debug.LogError("[SlotCellConfig] Weight calc failed, returning null");
        return null;
        
    }

}

[Serializable]
public class SlotCellOption
{
    public int uniqueID = -1;
    public Sprite image = null;
    public float weight = 1;
    public bool isWild = false;
}
