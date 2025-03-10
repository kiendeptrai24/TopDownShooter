using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Button : UI_TransparentOnHover, IPointerDownHandler
{
    [SerializeField] private Image focus;
    [Range(0, 1)]
    [SerializeField] private float alphaFocus = 1f;
    protected override void Start()
    {
        base.Start();
        focus?.gameObject.SetActive(false);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        focus?.gameObject.SetActive(true);
        SetAlphaImage(focus, alphaFocus);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        focus?.gameObject.SetActive(false);
    }
    void OnDisable()
    {
        focus?.gameObject.SetActive(false);
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        ResetColors();
    }
}

