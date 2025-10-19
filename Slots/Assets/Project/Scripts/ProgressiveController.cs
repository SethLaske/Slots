using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProgressiveController : MonoBehaviour
{
    private int activeTierIndex = 0;
    private int attemptsOnTier = 0;
    public List<GameObject> progressiveObjects = new List<GameObject>();
    public ProgressiveControllerConfig progressiveConfig;
    
    private void Awake()
    {
        activeTierIndex = 0;
        if (progressiveObjects.Count < 1)
        {
            Debug.LogError("Progressives not assigned");
            return;
        }

        for(int i = 0; i < progressiveObjects.Count; i++)
        {
            progressiveObjects[i].SetActive(i == activeTierIndex);
        }

        if (progressiveConfig == null || progressiveConfig.progressiveTiers.Count != progressiveObjects.Count)
        {
            Debug.LogError("Progressive Config not assigned correctly");
        }
    }

    public void AttemptIncrement()
    {
        attemptsOnTier++;

        if (progressiveConfig.progressiveTiers[activeTierIndex].minimumNumberOfSpins > attemptsOnTier)
        {
            return;
        }

        int random = Random.Range(0, 100);

        if (random <= progressiveConfig.progressiveTiers[activeTierIndex].chanceToUpgrade)
        {
            activeTierIndex ++;
            attemptsOnTier = 0;
            if (activeTierIndex >= progressiveObjects.Count)
            {
                ProgressiveManager.instance.AwardFreeSpins();
                activeTierIndex = 0;
            }

            for(int i = 0; i < progressiveObjects.Count; i++)
            {
                progressiveObjects[i].SetActive(i == activeTierIndex);
            }
        }
    }
}
