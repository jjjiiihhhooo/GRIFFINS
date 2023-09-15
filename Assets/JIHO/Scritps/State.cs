using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State<T>
{
    public virtual void StateChange(T PlayerController)
    { 
    
    }

    public virtual void StateEnter(T PlayerController)
    {

    }

    public virtual void StateUpdate(T PlayerController)
    {

    }

    public virtual void StateExit(T PlayerController)
    {

    }
}
