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
    public GameObject model;
    private float defaultSideStiffness;
    private void Awake() {
        cd = GetComponent<WheelCollider>();
        if(model == null)
            model = GetComponentInChildren<MeshRenderer>().gameObject;
    }
    public void SetDefaulStiffnesst(float newValue)
    {
        defaultSideStiffness = newValue;
        RestoreDefaultStiffness();
    }
    public void RestoreDefaultStiffness()
    {
        WheelFrictionCurve sidewayFriction = cd.sidewaysFriction;
        sidewayFriction.stiffness = defaultSideStiffness;
        cd.sidewaysFriction = sidewayFriction;
    }
}
