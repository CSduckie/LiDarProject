using System;
using UnityEngine;

public class ElevaterController : MonoBehaviour
{
    [SerializeField] private float moveDownSpeed;
    [SerializeField] private float moveUpSpeed;
    [SerializeField] private float moveDistance;
    
    private Rigidbody rb;
    
    private Vector3 startPos;
    private Vector3 downTarget;
    private void Start()
    {
        CurrentState = states.Idle;
        rb = GetComponent<Rigidbody>();
        CurrentState = states.Idle;
        startPos = transform.position;
        downTarget = startPos + Vector3.down * moveDistance;

        // 如果你用 MovePosition 来“程序驱动”，建议把刚体设为 Kinematic：
        // rb.isKinematic = true;
    }
    
    private states CurrentState;
    public enum states
    {
        Idle,
        Trigger,
        Reset,
    }

    // 物理帧里移动
    private void FixedUpdate()
    {
        switch (CurrentState)
        {
            case states.Idle:
                // 保持静止（确保速度清零）
                rb.linearVelocity = Vector3.zero;
                break;

            case states.Trigger:
                TriggerBehaviour();
                break;

            case states.Reset:
                ResetBehaviour();
                break;
        }
    }

    private void TriggerBehaviour()
    {
        // 使用 MovePosition 平滑移动到目标点
        Vector3 next = Vector3.MoveTowards(transform.position, downTarget, moveDownSpeed * Time.fixedDeltaTime);
        rb.MovePosition(next);

        // 到达就切到 Reset（或根据需要改成 Idle）
        if (Vector3.Distance(next, downTarget) <= 0.001f)
        {
            rb.linearVelocity = Vector3.zero;
            CurrentState = states.Reset;
        }
    }

    private void ResetBehaviour()
    {
        Vector3 next = Vector3.MoveTowards(transform.position, startPos, moveUpSpeed * Time.fixedDeltaTime);
        rb.MovePosition(next);

        if (Vector3.Distance(next, startPos) <= 0.001f)
        {
            rb.linearVelocity = Vector3.zero;
            CurrentState = states.Idle;
        }
    }

    public void ChangeState(states state)
    {
        CurrentState = state;
    }
}
