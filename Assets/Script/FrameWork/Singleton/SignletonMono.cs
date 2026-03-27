using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignletonMono<T> : MonoBehaviour where T: SignletonMono<T>
{
    public static T instance;
    protected virtual void Awake()
    {
        if(instance == null)
        {
            instance = (T)this;
        }
    }
}
