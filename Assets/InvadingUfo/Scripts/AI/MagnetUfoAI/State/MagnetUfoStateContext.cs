using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetUfoStateContext
{
    public MagnetUfoBaseState State;

    public void Execute()
    {
        State.Execute();
    }
}
