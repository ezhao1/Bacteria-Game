using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NegativeApple : Apple
{
    public override void Init(int value)
    {
        base.Init(-value);
    }
}
