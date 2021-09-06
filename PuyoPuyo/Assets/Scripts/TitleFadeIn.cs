using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TitleFadeIn : MonoBehaviour
{
    TextMeshProUGUI tmPro;
    public float speed;
    float intensityMul = 0;
    public bool activate = false;

    void Start()
    {
        tmPro = GetComponent<TextMeshProUGUI>();
        tmPro.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, -.8f);
    }

    // Update is called once per frame
    void Update()
    {
        if (activate)
        {
            if (tmPro.fontMaterial.GetFloat(ShaderUtilities.ID_FaceDilate) < 0f)
            {
                intensityMul = speed * Time.deltaTime;
                tmPro.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, tmPro.fontMaterial.GetFloat(ShaderUtilities.ID_FaceDilate) + intensityMul);
            }
            else
            {
                tmPro.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, 0f);
            }
        }
    }
}
