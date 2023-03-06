using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    float _time = 0;
    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;

        if (_time >= 1)
        {
            Destroy(gameObject);
        }
    }
}
