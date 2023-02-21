using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContoller : MonoBehaviour
{
    /// <summary>Player‚Æ‚ÌÅ‘å‚ÌŠÔ‡‚¢</summary>
    [Header("Player‚Æ‚ÌÅ‘å‚ÌŠÔ‡‚¢")]
    [SerializeField] float _maxRange = 8;
    /// <summary>Player‚Æ‚Ì1”Ô‹ß‚¢ŠÔ‡‚¢</summary>
    [Header("Player‚Æ‚ÌŠÔ‡‚¢")]
    [SerializeField] float _minRange = 4;
    /// <summary>EnemyBullets</summary>
    [Header("EnemyBullets")]
    [SerializeField] GameObject _bullets;
    /// <summary>Player‚Æ‚Ì‹——£</summary>
    [Header("Player‚Æ‚Ì‹——£")]
    [SerializeField] float _range;
    [Header("PlayerObject")]
    [SerializeField] GameObject _playerObj;
    [Header("EnemyMuzle")]
    [SerializeField] GameObject _enemyMuzle;
    Rigidbody _rb;
    [SerializeField] bool _inPlayer = false;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_inPlayer)
        {
            RangeCalculation(gameObject.transform.position,
                             _playerObj.transform.position);
            EnemyAI(_range);
        }
    }

    void EnemyAI(float range)
    {
        if (range <= _maxRange * _maxRange && _minRange * _minRange <= range)
        {
            Debug.Log("Attack");
        }
        else if (range > _maxRange * _maxRange)
        {
            Debug.Log($"{range}, ‹ß‚Ã‚­");
        }
        else if (range < _minRange * _minRange)
        {
            Debug.Log($"{range}, —£‚ê‚é");
        }
    }
     
    void RangeCalculation(Vector3 me, Vector3 player)
    {
        float x = me.x - player.x;
        float z = me.z - player.z;
        _range = x * x  + z * z;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _inPlayer = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player") ;
    }
}
