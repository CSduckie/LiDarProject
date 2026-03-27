using UnityEngine;

public class JumpPadController : MonoBehaviour
{
    [SerializeField] private float jumpPadForce;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpPadForce, ForceMode.Impulse);
            this.gameObject.SetActive(false);
        }
    }
}
