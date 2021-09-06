using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSettingsChange : MonoBehaviour
{
    [SerializeField]
    GameObject settingsPanel;
    [SerializeField]
    GameObject mainPanel;

    public void OpenSettings()
    {
        SFXManager.instance.PlayClip(0);
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        SFXManager.instance.PlayClip(0);
        settingsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }
}
