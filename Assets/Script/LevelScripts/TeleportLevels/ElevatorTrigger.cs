using UnityEngine;

public class ElevatorTrigger : MonoBehaviour
{
    public ElevaterController[] ElevaterControllers;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (ElevaterController controller in ElevaterControllers)
            {
                controller.ChangeState(ElevaterController.states.Trigger);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (ElevaterController controller in ElevaterControllers)
            {
                controller.ChangeState(ElevaterController.states.Reset);
            }
        }
    }
}
