using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TitleBreathScript : MonoBehaviour
{
    TextMeshProUGUI tmPro;
    public float speed;
    float intensityMul = 0;
    public bool activate = false;
    private int mod = 1;

    void Start()
    {
        tmPro = GetComponent<TextMeshProUGUI>();
        tmPro.fontMaterial.SetFloat(ShaderUtilities.ID_GlowPower, .75f);
    }

    void Update()
    {
        if (tmPro.fontMaterial.GetFloat(ShaderUtilities.ID_GlowPower) <= .25f)
        {
            tmPro.fontMaterial.SetFloat(ShaderUtilities.ID_GlowPower, .25f);
            mod = 1;
        }
        else if (tmPro.fontMaterial.GetFloat(ShaderUtilities.ID_GlowPower) >= .75f)
        {
            tmPro.fontMaterial.SetFloat(ShaderUtilities.ID_GlowPower, .75f);
            mod = -1;
        }

        intensityMul = speed * Time.deltaTime * mod;
        tmPro.fontMaterial.SetFloat(ShaderUtilities.ID_GlowPower, tmPro.fontMaterial.GetFloat(ShaderUtilities.ID_GlowPower) + intensityMul);

    }
}
