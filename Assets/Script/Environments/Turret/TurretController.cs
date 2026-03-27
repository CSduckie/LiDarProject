using UnityEngine;
using System.Collections;

public class TurretController : MonoBehaviour
{
    [Header("玩家引用")]
    [SerializeField] private Transform player;
    
    [Header("发射点")]
    [SerializeField] private Transform firePoint; // 追踪线发射点
    
    [Header("追踪线设置")]
    [SerializeField] private LineRenderer trackingLine;
    [SerializeField] private Color normalColor = Color.yellow; // 正常追踪线颜色（黄色）
    [SerializeField] private Color warningColor = Color.red; // 警告颜色（红色）
    
    [Header("射击设置")]
    [SerializeField] private float shootInterval = 5f; // 射击间隔（秒）
    [SerializeField] private float warningTime = 2f; // 警告时间（秒）
    
    [Header("射线检测")]
    [SerializeField] private LayerMask obstacleLayerMask = -1; // 障碍物层遮罩
    
    private float shootTimer = 0f;
    private bool isWarning = false; // 是否处于警告状态
    [Header("下一关")]
    public string nextScene;

    [Header("状态标记")]
    public bool isEnabled = true;

    void Start()
    {
        
        // 如果没有指定玩家，尝试通过标签查找
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
        
        // 如果没有指定发射点，使用自身位置
        if (firePoint == null)
        {
            firePoint = transform;
        }
        
        // 如果没有LineRenderer，尝试获取或创建
        if (trackingLine == null)
        {
            trackingLine = GetComponent<LineRenderer>();
            if (trackingLine == null)
            {
                trackingLine = gameObject.AddComponent<LineRenderer>();
            }
        }
        
        // 初始化LineRenderer设置
        trackingLine.enabled = true;
        trackingLine.startWidth = 0.05f;
        trackingLine.endWidth = 0.05f;
        trackingLine.positionCount = 2;
        trackingLine.material = new Material(Shader.Find("Sprites/Default"));
        trackingLine.material.color = normalColor;
        trackingLine.gameObject.SetActive(false);
        // 初始化射击计时器
        shootTimer = shootInterval;
        DisableTurret();
    }

    void Update()
    {
        if (player == null) return;
        if(!isEnabled) return;
        // 持续看向玩家
        LookAtPlayer();
        
        // 更新追踪线
        UpdateTrackingLine();
        
        // 更新射击计时器
        UpdateShootTimer();
    }
    
    private void LookAtPlayer()
    {
        // 计算朝向玩家的方向（包含XYZ三轴）
        Vector3 direction = player.position - transform.position;

        if (direction.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }
    
    private void UpdateTrackingLine()
    {
        if (player == null || firePoint == null || trackingLine == null) return;
        
        Vector3 direction = player.position - firePoint.position;
        float distance = direction.magnitude;
        
        // 发射射线检测是否有阻挡（排除自身所在的Layer）
        int layerMask = obstacleLayerMask.value & ~(1 << gameObject.layer);
        
        // 使用RaycastAll来确保检测到所有碰撞体
        RaycastHit[] hits = Physics.RaycastAll(firePoint.position, direction.normalized, distance, layerMask);
    
        Vector3 lineEndPoint = player.position; // 默认指向玩家
        
        if (hits.Length > 0)
        {
            // 按照距离排序，找到第一个命中的物体
            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
            RaycastHit firstHit = hits[0];
            
            

            // 检查第一个命中的是否是玩家
            if (firstHit.collider != null && firstHit.collider.CompareTag("Player"))
            {
                // 第一个命中玩家，追踪线指向玩家
                lineEndPoint = player.position;
            }
            else
            {
                // 第一个命中其他物体，追踪线停在阻挡点
                lineEndPoint = firstHit.point;
            }
        }
        
        // 设置追踪线的起点和终点
        trackingLine.SetPosition(0, firePoint.position);
        trackingLine.SetPosition(1, lineEndPoint);
        
        // 根据警告状态更新颜色
        if (isWarning)
        {
            trackingLine.material.color = warningColor;
        }
        else
        {
            trackingLine.material.color = normalColor;
        }
    }
    
    private void UpdateShootTimer()
    {
        shootTimer -= Time.deltaTime;
        
        // 检查是否进入警告状态（射击前2秒）
        if (shootTimer <= warningTime && !isWarning)
        {
            isWarning = true;
        }
        
        // 检查是否可以射击
        if (shootTimer <= 0f)
        {
            Shoot();
            shootTimer = shootInterval;
            isWarning = false;
        }
    }
    
    private void Shoot()
    {
        if (player == null || firePoint == null) return;
        
        Vector3 direction = player.position - firePoint.position;
        float distance = direction.magnitude;
        
        // 发射射线检测是否有阻挡（排除自身所在的Layer）
        int layerMask = obstacleLayerMask.value & ~(1 << gameObject.layer);
        
        // 使用RaycastAll来确保检测到所有碰撞体
        RaycastHit[] hits = Physics.RaycastAll(firePoint.position, direction.normalized, distance, layerMask);
        
        if (hits.Length > 0)
        {
            // 按照距离排序，找到第一个命中的物体
            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
            RaycastHit firstHit = hits[0];
            
            // 检查第一个命中的是否是玩家
            if (firstHit.collider != null && firstHit.collider.CompareTag("Player"))
            {
                // 命中玩家，执行射击逻辑
                Debug.Log("Turret射击命中玩家！");
                // TODO: 在这里添加实际的射击伤害逻辑
                // 例如: player.GetComponent<PlayerControllerLidar>().TakeDamage(damage);
                SceneLoader.Instance.ReloadCurrentScene();
            }
            else
            {
                // 被其他物体阻挡
                Debug.Log("Turret射击被阻挡");
            }
        }
        else
        {
            // 没有阻挡，直接命中玩家
            Debug.Log("Turret射击命中玩家！");
            // TODO: 在这里添加实际的射击伤害逻辑
            SceneLoader.Instance.ReloadCurrentScene();
        }
    }

    public void EnableTurret()
    {
        isEnabled = true;
        trackingLine.gameObject.SetActive(true);
    }

    public void DisableTurret()
    {
        isEnabled = false;
        trackingLine.gameObject.SetActive(false);
    }
}
