using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenManager : MonoBehaviour
{
    public static TweenManager instance;

    private List<Tween> allTweens = new List<Tween>();

    private List<Tween> newTweens = new List<Tween>();
    private List<Tween> endedTweens = new List<Tween>();
    void Awake()
    {
        if (instance == null)
        {

            instance = this;
        }
        else if (instance != this)
        {

            Destroy(gameObject);
        }
    }

    public void DoUpdate(float delta) 
    {
        if (newTweens.Count > 0)
        {
            foreach (Tween newTween in newTweens)
            {
                allTweens.Add(newTween);
            }
            newTweens.Clear();
        }

        if (endedTweens.Count > 0) 
        {
            foreach (Tween endedTween in endedTweens)
            {
                if (allTweens.Contains(endedTween))
                {
                    allTweens.Remove(endedTween);
                }
            }
            endedTweens.Clear();
        }

        foreach (Tween tween in allTweens)
        {
            tween.UpdateTweener(delta);
        }
    }

    //New and ended tweens are handled seperately to avoid errors with foreach loops
    public void AddNewTween(Tween argTween) 
    { 
        newTweens.Add(argTween);
    }

    public void AddEndedTween(Tween argEndedTween)
    {
        endedTweens.Add(argEndedTween);
    }


}