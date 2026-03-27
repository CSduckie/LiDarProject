using UnityEngine;

public class TeslaPanel : MonoBehaviour
{
    private TeslaController teslaController;
    private void Start()
    {
        teslaController = GetComponentInParent<TeslaController>();
    }

    public void DisableTesla()
    {
        teslaController.ChangeState(TeslaStates.Deactive);
        teslaController.electricVFX.SetActive(false);
        teslaController.warnZone.SetActive(false);
        teslaController.killZone.SetActive(false);
        teslaController.killBox.SetActive(false);
    }
}
