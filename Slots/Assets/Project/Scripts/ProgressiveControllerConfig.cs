using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Progressive Controller Config", menuName = "Slots/Progressive Controller Config")]
public class ProgressiveControllerConfig : ScriptableObject
{
    public List<ProgressiveTier> progressiveTiers;
}

[Serializable]
public class ProgressiveTier
{
    public int minimumNumberOfSpins = 0;
    
    [Tooltip("Enter percent as whole number (50 = 50%)")]
    public int chanceToUpgrade = 50;
}
