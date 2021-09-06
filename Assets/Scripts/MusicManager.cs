using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    private SoundEvent musicScript;

    [SerializeField]
    private Slider slider;

    public static MusicManager instance;

    private void Awake()
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

    private void Update()
    {
        if (slider != null)
        {
            musicScript.SetVolume(slider.value);
        }
    }

    public void PlayClip(int index)
    {
        musicScript.PlayClipByIndex(index);
    }

    public void StopClip()
    {
        musicScript.StopClip();
    }
}
