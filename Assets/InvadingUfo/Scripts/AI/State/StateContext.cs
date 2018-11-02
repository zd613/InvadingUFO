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
    public void ChangeState(BaseState state, float startDelaySec = 0, float endDelaySec = 0)
    {
        Func<float, float, Task> delayFunc = async (start, end) =>
        {
            canExecute = false;
            await Task.Delay((int)(start * 1000));
            State = state;
            await Task.Delay((int)(end * 1000));
            canExecute = true;
        };

        delayFunc(startDelaySec, endDelaySec);

    }
}
