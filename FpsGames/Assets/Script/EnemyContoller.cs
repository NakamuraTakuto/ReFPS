using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContoller : MonoBehaviour
{
    /// <summary>Player�Ƃ̍ő�̊ԍ���</summary>
    [Header("Player�Ƃ̍ő�̊ԍ���")]
    [SerializeField] float _maxRange = 8;
    /// <summary>Player�Ƃ�1�ԋ߂��ԍ���</summary>
    [Header("Player�Ƃ̊ԍ���")]
    [SerializeField] float _minRange = 4;
    /// <summary>EnemyBullets</summary>
    [Header("EnemyBullets")]
    [SerializeField] GameObject _bullets;
    /// <summary>Player�Ƃ̋���</summary>
    [Header("Player�Ƃ̋���")]
    [SerializeField] float _range;
    [Header("PlayerObject")]
    [SerializeField] GameObject _playerObj;
    [Header("EnemyMuzle")]
    [SerializeField] GameObject _enemyMuzle;
    Rigidbody _rb;
    [SerializeField] bool _inPlayer = false;
    [SerializeField] EnemyType _type = EnemyType.defalt;
    [SerializeField] float _moveSpeed = 5f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //Player��Trigger�͈͓��ɂ���ꍇ�̂ݎ��s
        if (_inPlayer)
        {
            gameObject.transform.forward = _playerObj.transform.position;

            RangeCalculation(gameObject.transform.position,
                             _playerObj.transform.position);
            EnemyAI(_range);
        }
    }

    //Enemy�̍s���p�^�[���Ǘ�
    void EnemyAI(float range)
    {
        switch (_type)
        {
            case EnemyType.defalt:
                //Player����苗���ɂ���Ƃ��ɍU��
                if (range <= _maxRange * _maxRange && _minRange * _minRange <= range)
                {
                    Debug.Log($"Attack");
                }
                //Player�������ꍇ�A�߂Â�
                else if (range > _maxRange * _maxRange)
                {
                    Debug.Log($"�߂Â�");
                    //_rb.velocity = new Vector3(0, 0, 1) * _moveSpeed;
                    _rb.velocity = -_playerObj.transform.position.normalized * _moveSpeed;
                }
                //Player���߂��ꍇ�A�����
                else if (range < _minRange * _minRange)
                {
                    Debug.Log($"�����");
                    _rb.velocity = new Vector3(0, 0, -1) * _moveSpeed;
                }
                break;

            case EnemyType.Boss:
                //defalt�ɋߐڍU����ǉ�
                break;
        }
    }
     
    //Player�Ƃ̋������v�Z����
    void RangeCalculation(Vector3 me, Vector3 player)
    {
        //�����͍l�����Ȃ����ߕ��ʂŌv�Z
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
        if (other.gameObject.tag == "Player")
        {
            _rb.velocity = gameObject.transform.position * 0;
            _inPlayer = false;
        }
    }

    enum EnemyType
    {
        Boss,
        defalt
    }
}
