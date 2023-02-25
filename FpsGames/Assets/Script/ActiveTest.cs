using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveTest : ActiveBase
{
    public override void Active()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.red;
    }
}
