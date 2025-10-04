using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Number Cell", menuName = "Slots/Number Cell")]
public class NumberCell : SlotCellOption
{
    
    //Payouts
    public float x3Multiplier = 1;
    public float x4Multiplier = 1;
    public float x5Multiplier = 1;

    public float GetPayoutMultiplier(int matchingCount)
    {
        switch (matchingCount)
        {
            case 3:
                return x3Multiplier;
            case 4:
                return x4Multiplier;
            case 5:
                return x5Multiplier;
        }

        Debug.LogError("[SlotCellConfig] Invalid matching count, no payout");
        return 0;
    }
}