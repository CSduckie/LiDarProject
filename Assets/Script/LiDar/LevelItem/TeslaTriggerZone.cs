using UnityEngine;

public class TeslaTriggerZone : MonoBehaviour
{
    private TeslaController teslaController;
    private bool isTriggered = false;
    private void Start()
    {
        teslaController = GetComponentInParent<TeslaController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !isTriggered)
        {
            if(teslaController.currentState == TeslaStates.Deactive) return;
            teslaController.ChangeState(TeslaStates.Active);
            // 激活电击特效
            teslaController.electricVFX.SetActive(true);
            // 激活警告圈
            teslaController.warnZone.SetActive(true);
            // 激活击杀圈
            teslaController.killZone.SetActive(true);
            // 激活击杀盒子
            teslaController.killBox.SetActive(true);
            isTriggered = true;
        }
    }
}
