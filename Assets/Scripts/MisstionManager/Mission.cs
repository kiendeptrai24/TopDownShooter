using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public abstract class Mission : ScriptableObject 
{   

    public string misssionName;
    [TextArea]
    public string missionDescription;
    public abstract void StartMisstion();
    public abstract bool MissionCompleted(); 
    public virtual void UpdateMission()
    {

    }
    
}
