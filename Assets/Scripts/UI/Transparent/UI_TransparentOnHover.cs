using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TransparentOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected Dictionary<TextMeshProUGUI, Color> originalTextColors = new();
    protected Dictionary<Image, Color> originalImageColors = new();
    [Range(0,1)]
    [SerializeField] private float alphaText = .5f;
    [Range(0,1)]
    [SerializeField] private float alphaImage = .2f;


    protected virtual void Start() {
        foreach (var image in GetComponentsInChildren<Image>(true))
        {
            originalImageColors[image] = image.color;
        }
        foreach (var text in GetComponentsInChildren<TextMeshProUGUI>(true))
        {
            originalTextColors[text] = text.color;
        }
    }
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        foreach (var image in originalImageColors.Keys)
        {
            SetAlphaImage(image, alphaImage);
        }
        foreach (var text in originalTextColors.Keys)
        {
            SetAlphaText(text, alphaText);
        }
        
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        this.ResetColors();
    }

    public void ResetColors()
    {
        foreach (var image in originalImageColors.Keys)
        {
            image.color = originalImageColors[image];
        }
        foreach (var text in originalTextColors.Keys)
        {
            text.color = originalTextColors[text];
        }
    }
    public void SetAlphaImage(Image image, float alpha)
    {
        var color = image.color;
        color.a = alpha;
        image.color = color;
    }
    public void SetAlphaText(TextMeshProUGUI text, float alpha)
    {
        var color = text.color;
        color.a = alpha;
        text.color = color;
    }

}

