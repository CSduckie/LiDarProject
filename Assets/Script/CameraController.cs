using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensX;
    public float sensY;
    
    public Transform orientation;
    
    public float xRotation;
    public float yRotation;
    
    //public PlayerController player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
       // if(player.isTeleporting)
           // return;
        
        float mouseX = Input.GetAxis("Mouse X") * sensX;
        float mouseY = Input.GetAxis("Mouse Y") * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

    }

    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
    public void SetCamRotation(float Xrotation,float Yrotation)
    {
        xRotation = Xrotation;
        yRotation = Yrotation;
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
