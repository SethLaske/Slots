using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressiveManager : MonoBehaviour
{
    public static ProgressiveManager instance;

    public List<ProgressiveController> progressiveControllers = new List<ProgressiveController>();

    public int numberOfFreeSpinsRemaining { get; private set; }

    private void Awake()
    {
        instance = this;

        if (progressiveControllers.Count < 1)
        {
            Debug.LogError("No progressive controllers assigned");
        }
    }

    public void OnWildShown()
    {
        foreach (ProgressiveController progressiveController in progressiveControllers)
        {
            int random = Random.Range(0, 3);

            if (random == 0)
            {
                progressiveController.AttemptIncrement();
            }
        }
    }

    public void AwardFreeSpins()
    {
        numberOfFreeSpinsRemaining += SlotGameController.instance.gameConfig.freeSpinsAwarded;
        Debug.Log($"[Progressive] Activated reward, Free Spins {numberOfFreeSpinsRemaining}");
    }

    public void StartedFreeSpin()
    {
        numberOfFreeSpinsRemaining--;
        Debug.Log($"[Progressive] Used reward, Remaining Spins {numberOfFreeSpinsRemaining}");
    }
}
