using UnityEngine;

public class KeyController : MonoBehaviour
{
    private bool canPickUp = false;
    private bool picked;
    public GameObject[] bridges;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("可以捡起钥匙");
            canPickUp = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canPickUp = false;
        }
    }

    private void Update()
    {
        if (canPickUp && !picked)
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                picked = true;
                OnPickUp();
            }
        }
    }

    private void OnPickUp()
    {
        foreach (var bridge in bridges)
        {
            bridge.SetActive(false);
        }
        Destroy(this.gameObject);
    }
}
