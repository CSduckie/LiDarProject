using UnityEngine;

public class TeslaKillZone : MonoBehaviour
{
    private TeslaController teslaController;

    private void Start()
    {
        teslaController = GetComponentInParent<TeslaController>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" )
        {
            Debug.Log("Player Killed");
            SceneLoader.Instance.ReloadCurrentScene();
        }
    }
}
