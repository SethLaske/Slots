using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProgressiveController : MonoBehaviour
{

    private int activeObjectIndex = 0;
    public List<GameObject> progressiveObjects = new List<GameObject>();

    private void Awake()
    {
        activeObjectIndex = 0;
        if (progressiveObjects.Count < 1)
        {
            Debug.LogError("Progressives not assigned");
            return;
        }

        for(int i = 0; i < progressiveObjects.Count; i++)
        {
            progressiveObjects[i].SetActive(i == activeObjectIndex);
        }
    }

    public void AttemptIncrement()
    {
        int random = Random.Range(0, 1);//2 + (activeObjectIndex * 2));

        if (random == 0)
        {
            activeObjectIndex ++;
            if (activeObjectIndex >= progressiveObjects.Count)
            {
                ProgressiveManager.instance.AwardFreeSpins();
                activeObjectIndex = 0;
            }

            for(int i = 0; i < progressiveObjects.Count; i++)
            {
                progressiveObjects[i].SetActive(i == activeObjectIndex);
            }
        }
    }
}
