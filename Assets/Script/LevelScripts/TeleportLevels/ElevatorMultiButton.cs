using UnityEngine;

public class ElevatorMultiButton : MonoBehaviour
{
    public bool isTriggered = false;

    private void OnTriggerStay(Collider other)
    {
        isTriggered = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isTriggered = false;
    }
}
