using UnityEngine;

public class ExitPointController : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneLoader.Instance.LoadSceneAsync(sceneToLoad);
        }
    }

}
