using UnityEngine;
using UnityEngine.UI;

public class UI_OnHover_Focus : UI_TransparentOnHover
{
    [SerializeField] private Image focus;
    protected override void Start()
    {
        base.Start();
        focus.gameObject.SetActive(false);
    }
    public override void OnPointerEnter(UnityEngine.EventSystems.PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        focus.gameObject.SetActive(true);
        focus.color = new Color(1, 1, 1, 0.9f);
    }
    public override void OnPointerExit(UnityEngine.EventSystems.PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        focus.gameObject.SetActive(false);
    }
}
