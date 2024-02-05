using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StatBar : MonoBehaviour
{
    private Slider slider;
    private RectTransform rectTransform;

    [Header("Bar Options")]
    [SerializeField] protected bool scaleBarLenghtWithStats = true;
    [SerializeField] protected float widthScaleMultiplier = 1;
    // VARIABLE TO SCALE BAR SIZE DEPENDING ON STAT


    protected virtual void Awake()
    {
        slider = GetComponent<Slider>();
        rectTransform = GetComponent<RectTransform>();
    }

    public virtual void SetStat(int newValue)
    {
        slider.value = newValue;
    }

    public virtual void SetMaxStat(int maxValue)
    {
        slider.maxValue = maxValue;
        slider.value = maxValue;

        if (scaleBarLenghtWithStats)
        {
            rectTransform.sizeDelta = new Vector2(maxValue * widthScaleMultiplier, rectTransform.sizeDelta.y);

            // RESET THE POSITION OF THE BAR BASED ON THEIR LAYOUT GROUP'S SETTINGS
            PlayerUIManager.instance.playerUIHudManager.RefreshHUD();
        }
    }
}
