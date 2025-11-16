using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }

    private Queue<AudioSource> availableSources = new Queue<AudioSource>();

    public AudioSource sFXPrefab;

    private int spawnAmount = 5;
    private AudioSource referencedSource;
    
    public AudioClip reelStartSpinning;
    public AudioClip reelMidSpinning;
    public AudioClip reelEndSpinning;
    
    public AudioClip paidSound;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (availableSources.Count <= 0)
        {
            SpawnObjects();
        }
    }

    private void SpawnObjects()
    {
        if (sFXPrefab == null)
        {
            return;
        }
        for (int i = 0; i < spawnAmount; i++)
        {
            
            ReturnSourceToPool(Instantiate(sFXPrefab, transform));
        }
    }

    public void ReturnSourceToPool(AudioSource source)
    {
        availableSources.Enqueue(source);
        //source.gameObject.SetActive(false);
        source.transform.parent = transform;
    }

    public void PlaySound(AudioClip clip) {
        if (clip == null)
        {
            return;
        }

        referencedSource = GetSourceFromPool();
        referencedSource.clip = clip;
        referencedSource.Play();
        SFXManager.Instance.ReturnSourceToPool(referencedSource);
    }

    public void PlaySound(AudioClip clip, Transform parent) {
        if (clip == null)
        {
            return;
        }
        
        referencedSource = GetSourceFromPool();
        //referencedSource.transform.parent = parent;
        referencedSource.transform.position = parent.position;
        referencedSource.clip = clip;
        referencedSource.Play();
        SFXManager.Instance.ReturnSourceToPool(referencedSource);
        //StartCoroutine(EndOfAudio(clip.length, referencedSource));
    }

    /*public void PlaySound() { 
        
    }*/

    public AudioSource GetSourceFromPool()
    {
        if (availableSources.Count <= 0)
        {
            SpawnObjects();
        }

        referencedSource = availableSources.Dequeue();

        referencedSource.gameObject.SetActive(true);

        return referencedSource;
    }

    public AudioSource GetSourceFromPool(Vector3 spawnPos)
    {
        referencedSource = GetSourceFromPool();
        referencedSource.transform.position = spawnPos;
        return referencedSource;
    }

    IEnumerator EndOfAudio(float time, AudioSource source) { 
        yield return new WaitForSeconds(time);

        SFXManager.Instance.ReturnSourceToPool(source);
    }

    void OnSoundFinished(AudioSource source) {
        Debug.Log("Sound finished");
        
    }
}
