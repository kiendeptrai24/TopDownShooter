
using UnityEngine;

public class Enemy_AnimationEvent : MonoBehaviour
{
    private Enemy enemy;
    private void Start() {
        enemy = GetComponentInParent<Enemy>();
    }
    public void AnimationTrigger() => enemy.AnimationTrigger();
    public void StartManualMovement() => enemy.ActiveManualMovement(true);
    public void StopManualMovement() => enemy.ActiveManualMovement(false);
    public void StartManualRotation() => enemy.ActiveManualRotation(true);
    public void StopManualRotation() => enemy.ActiveManualRotation(false);
    public void AbilitiEven() => enemy.AbilityTrigger();

}
