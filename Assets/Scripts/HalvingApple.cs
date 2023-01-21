using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalvingApple : Apple
{
    public override void OnClear()
    {
        if (this.Value == 1)
        {
            AnimateDelete();
        }
        else
        {
            this.Value -= this.Value/2;
            BeginShake();
        }
    }
    public override void OnMove()
    {

    }
}
