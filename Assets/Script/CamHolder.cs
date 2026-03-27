using UnityEngine;

public class CamHolder : MonoBehaviour
{
    public Transform camPos;

    private void Update()
    {
        this.transform.position = camPos.position;
    }
}
