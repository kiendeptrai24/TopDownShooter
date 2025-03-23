
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TransparentOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    protected Dictionary<TextMeshProUGUI, Color> originalTextColors = new();
    protected Dictionary<Image, Color> originalImageColors = new();
    [Range(0,1)]
    [SerializeField] private float alphaText = .5f;
    [Range(0,1)]
    [SerializeField] private float alphaImage = .2f;
    [Header("Audio")]
    [SerializeField] protected AudioSource pointerEnterSFX;
    [SerializeField] protected AudioSource pointerDownSFX;


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
        if(pointerEnterSFX != null)
            pointerEnterSFX.Play();
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
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if(pointerDownSFX != null)
            pointerDownSFX?.Play();
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
    public void AssignAudioSource()
    {
        pointerEnterSFX = GameObject.Find("UI_Pointer_Enter").GetComponent<AudioSource>();
        pointerDownSFX = GameObject.Find("UI_Pointer_Down").GetComponent<AudioSource>();
    }

}

