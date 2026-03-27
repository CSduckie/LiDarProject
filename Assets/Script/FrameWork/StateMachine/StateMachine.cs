using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IStateMachineOwner{}
public class StateMachine
{
    private IStateMachineOwner owner;
    private Dictionary<Type, StateBase> stateDic = new Dictionary<Type, StateBase>();

    public Type CurrentStateType{get => currentState != null ? currentState.GetType() : null;}
    public bool hasState {get => currentState != null;}
    private StateBase currentState;
    public StateBase CurrentState{get => currentState;}

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="owner"></param>

    public void Init(IStateMachineOwner owner)
    {
        this.owner = owner;
    }
    
    
    
    //用于检测玩家是否要修改状态

    /// <summary>
    ///切换状态 
    /// </summary>
    /// <typeparam name="T">具体要切换到的状态类型</typeparam>
    /// <param name="reCurrstate">如果状态没变，是否需要刷新状态</param>
    /// <returns></returns>
    public bool ChangeState<T>(bool reCurrstate = false) where T:StateBase,new()
    {
        //状态一致，并且不需要刷新状态，则不需要进行切换
        if(hasState && CurrentStateType == typeof(T) && !reCurrstate) return false;

        //退出当前状态
        if(currentState != null)
        {
            currentState.Exit();
            MonoManager.instance.RemoveUpdateListener(currentState.Update);
            MonoManager.instance.RemoveLateUpdateListener(currentState.LateUpdate);
            MonoManager.instance.RemoveFixedUpdateListener(currentState.FixedUpdate);
        }

        //进入新状态,执行进入命令后，注册逻辑方法
        currentState = GetState<T>();
        currentState.Enter();
        MonoManager.instance.AddUpdateListener(currentState.Update);
        MonoManager.instance.AddLateUpdateListener(currentState.LateUpdate);
        MonoManager.instance.AddFixedUpdateListener(currentState.FixedUpdate);
        return false;
    }

    private StateBase GetState<T>() where T:StateBase,new()
    {
        Type type = typeof(T);
        //缓存字典中如果不存在，则人为创建添加
        if(!stateDic.TryGetValue(typeof(T),out StateBase state))
        {
            state = new T();
            state.Init(owner);
            stateDic.Add(type,state);
        }

        return state;
    }
    
    /// <summary>
    /// 停止工作释放资源
    /// </summary>
    public void Stop()
    {
        currentState.Exit();
        MonoManager.instance.RemoveUpdateListener(currentState.Update);
        MonoManager.instance.RemoveLateUpdateListener(currentState.LateUpdate);
        MonoManager.instance.RemoveFixedUpdateListener(currentState.FixedUpdate);
        currentState = null;

        foreach(var item in stateDic.Values)
        {
            item.UnInit();
        }
        stateDic.Clear();
    }
}
