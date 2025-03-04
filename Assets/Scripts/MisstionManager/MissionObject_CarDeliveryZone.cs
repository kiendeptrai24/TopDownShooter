using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionObject_CarDeliveryZone : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Car>() != null)
        {
            Car car = other.GetComponent<Car>();
            
            car?.GetComponent<MissionObject_CarToDeliver>().InvokeOnCarDelivery();
        }
    }
}
