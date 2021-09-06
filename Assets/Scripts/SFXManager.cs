using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SFXManager : MonoBehaviour
{
    [SerializeField]
    private SoundEvent sfxScript;

    [SerializeField]
    private Slider slider;

    public static SFXManager instance;

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
            sfxScript.SetVolume(slider.value);
        }
    }

    public void PlayClip(int index)
    {
        sfxScript.PlayClipByIndex(index);
    }

    public void PlayClipWithDelay(int index, float time)
    {
        StartCoroutine(WaitToPlay(time, index));
    }

    public void StopClip()
    {
        sfxScript.StopClip();
    }

    IEnumerator WaitToPlay(float time, int index)
    {
        yield return new WaitForSeconds(time);
        sfxScript.PlayClipByIndex(index);
    }
}
