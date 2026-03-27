using UnityEngine;

public class ElevatorMultiTrigger : MonoBehaviour
{
    public ElevatorMultiButton[] buttons;
    
    [SerializeField]
    private ElevaterController elevater;


    private void Update()
    {
        foreach (var button in buttons)
        {
            if (!button.isTriggered)
            {
                elevater.ChangeState(ElevaterController.states.Reset);
                return;
            }
        }
        elevater.ChangeState(ElevaterController.states.Trigger);
    }
}
