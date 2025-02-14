using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    [Header("Settings")]
    public bool friendlyFire;

    private void Awake() {
        Instance = this;
    }
}
