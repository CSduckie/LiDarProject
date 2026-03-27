using UnityEngine;

public class KillZoneController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneLoader.Instance.ReloadCurrentScene();
        }
    }
}
