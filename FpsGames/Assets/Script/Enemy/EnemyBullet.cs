using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyBullet : MonoBehaviour
{
    [SerializeField] int _moveSpeed = 3;
    [SerializeField] int _damage = 5;
    Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        _rb.velocity = gameObject.transform.forward.normalized * _moveSpeed;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.Instance.GetSetValues.GetPlayerHP -= _damage;
        }
        Destroy(gameObject);
    }
}
