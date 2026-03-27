using UnityEngine;

public class TurretActiveBox : MonoBehaviour
{
    private TurretController turretController;
    private void Start()
    {
        turretController = GetComponentInParent<TurretController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            turretController.EnableTurret();
            Destroy(gameObject);
        }
    }
}
