using UnityEngine;

public class TerminalController : MonoBehaviour
{
    public EnemyController enemyController;

    public void DisableEnemy()
    {
        enemyController.DisableEnemy();
    }
}
