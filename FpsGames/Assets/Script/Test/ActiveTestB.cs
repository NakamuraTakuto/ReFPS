using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveTestB : ActiveBase
{
    public override void Active()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.blue;
    }
}
