using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class StateContext
{
    public BaseState State;
    bool canExecute = true;

    public void Execute()
    {
        if (!canExecute)
            return;
        //Debug.Log("execute");
        State.Execute();
    }

    public override string ToString()
    {
        return State.ToString();
    }

    //state変更後に待機
    public void ChangeState(BaseState state, float startDelaySec = 0)
    {
        State = state;
        canExecute = false;
        Func<Task> func = async () => await Task.Delay((int)(startDelaySec * 1000));
        func();
        canExecute = true;
    }
}
