using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "New CarDelivery Mission", menuName = "Missions/Car Delivery - Mission")]
public class Mission_CarDelivery : Mission
{
    private bool carWasDelivered;
    public override void StartMisstion()
    {
        FindObjectOfType<MissionObject_CarDeliveryZone>(true).gameObject.SetActive(true);
        UpdateMissionUI();

        carWasDelivered = false;
        MissionObject_CarToDeliver.OnCarDelivered += CarDeliveryCompleted;
        Car_Controller[] cars = FindObjectsOfType<Car_Controller>();
        
        foreach (Car_Controller car in cars)
        {
            car.AddComponent<MissionObject_CarToDeliver>();            
        }
    }
    public override bool MissionCompleted()
    {
        return carWasDelivered;
    }
    private void CarDeliveryCompleted()
    {
        carWasDelivered = true;
        MissionObject_CarToDeliver.OnCarDelivered -= CarDeliveryCompleted;
        UI.Instance.inGameUI.UpdateMissionInfo("get to the evacuation point");
    }
    private void UpdateMissionUI()
    {
        string missionText = "Deliver it to the evacuation point";    
        string missionDetails = "Find a functional vehicel";
        UI.Instance.inGameUI.UpdateMissionInfo(missionText, missionDetails);
    }
}
