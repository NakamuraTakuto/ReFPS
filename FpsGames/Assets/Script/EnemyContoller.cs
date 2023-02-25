using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContoller : ActiveBase
{
    [SerializeField] EnemyType _type = EnemyType.defalt;
    [SerializeField] bool _inPlayer = false;
    [SerializeField] AttachmentObj _attach;
    [SerializeField] SetValues _setValues;
    float _moveSpeed;
    /// <summary>Playerとの距離</summary>
    float _range;
    Rigidbody _rb;
    float _maxRange;
    float _minRange;
    int _enemyHP;
    bool _deth = false;
    GameObject _playerObj;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _maxRange = _setValues.GetMaxRange;
        _minRange = _setValues.GetMinRange;
        _moveSpeed = _setValues.GetMoveSpeed;
        _enemyHP = _setValues.GetEnemyHP;
        _playerObj = _attach.GetPlayerObj;
    }

    private void Update()
    {
        if(_deth)
        {
            switch (_type)
            {
                case EnemyType.defalt:
                    Destroy(this.gameObject);
                    break;

                case EnemyType.Boss:
                    //GameManagerからclear判定のBoolを取得して切り替える
                    Destroy(this.gameObject);
                    break;
            }
        }
        //PlayerがTrigger範囲内にいる場合のみ実行
        if (_inPlayer)
        {
            gameObject.transform.LookAt(_playerObj.transform.position);
            RangeCalculation(gameObject.transform.position,
                             _playerObj.transform.position);
            EnemyAI(_range);
        }
    }
    //Enemyの行動パターン管理
    void EnemyAI(float range)
    {
        switch (_type)
        {
            case EnemyType.defalt:
                //Playerが一定距離にいるときに攻撃
                if (range <= _maxRange * _maxRange && _minRange * _minRange <= range)
                {
                    _rb.velocity = gameObject.transform.position * _moveSpeed * 0;
                    Debug.Log($"Attack");
                }
                //Playerが遠い場合、近づく
                else if (range > _maxRange * _maxRange)
                {
                    Debug.Log($"近づく");
                    Vector3 _target = new Vector3(_playerObj.transform.position.x - gameObject.transform.position.x, 0, 
                                                    _playerObj.transform.position.z - gameObject.transform.position.z);
                    _rb.velocity = _target.normalized * _moveSpeed;
                }
                //Playerが近い場合、離れる
                else if (range < _minRange * _minRange)
                {
                    Debug.Log($"離れる");
                    _rb.velocity = _playerObj.transform.position.normalized * -1 * _moveSpeed;
                }
                break;

            case EnemyType.Boss:
                //defaltに近接攻撃を追加
                break;
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
        if (other.gameObject.tag == "Player")
        {
            _rb.velocity = gameObject.transform.position * 0;
            _inPlayer = false;
        }
    }

    public override void Active()
    {
        //Playerに撃たれた時の処理
        _enemyHP -= 2;
        _rb.AddForce(0, 0, -1);

        if (_enemyHP <= 0)
        {
            _deth = true;
        }
    }

    enum EnemyType
    {
        Boss,
        defalt
    }

    [System.Serializable]
    class SetValues
    {
        [Header("EnemyHP")]
        [SerializeField] int enemyHp = 10;
        public int GetEnemyHP
        {
            set
            { enemyHp = value; }
            get
            { return enemyHp; }
        }
        /// <summary>EnemyMoveSpeed</summary>
        [Header("Enemyの移動速度")]
        [SerializeField] private float moveSpeed = 5;
        public float GetMoveSpeed => moveSpeed;
        /// <summary>Playerとの最大の間合い</summary>
        [Header("Playerとの最大の間合い")]
        [SerializeField] private float _maxRange = 8;
        public float GetMaxRange => _maxRange;
        /// <summary>Playerとの1番近い間合い</summary>
        [Header("Playerとの間合い")]
        [SerializeField] private float _minRange = 4;
        public float GetMinRange => _minRange;
    }
    [System.Serializable]
    class AttachmentObj
    {
        [Header("PlayerObject")]
        [SerializeField] private GameObject _playerObj;
        public GameObject GetPlayerObj => _playerObj;

        /// <summary>EnemyBullets</summary>
        [Header("EnemyBullets")]
        [SerializeField] GameObject _bullets;
        public GameObject GetBullets => _bullets;

        [Header("EnemyMuzle")]
        [SerializeField] GameObject _enemyMuzle;
        public GameObject GetEnemyMuzle => _enemyMuzle;
    }
}
