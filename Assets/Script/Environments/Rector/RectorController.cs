using Unity.VisualScripting;
using UnityEngine;

public class RectorController : MonoBehaviour
{
    private RectorStates currentState;
    [SerializeField]private PlayerControllerLidar player;
    
    [Header("射线检测参数")]
    [SerializeField] private float raycastInterval = 1f; // 射线检测间隔（秒）
    [SerializeField] private LayerMask obstacleLayerMask = -1; // 障碍物层遮罩（默认检测所有层）
    private float raycastTimer = 0f;
    
    [Header("射线可视化")]
    [SerializeField] private Color rayColor = Color.red; // 射线颜色（命中玩家）
    [SerializeField] private Color blockedRayColor = Color.yellow; // 射线颜色（被阻挡）
    private Vector3 rayStart;
    private Vector3 rayEnd;
    private Color currentRayColor;

    [Header("VFX")]
    [SerializeField] private GameObject vfx;
    [SerializeField] private Transform vfxTransform;

    [Header("场景加载")]
    [SerializeField] private string sceneToLoad;

    void Start()
    {
        currentState = RectorStates.Disabled;
    }


    void Update()
    {
        switch(currentState)
        {
            case RectorStates.Disabled:
                OndisabledUpdate();
                break;
            case RectorStates.Enabled:
                OnEnableUpdate();
                break;
        }
    }

    private void OndisabledUpdate()
    {

    }


    public void DisableTesla()
    {
        currentState = RectorStates.Disabled;
        SceneLoader.Instance.LoadSceneAsync(sceneToLoad);
    }

    private void OnEnableUpdate()
    {
        //周期性向玩家发射射线，如果没有阻挡，则击杀玩家，如果有则无事发生
        raycastTimer += Time.deltaTime;
        
        if (raycastTimer >= raycastInterval)
        {
            raycastTimer = 0f;

            //生成特效
            var newVfx = Instantiate(vfx, vfxTransform.position, Quaternion.identity);
            newVfx.transform.SetParent(this.transform);
            newVfx.transform.localScale = new Vector3(2,2,2);
            Destroy(newVfx, 5f);

            if (player != null && player.gameObject != null)
            {
                Vector3 direction = player.transform.position - transform.position;
                float distance = direction.magnitude;
                
                // 发射射线检测是   否有阻挡（排除自身所在的Layer）
                int layerMask = obstacleLayerMask.value & ~(1 << gameObject.layer);
                
                // 使用RaycastAll来确保检测到所有碰撞体
                RaycastHit[] hits = Physics.RaycastAll(transform.position, direction.normalized, distance, layerMask);
                
                if (hits.Length > 0)
                {
                    // 按照距离排序，找到第一个命中的物体
                    System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
                    RaycastHit hit = hits[0];
                    
                    // 存储射线信息用于Gizmos绘制
                    rayStart = transform.position;
                    rayEnd = hit.point;
                    
                    // 检查射线是否直接命中玩家（需要第一个命中的就是玩家才击杀）
                    if (hit.collider != null && hit.collider.CompareTag("Player"))
                    {
                        currentRayColor = rayColor;
                        // 没有阻挡，击杀玩家
                        Debug.Log("Player Killed by Rector");
                        // TODO: 调用玩家死亡方法，例如: player.Kill(); 或 player.Die();
                        SceneLoader.Instance.ReloadCurrentScene();
                    }
                    else
                    {
                        currentRayColor = blockedRayColor;
                    }
                }
                else
                {
                    // 射线未命中任何物体
                    rayStart = transform.position;
                    rayEnd = transform.position + direction;
                    currentRayColor = rayColor;
                    Debug.LogWarning($"射线未命中任何物体。玩家Layer: {LayerMask.LayerToName(player.gameObject.layer)}, 检测LayerMask: {layerMask}, 距离: {distance:F2}");
                }
            }
        }
    }
    
    public void ChangeState(RectorStates newState)
    {
        if(currentState == newState) return;
        currentState = newState;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = currentRayColor;
        Gizmos.DrawLine(rayStart, rayEnd);
    }
}

public enum RectorStates
{
    Disabled,
    Enabled
}