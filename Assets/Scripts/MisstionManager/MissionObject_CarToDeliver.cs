using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionObject_CarToDeliver : MonoBehaviour
{
    public static event System.Action OnCarDelivered;
    public void InvokeOnCarDelivery() => OnCarDelivered?.Invoke();
}
