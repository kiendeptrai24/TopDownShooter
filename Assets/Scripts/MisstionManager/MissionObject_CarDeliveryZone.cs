using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionObject_CarDeliveryZone : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Car_Controller>() != null)
        {
            Car_Controller car = other.GetComponent<Car_Controller>();
            
            car?.GetComponent<MissionObject_CarToDeliver>().InvokeOnCarDelivery();
        }
    }
}
