using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gimmick : ActiveBase
{
    public override void Active()
    {
        if (GameManager.Instance.GimmickActive)
        {
            GameManager.Instance.GimmickJudge(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
