using UnityEngine;
using System.Collections;
using UnityEngine.AI;
public enum EnemyState
{
    Idle,   // 待机状态
    Chase,  // 追逐状态
    Back,    // 返回状态
    Disabled, // 禁用状态
}

public class EnemyController : MonoBehaviour
{
    [Header("玩家检测")]
    [SerializeField] private float detectionRange = 15f; // 检测玩家的范围
    [SerializeField] private float moveSpeed = 3f; // 移动速度
    
    [Header("脚印设置")]
    [SerializeField] private GameObject footprintPrefab; // 脚印预制体（需要带Collider和特定Tag）
    [SerializeField] private float footprintInterval = 1f; // 脚印生成间隔（秒）
    [SerializeField] private float footprintLifetime = 10f; // 脚印存在时间（秒）
    [SerializeField] private string footprintTag = "Enemy"; // 脚印的Tag，需要在PointsData中配置
    
    [Header("地面检测")]
    [SerializeField] private LayerMask groundLayer; // 地面层
    [SerializeField] private float groundCheckDistance = 0.5f; // 地面检测距离
    
    private Transform playerTransform;
    private Vector3 startPosition; // 初始位置（敌人模型保持在这里）
    private EnemyState currentState = EnemyState.Idle; // 当前状态
    private float lastFootprintTime = 0f;
    
    public GameObject FootPrintContainer;

    [Header("寻路")]
    public NavMeshAgent navMeshAgent;
    private Vector3 desinationPosition;

    private void Start()
    {
        // 查找玩家
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        
        startPosition = transform.position;
    }
    
    private void Update()
    {
        if (playerTransform == null) return;

        if (FootPrintContainer == null)
        {
            FootPrintContainer = GameObject.Find("FootPrintVFX(Clone)");
        }

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // 状态机逻辑
        switch (currentState)
        {
            case EnemyState.Idle:
                // 检测玩家是否在范围内
                if (distanceToPlayer <= detectionRange)
                {
                    currentState = EnemyState.Chase;
                    navMeshAgent.enabled = true;
                }
                break;
            case EnemyState.Chase:
                // 向玩家方向生成脚印（敌人模型不移动）
                transform.LookAt(new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z));
                //MoveVirtualPositionTowardsPlayer();
                CreateFootprints();
                navMeshAgent.destination = playerTransform.position;
                desinationPosition = playerTransform.position;
                // 检测玩家是否离开范围
                if (distanceToPlayer > detectionRange)
                {
                    currentState = EnemyState.Back;
                }
                break;

            case EnemyState.Back:
                // 敌人模型朝向初始位置
                transform.LookAt(new Vector3(startPosition.x, transform.position.y, startPosition.z));
                //ReturnVirtualPositionToStart();
                CreateFootprints();

                //设置返程路径
                navMeshAgent.destination = startPosition;
                desinationPosition = startPosition;

                // 检查是否回到初始位置（只计算XZ平面的距离，忽略Y坐标）
                //Vector3 virtualPosXZ = new Vector3(virtualPosition.x, 0, virtualPosition.z);
                Vector3 startPosXZ = new Vector3(startPosition.x, 0, startPosition.z);
                Vector3 currentPosXZ = new Vector3(transform.position.x, 0, transform.position.z);
                float distanceToStart = Vector3.Distance(currentPosXZ, startPosXZ);

                if (distanceToStart <= 0.67f)
                {
                    transform.position = startPosition;
                    navMeshAgent.enabled = false;
                    currentState = EnemyState.Idle;
                    if (FootPrintContainer != null)
                    {
                        Destroy(FootPrintContainer);
                        FootPrintContainer = null;
                    }
                }
                Debug.Log(distanceToStart);
                break;
            case EnemyState.Disabled:
                break;
        }
        
    }

    private void CreateFootprints()
    {
        // 检查是否到了生成脚印的时间
        if (Time.time - lastFootprintTime < footprintInterval)
            return;

        // 检测地面
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.down, out RaycastHit hit, groundCheckDistance + 0.5f, groundLayer))
        {
            Vector3 footprintPosition = hit.point;

            // 创建脚印
            if (footprintPrefab != null)
            {
                GameObject footprint = Instantiate(footprintPrefab, footprintPosition, Quaternion.identity);
                footprint.tag = "Enemy";
                //Debug.Log("创建脚印: " + footprintPosition);
                // 设置脚印的生命周期
                Footprint footprintScript = footprint.GetComponent<Footprint>();
                if (footprintScript == null)
                {
                    footprintScript = footprint.AddComponent<Footprint>();
                }
                footprintScript.Initialize(footprintLifetime);
                footprint.transform.LookAt(desinationPosition);
                footprint.transform.rotation = Quaternion.Euler(0, footprint.transform.rotation.eulerAngles.y, footprint.transform.rotation.eulerAngles.z);
            }

            lastFootprintTime = Time.time;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // 绘制检测范围
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // 绘制地面检测线
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }

    public void DisableEnemy()
    {
        currentState = EnemyState.Disabled;
    }
}
