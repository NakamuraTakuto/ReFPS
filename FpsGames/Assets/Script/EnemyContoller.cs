using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContoller : MonoBehaviour
{
    /// <summary>Playerとの最大の間合い</summary>
    [Header("Playerとの最大の間合い")]
    [SerializeField] float _maxRange = 8;
    /// <summary>Playerとの1番近い間合い</summary>
    [Header("Playerとの間合い")]
    [SerializeField] float _minRange = 4;
    /// <summary>EnemyBullets</summary>
    [Header("EnemyBullets")]
    [SerializeField] GameObject _bullets;
    /// <summary>Playerとの距離</summary>
    [Header("Playerとの距離")]
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
        //PlayerがTrigger範囲内にいる場合のみ実行
        if (_inPlayer)
        {
            RangeCalculation(gameObject.transform.position,
                             _playerObj.transform.position);
            EnemyAI(_range);
        }
    }

    //Enemyの行動パターン管理
    void EnemyAI(float range)
    {
        //Playerが一定距離にいるときに攻撃
        if (range <= _maxRange * _maxRange && _minRange * _minRange <= range)
        {
            Debug.Log("Attack");
        }//Playerが遠い場合、近づく
        else if (range > _maxRange * _maxRange)
        {
            Debug.Log($"{range}, 近づく");
        }//Playerが近い場合、離れる
        else if (range < _minRange * _minRange)
        {
            Debug.Log($"{range}, 離れる");
        }
    }
     
    //Playerとの距離を計算する
    void RangeCalculation(Vector3 me, Vector3 player)
    {
        //高さは考慮しないため平面で計算
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
