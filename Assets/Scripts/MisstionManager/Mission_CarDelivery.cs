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
        carWasDelivered = false;
        MissionObject_CarToDeliver.OnCarDelivered += CarDeliveryCompleted;

        Car[] cars = FindObjectsOfType<Car>();
        foreach (Car car in cars)
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
    }   
}
