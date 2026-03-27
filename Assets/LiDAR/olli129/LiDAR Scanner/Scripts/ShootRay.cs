using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LiDAR
{
    public class ShootRay : MonoBehaviour
    {
        [Header("General Settings")]
        public Camera cam;
        [SerializeField] Transform gunPoint;
        public bool switchFireMode;

        [Header("Render")]
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
            StartCoroutine(ShootInterval());
            audioSource.clip = randomLaser;
            sprayAngle = 200f;

            dotDistance = -300f;
        }

        // Update is called once per frame
        void Update()
        {
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

        void LaserAudio()
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
}

