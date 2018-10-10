using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetUfoStateContext
{
    public MagnetUfoBaseState State;

    public void Execute()
    {
        //Debug.Log("execute");
        State.Execute();
    }

    public override string ToString()
    {
        return State.ToString();
    }
}
