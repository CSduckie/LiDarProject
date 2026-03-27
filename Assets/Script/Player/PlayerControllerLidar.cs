using UnityEngine;

public class PlayerControllerLidar : MonoBehaviour
{
    [Header("角色基础组件以及移动")]
    public Camera cameraCamera;
    public CameraController playerCamera;
    private Transform playerCam;
    private CharacterController controller;
    [Header("移动参数")] 
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float gravity = 15f;
    private Vector3 moveDir;

    [Header("摄像机灵敏度")]
    [SerializeField] private float sensetivity = 1f;
    

    [Header("玩家状态标记")] 
    [HideInInspector] public bool canJump;

    
    [Header("Scanner")]
    public GameObject scanner;
    
    [Header("碰撞检测")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;

    [Header("交互系统")]
    [SerializeField] private GameObject interactDot;
    private KeypadKey currentKey;
    public float interactDistance = 300f;
    void Start()
    {
        //if(scanner != null)
           // scanner.SetActive(false);
        playerCamera = GetComponentInChildren<CameraController>();
        controller = GetComponent<CharacterController>();
        playerCam = GetComponentInChildren<Camera>().transform;
    }
    
    
    void Update()
    {
        //Horizontal
        transform.Rotate(0,Input.GetAxis("Mouse X")* sensetivity,0);
        //Vertical
        playerCam.Rotate(-Input.GetAxis("Mouse Y") * sensetivity,0,0);

        //防止Y轴能转一周
        if (playerCam.localRotation.eulerAngles.y != 0)
        {
            playerCam.Rotate(Input.GetAxis("Mouse Y") * sensetivity,0,0);
        }

        moveDir = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, moveDir.y, Input.GetAxis("Vertical") * moveSpeed);
        //进行世界坐标转换
        moveDir = transform.TransformDirection(moveDir);

        //Jump
        if (controller.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                moveDir.y = jumpForce;
            else
                moveDir.y = 0;
        }
        
        moveDir.y -= gravity * Time.deltaTime;
        if(controller.enabled)
            controller.Move(moveDir * Time.deltaTime);

        CheckforKeys();
        CheckforTeslaPanel();
        CheckForRector();
        CheckForTurret();
        CheckForTerminal();
    }

    private void CheckForTerminal()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance,LayerMask.GetMask("Interact Object")))
        {
            TerminalController terminal = hit.transform.GetComponent<TerminalController>();
            if(terminal != null)
            {
                interactDot.SetActive(true);
                if (Input.GetMouseButtonDown(1))
                {
                    terminal.DisableEnemy();
                }
            }
        }
        else
        {
            interactDot.SetActive(false);
        }
    }
    private void CheckForTurret()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance,LayerMask.GetMask("Interact Object")))
        {
            TurretController turret = hit.transform.GetComponent<TurretController>();
            if(turret != null)
            {
                interactDot.SetActive(true);
                if (Input.GetMouseButtonDown(1))
                {
                    turret.DisableTurret();
                    SceneLoader.Instance.LoadSceneAsync(turret.nextScene);
                }
            }
        }
        else
        {
            interactDot.SetActive(false);
        }
    }


    private void CheckForRector()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance,LayerMask.GetMask("Interact Object")))
        {
            RectorController rector = hit.transform.GetComponent<RectorController>();
            if(rector != null)
            {
                interactDot.SetActive(true);
                if (Input.GetMouseButtonDown(1))
                {
                    rector.DisableTesla();
                    Debug.Log("Disable Tesla");
                }
            }
        }
        else
        {
            interactDot.SetActive(false);
        }
    }

    private void CheckforTeslaPanel()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance,LayerMask.GetMask("Interact Object")))
        {
            TeslaPanel teslaPanel = hit.transform.GetComponent<TeslaPanel>();
            if(teslaPanel != null)
            {
                interactDot.SetActive(true);
                if (Input.GetMouseButtonDown(1))
                {
                    teslaPanel.DisableTesla();
                    Debug.Log("Disable Tesla");
                }
            }
        }
        else
        {
            interactDot.SetActive(false);
        }
    }

    private void CheckforKeys()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance,LayerMask.GetMask("Interact Object")))
        {
            KeypadKey key = hit.transform.GetComponent<KeypadKey>();

            // 瞄准目标改变时才处理高亮 & dot
            if (key != currentKey)
            {
                if (currentKey != null)
                    currentKey.SetHighlight(false);

                currentKey = key;

                if (currentKey != null)
                {
                    currentKey.SetHighlight(true);
                    interactDot.SetActive(true);
                }
                else
                {
                    interactDot.SetActive(false);
                }
            }
            // 点击逻辑（左键）
            if (Input.GetMouseButtonDown(1) && key != null)
            {
                key.SendKey();
                Debug.Log("Send Key");
            }
        }
        else
        {
            // 什么都没打到，清空状态
            if (currentKey != null)
            {
                currentKey.SetHighlight(false);
                currentKey = null;
            }

            interactDot.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * interactDistance);
    }
}
