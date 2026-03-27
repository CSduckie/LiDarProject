using UnityEngine;
using System.Collections;
public class PlayerController : MonoBehaviour
{
    [Header("移动参数")] 
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float gravity = 15f;
    private Vector3 moveDir;
    
    [Header("摄像机灵敏度")]
    [SerializeField] private float sensetivity = 1f;

    private Transform playerCam;
    private CharacterController controller;


    [Header("LiDAR")]
    public Camera cam;
    [SerializeField] Transform gunPoint;
    public bool switchFireMode;

    [SerializeField] LineRenderer lineRenderer;

    [Header("Particles")]
    public ParticleSystem wallParticleSystem;
    public ParticleSystem exitParticleSystem;
    public ParticleSystem enemyParticleSystem;

    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip randomLaser;
    [SerializeField] AudioClip lineLaser;

    Vector2 aimPosition;
    private bool startedFiring;
    private float sprayAngle;
    private float dotDistance;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCam = GetComponentInChildren<Camera>().transform;
        //Hide mouse Cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        StartCoroutine(ShootInterval());
        audioSource.clip = randomLaser;
        sprayAngle = 200f;
        dotDistance = -300f;
    }

    private void Update()
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



         if (Input.GetKey(KeyCode.Mouse0))
            {
                startedFiring = true;
            }
            else
            {
                startedFiring = false;
                lineRenderer.enabled = false;
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                switchFireMode = !switchFireMode;

                if (switchFireMode)
                {
                    audioSource.clip = lineLaser;
                }
                else
                {
                    audioSource.clip = randomLaser;
                }
            }

            LaserAudio();

            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                sprayAngle += 8;
                sprayAngle = Mathf.Clamp(sprayAngle, 10, 300);
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                sprayAngle -= 8;
                sprayAngle = Mathf.Clamp(sprayAngle, 10, 300);
            }
    }


    private void LaserAudio()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Play Laser Sound
            audioSource.Play();
        }

        if (Input.GetMouseButtonUp(0))
        {
            // Stop Laser Sound
            audioSource.Stop();
        }
    }

        void ParticleShooting()
        {
            if (!switchFireMode)
            {
                float xCoord = Input.mousePosition.x + Random.Range(-sprayAngle, sprayAngle);
                float yCoord = Input.mousePosition.y + Random.Range(-sprayAngle, sprayAngle);
                aimPosition = new Vector2(xCoord, yCoord);
            }
            else
            {
                float xCoord = Input.mousePosition.x + Random.Range(-sprayAngle, sprayAngle);
                float yCoord = Input.mousePosition.y;
                aimPosition = new Vector2(xCoord, yCoord);
            }

            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(aimPosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag != "Enemy" && hit.transform.gameObject.tag != "Exit")
                {
                    lineRenderer.enabled = true;
                    lineRenderer.SetPosition(0, gunPoint.position);
                    lineRenderer.SetPosition(1, hit.point);
                    //Instantiate(hitPointObject, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal), laserParentTransform);
                    var emitParams = new ParticleSystem.EmitParams();
                    emitParams.position = hit.point;
                    emitParams.velocity = new Vector3(0.0f, 0.0f, 0.0f);
                    wallParticleSystem.Emit(emitParams, 1);
                }

                if (hit.transform.gameObject.tag == "Exit")
                {
                    lineRenderer.enabled = true;
                    lineRenderer.SetPosition(0, gunPoint.position);
                    lineRenderer.SetPosition(1, hit.point);
                    //Instantiate(hitPointObject, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal), laserParentTransform);
                    var emitParams = new ParticleSystem.EmitParams();
                    emitParams.position = hit.point;
                    emitParams.velocity = new Vector3(0.0f, 0.0f, 0.0f);
                    exitParticleSystem.Emit(emitParams, 1);
                }

                if (hit.transform.gameObject.tag == "Enemy")
                {
                    lineRenderer.enabled = true;
                    lineRenderer.SetPosition(0, gunPoint.position);
                    lineRenderer.SetPosition(1, hit.point);
                    //Instantiate(hitPointObject, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal), laserParentTransform);
                    var emitParams = new ParticleSystem.EmitParams();
                    emitParams.position = hit.point;
                    emitParams.velocity = new Vector3(0.0f, 0.0f, 0.0f);
                    enemyParticleSystem.Emit(emitParams, 1);
                }
            }
        }

        private IEnumerator ShootInterval()
        {
            while (true)
            {
                yield return null;
                if (startedFiring)
                {
                    //RayShooting();
                    for (int i = 0; i < 30; i++)
                    {
                        //ParticleShooting();
                        ParticleShooting();
                        if (dotDistance >= 300)
                        {
                            dotDistance = -300f;
                        }
                    }
                }
            }
        }


}
