using System.Collections;
using UnityEngine;

public class SpeedPotion : Pickup
{
    [SerializeField] private float SpeedBoost;
    [SerializeField] private float Duration;
    private bool SoftDestroy =false;
    protected override void OnPickup(GameObject actor)
    {
        if (!SoftDestroy)
        {
            StartCoroutine(ApplySpeedBoost(actor.GetComponent<PlayerMovement>()));
            GetComponent<Renderer>().enabled = !GetComponent<Renderer>().enabled;
            SoftDestroy = true;
        }
    }


    private IEnumerator ApplySpeedBoost(PlayerMovement movement)
    {
        float CurrentSpeed = movement.GetCurrentSpeed();
        movement.SetCurrentSpeed(SpeedBoost+CurrentSpeed);
        yield return new WaitForSeconds(Duration);
        movement.SetCurrentSpeed(CurrentSpeed);
        Destroy(gameObject);
    }

}
