using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSoud : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.Log("Ypbareta");
        GetComponent<AudioSource>().Play();
    }
}
