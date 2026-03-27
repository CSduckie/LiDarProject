using UnityEngine;
using VHierarchy.Libs;

public class TeslaController : MonoBehaviour
{
    public TeslaStates currentState{get; private set;}
    public GameObject electricVFX;

    [Header("击杀判定圈")]
    //颜色圈
    public GameObject warnZone;
    public GameObject killZone;
    //击杀判定圈
    public GameObject killBox;

    private void Start()
    {
        currentState = TeslaStates.Idle;
        electricVFX.SetActive(false);
        warnZone.SetActive(false);
        killZone.SetActive(false);
        killBox.SetActive(false);
    }

    private void Update()
    {
        switch (currentState)
        {
            case TeslaStates.Idle:
                break;
            case TeslaStates.Active:
                break;
            case TeslaStates.Deactive:
                break;
        }
    }

    public void ChangeState(TeslaStates newState)
    {
        if (currentState == newState) return;
        currentState = newState;
    }
}

public enum TeslaStates
{
    Idle,
    Active,
    Deactive,
}
