using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIPanel : MonoBehaviour
{
    [SerializeField]
    private bool startHidden = true;
    private bool canGrow;
    private bool canShrink;

    [SerializeField]
    private float scaleFactor;

    private void Awake()
    {
        if (startHidden)
        {
            if (GetComponent<Image>() != null)
            {
                GetComponent<Image>().enabled = false;
            }

            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
                transform.GetChild(i).GetComponent<RectTransform>().localScale = Vector3.zero;
            }
        }
        else
        {
            if (GetComponent<Image>() != null)
            {
                GetComponent<Image>().enabled = true;
            }

            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
                transform.GetChild(i).GetComponent<RectTransform>().localScale = Vector3.one;
            }
        }
    }

    protected void Update()
    {
        if (canGrow)
        {
            GrowChildren();
        }

        if (canShrink)
        {
            ShrinkChildren();
        }
    }

    virtual public void Show()
    {
        if (GetComponent<Image>() != null)
        {
            GetComponent<Image>().enabled = true;
        }

        canGrow = true;
    }

    virtual public void Hide()
    {
        if (GetComponent<Image>() != null)
        {
            GetComponent<Image>().enabled = false;
        }

        canShrink = true;
    }

    public void GrowChildren()
    {
        if (transform.GetChild(0).GetComponent<RectTransform>().localScale.x < 1) {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
                transform.GetChild(i).GetComponent<RectTransform>().localScale += new Vector3(scaleFactor * Time.deltaTime, scaleFactor * Time.deltaTime, scaleFactor * Time.deltaTime);
            }
        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<RectTransform>().localScale = Vector3.one;
            }
            canGrow = false;
        }
    }

    public void ShrinkChildren()
    {
        if (transform.GetChild(0).GetComponent<RectTransform>().localScale.x > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
                transform.GetChild(i).GetComponent<RectTransform>().localScale += new Vector3(-scaleFactor * Time.deltaTime, -scaleFactor * Time.deltaTime, -scaleFactor * Time.deltaTime);
            }
        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<RectTransform>().localScale = Vector3.zero;
            }
            canShrink = false;
        }
    }
}
