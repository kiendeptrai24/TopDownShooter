using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Interaction : Interactable
{
    private Car_HealthController carHealthController;
    private Car_Controller car;
    private Transform player;
    private float defautPlayerScale;
    [Header("Exit details")]
    [SerializeField] private float exitCheckRadius;
    [SerializeField] private Transform[] exitPoints;
    [SerializeField] private LayerMask whatToIngoreForExit;
    private void Start() {
        car = GetComponent<Car_Controller>();
        carHealthController = GetComponent<Car_HealthController>();
        player = GameManager.Instance.player.transform;
        DisablePoint();
    }
    private void DisablePoint()
    {
        foreach (var point in exitPoints)
        {
            point.GetComponent<MeshRenderer>().enabled = false;
            point.GetComponent<SphereCollider>().enabled = false;
        }
    }
    public override void Interaction()
    {
        base.Interaction();
        Debug.Log("Got into the car");
        GetIntoTheCar();
    }
    private void GetIntoTheCar()
    {
        ControlsManager.Instance.SwitchToCarControls();
        carHealthController.UpdateCarHealthUI();
        car.ActivateCar(true);
        defautPlayerScale = player.localScale.x;
        player.localScale = new Vector3(0.01f,0.01f,0.01f);
        player.transform.parent = transform;
        player.transform.localPosition = Vector3.up / 2;
        CameraManager.Instance.ChangeCameraTarget(transform,20,.5f);
    }
    public void GetOutOfTheCar()
    {
        if(car.carActive == false)
            return;
        car.ActivateCar(false);
        player.parent = null;
        player.position = GetExitPoint();
        player.transform.localScale = new Vector3(defautPlayerScale, defautPlayerScale, defautPlayerScale);
        ControlsManager.Instance.SwitchToCharactorControls();
        Player_Aim aim = GameManager.Instance.player.aim;
        CameraManager.Instance.ChangeCameraTarget(aim.GetAimCameraTarget());
    }
    private Vector3 GetExitPoint()
    {
        foreach (var exitPoint in exitPoints)
        {
            if(IsExitClear(exitPoint.position))
                return exitPoint.position;
        }
        return exitPoints[0].position;
    }
    private bool IsExitClear(Vector3 point)
    {
        Collider[] colliders = Physics.OverlapSphere(point,exitCheckRadius, ~whatToIngoreForExit);
        return colliders.Length == 0;
    }

}
