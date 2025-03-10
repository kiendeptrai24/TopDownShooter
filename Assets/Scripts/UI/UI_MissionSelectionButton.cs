using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_MissionSelectionButton : UI_Button
{
    private UI_MissionSelection missionUI;
    private TextMeshProUGUI myText;
    [SerializeField] private Mission myMission;
    private void OnValidate()
    {
        gameObject.name = "Button - Select Mission: " + myMission.misssionName;
    }
    protected override void Start() 
    {
        base.Start();
        missionUI = GetComponentInParent<UI_MissionSelection>();
        myText = GetComponentInChildren<TextMeshProUGUI>();
        myText.text = myMission.misssionName;
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        missionUI.UpdateMissionDescription(myMission.missionDescription);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        missionUI.UpdateMissionDescription("Choose a mission");
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        MissionManager.instance.SetCurrrentMission(myMission);
        UI.Instance.SwitchToInGameUI();
    }

}
