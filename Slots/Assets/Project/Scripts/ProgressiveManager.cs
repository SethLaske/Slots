using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProgressiveManager : MonoBehaviour
{
    public static ProgressiveManager instance;

    public List<ProgressiveController> progressiveControllers = new List<ProgressiveController>();

    public GameObject snowballPrefab;

    public int numberOfFreeSpinsRemaining { get; private set; }

    private const string PROGRESSIVE_CONTROLLERS_PREFIX_KEY = "ProgressiveController{0}";

    private void Awake()
    {
        instance = this;

        if (progressiveControllers.Count < 1)
        {
            Debug.LogError("No progressive controllers assigned");
        }

        LoadData();
    }

    private void Start()
    {
        SlotUIManager.instance.SetFreeSpinsVisible(numberOfFreeSpinsRemaining > 0);
        SlotUIManager.instance.SetFreeSpinsText(numberOfFreeSpinsRemaining);
    }

    public void LoadData()
    {
        for (int i = 0; i < progressiveControllers.Count; i++)
        {
            progressiveControllers[i].LoadData(string.Format(PROGRESSIVE_CONTROLLERS_PREFIX_KEY, i));
        }
    }

    public void SaveData()
    {
        for (int i = 0; i < progressiveControllers.Count; i++)
        {
            progressiveControllers[i].SaveData(string.Format(PROGRESSIVE_CONTROLLERS_PREFIX_KEY, i));
        }
    }

    public void OnWildShown()
    {
        foreach (ProgressiveController progressiveController in progressiveControllers)
        {
            int random = Random.Range(0, 3);

            if (random == 0)
            {
                GameObject snowBallClone = Instantiate(snowballPrefab, SlotGameController.instance.transform);

                new Tween(1, snowBallClone.transform, snowBallClone.transform.position,
                    progressiveController.transform.GetChild(0).position, true, null, null,
                    () =>
                    {
                        progressiveController.AttemptIncrement();
                        Destroy(snowBallClone);
                    });

            }
        }
    }

    public void AwardFreeSpins()
    {
        numberOfFreeSpinsRemaining += SlotGameController.instance.gameConfig.freeSpinsAwarded;
        Debug.Log($"[Progressive] Activated reward, Free Spins {numberOfFreeSpinsRemaining}");
        SlotUIManager.instance.SetFreeSpinsVisible(true);
        SlotUIManager.instance.SetFreeSpinsText(numberOfFreeSpinsRemaining);
    }

    public void StartedFreeSpin()
    {
        numberOfFreeSpinsRemaining--;
        Debug.Log($"[Progressive] Used reward, Remaining Spins {numberOfFreeSpinsRemaining}");
        SlotUIManager.instance.SetFreeSpinsText(numberOfFreeSpinsRemaining);
    }
}
