using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 状态基类
/// </summary>
public abstract class StateBase
{

    /// <summary>
    /// 初始化函数，旨在第一次创建时调用
    /// </summary>
    /// <param name="owner">宿主</param>
    /// <param name="stateType">代表实际枚举变量的Int</param>
    public virtual void Init(IStateMachineOwner owner)
    {

    }

    /// <summary>
    /// 反初始化,用于释放资源
    /// 销毁时也能释放资源
    /// </summary>
    public virtual void UnInit()
    {

    }

    /// <summary>
    /// 每次进入状态执行一次
    /// </summary>
    public virtual void Enter()
    {
        
    }


    /// <summary>
    /// 状态退出执行一次
    /// </summary>
    public virtual void Exit()
    {
        
    }

    public virtual void Update()
    {
        
    }

    public virtual void LateUpdate()
    {
        
    }

    public virtual void FixedUpdate()
    {
        
    }

}
