using UnityEngine;

public class RectorActiveTrigger : MonoBehaviour
{
    private RectorController rectorController;

    private void Start()
    {
        rectorController = GetComponentInParent<RectorController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            rectorController.ChangeState(RectorStates.Enabled);
        }
    }
}
