using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New weapon data", menuName = "Enemy Data/Enemy Weapon Data", order = 0)]
public class Enemy_MeleeWeaponData : ScriptableObject {
    public List<AttackData_EnemyMelee> attackData;
    public float turnSpeed;
}

