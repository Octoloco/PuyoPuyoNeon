using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonGlowScript : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (GetComponent<Button>().interactable)
        {
            GetComponentInChildren<TextMeshProUGUI>().fontMaterial.SetFloat(ShaderUtilities.ID_GlowPower, .2f);
        }
    }

    private void OnMouseEnter()
    {
        if (GetComponent<Button>().interactable)
        {
            SFXManager.instance.PlayClip(0);
            GetComponentInChildren<TextMeshProUGUI>().fontMaterial.SetFloat(ShaderUtilities.ID_GlowPower, 1f);
        }
    }

    private void OnMouseExit()
    {
        if (GetComponent<Button>().interactable)
        {
            //SFXManager.instance.PlayClip(0);
            GetComponentInChildren<TextMeshProUGUI>().fontMaterial.SetFloat(ShaderUtilities.ID_GlowPower, .2f);
        }
    }
}
