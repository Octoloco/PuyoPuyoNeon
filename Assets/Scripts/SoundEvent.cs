using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEvent : MonoBehaviour
{
    public SoundCollection collection;
    public int index;
    public bool isRandom = false;
    public bool playAwake=false;
    AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        if (playAwake)
            PlayClip();
    }

    public void SetVolume(float value)
    {
        collection.volume = value;
    }

    public void Update()
    {
        source.volume = collection.volume;
    }
    public void PlayClip()
    {
       
        index = isRandom ? Random.Range(0, collection.audioClips.Count) : index;
        collection.PlayAudioClip( source, index);
    }

    public void PlayClipByIndex(int index)
    {
       
        collection.PlayAudioClip( source, index);
    }

    public void PlayOnDisable(int index)
    {
        collection.PlayOnDisable(index, transform);
    }

    public void StopClip()
    {
        source.Stop();
    }

}
