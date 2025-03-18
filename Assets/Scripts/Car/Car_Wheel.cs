using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public enum AxelType {Front, Back}
[RequireComponent(typeof(WheelCollider))]
public class Car_Wheel : MonoBehaviour
{
    public AxelType axelType;
    public WheelCollider cd {get; private set;}
    public GameObject model {get; private set;}
    private float defaultSideStiffness;
    private void Start() {
        cd = GetComponent<WheelCollider>();
        model = GetComponentInChildren<MeshRenderer>().gameObject;
        defaultSideStiffness = cd.sidewaysFriction.stiffness;
    }
    public void RestoreDefaultStiffness()
    {
        WheelFrictionCurve sidewayFriction = cd.sidewaysFriction;
        sidewayFriction.stiffness = defaultSideStiffness;
        cd.sidewaysFriction = sidewayFriction;
    }
}
