using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyContoller : ActiveBase
{
    [SerializeField] EnemyType _type = EnemyType.defalt;
    [SerializeField] bool _inPlayer = false;
    [SerializeField] AttachmentObj _attach;
    [SerializeField] SetValues _setValues;
    float _moveSpeed;
    /// <summary>Player�Ƃ̋���</summary>
    float _range;
    Rigidbody _rb;
    float _maxRange;
    float _minRange;
    int _enemyHP;
    float _ct;
    float _moveCT;
    bool _deth = false;
    GameObject _playerObj;
    GameObject _bullet;
    GameObject _muzzle;
    GameObject _effect;
    Slider _hpSlider;
    float _time;
    float _moveTime;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _maxRange = _setValues.GetMaxRange;
        _minRange = _setValues.GetMinRange;
        _moveSpeed = _setValues.GetMoveSpeed;
        _enemyHP = _setValues.GetEnemyHP;
        _ct = _setValues.GetCoolTime;
        _moveCT = _setValues.GetMoveCT;
        _playerObj = _attach.GetPlayerObj;
        _bullet = _attach.GetBullets;
        _muzzle = _attach.GetEnemyMuzle;
        _hpSlider = _attach.GetHpSlider;
        _effect = _attach.GetEffect;
        _hpSlider.value = _enemyHP;
        _hpSlider.maxValue = _setValues.GetEnemyHP;
    }

    private void Update()
    {
        _time += Time.deltaTime;
        _hpSlider?.transform.LookAt(_playerObj.transform.position);

        if (_deth)
        {
            switch (_type)
            {
                case EnemyType.defalt:
                    Instantiate(_effect, gameObject.transform.position, transform.rotation);
                    Destroy(this.gameObject);
                    break;

                case EnemyType.Boss:
                    //GameManager����clear�����Bool���擾���Đ؂�ւ���
                    Instantiate(_effect, gameObject.transform.position, transform.rotation);
                    GameManager.Instance.GameClear = true;
                    Destroy(this.gameObject);
                    break;
            }
        }
        //Player��Trigger�͈͓��ɂ���ꍇ�̂ݎ��s
        if (_inPlayer)
        {
            gameObject.transform.LookAt(_playerObj.transform.position);
            RangeCalculation(gameObject.transform.position,
                             _playerObj.transform.position);
            EnemyAI(_range);
            _moveTime += Time.deltaTime;
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
                    _rb.velocity = gameObject.transform.position * _moveSpeed * 0;
                    
                    if (_time >= _ct)
                    {
                        Instantiate(_bullet, _muzzle.transform.position, transform.rotation);
                        _bullet.transform.forward = _playerObj.transform.position;
                        _time = 0;
                    }
                }
                //Player�������ꍇ�A�߂Â�
                else if (range > _maxRange * _maxRange)
                {
                    if (_moveTime >= _moveCT)
                    {
                        _moveTime = 0;
                        Vector3 _target = new Vector3(_playerObj.transform.position.x - gameObject.transform.position.x, 0,
                                                        _playerObj.transform.position.z - gameObject.transform.position.z);
                        _rb.velocity = _target.normalized * _moveSpeed;
                    }
                }
                //Player���߂��ꍇ�A�����
                else if (range < _minRange * _minRange)
                {
                    if (_moveTime >= _moveCT)
                    {
                        _moveTime = 0;
                        _rb.velocity =
                            new Vector3(_playerObj.transform.position.x * -1, 0, _playerObj.transform.position.z * -1).normalized
                            * _moveSpeed;
                    }
                }
                break;

            case EnemyType.Boss:
                //Player����苗���ɂ���Ƃ��ɍU��
                if (range <= _maxRange * _maxRange && _minRange * _minRange <= range)
                {
                    _rb.velocity = gameObject.transform.position * _moveSpeed * 0;
                    _muzzle.transform.LookAt(_playerObj.transform.position);

                    if (_time >= _ct)
                    {
                        Instantiate(_bullet, _muzzle.transform.position, transform.rotation);
                        _bullet.transform.forward = _playerObj.transform.position;
                        _time = 0;
                    }
                }
                //Player�������ꍇ�A�߂Â�
                else if (range > _maxRange * _maxRange)
                {
                    if (_moveTime >= _moveCT)
                    {
                        _moveTime = 0;
                        Vector3 _target = new Vector3(_playerObj.transform.position.x - gameObject.transform.position.x, 0,
                                                        _playerObj.transform.position.z - gameObject.transform.position.z);
                        _rb.velocity = _target.normalized * _moveSpeed;
                    }
                }
                //Player���߂��ꍇ�A�ߐڍU��
                else if (range < _minRange * _minRange)
                {
                    _rb.velocity = gameObject.transform.position * _moveSpeed * 0;

                    if (_time >= _ct)
                    {
                        Debug.Log("�ߐڍU��");
                        var pos = new Vector3(_playerObj.transform.position.x,
                                             _playerObj.transform.position.y + 5,
                                             _playerObj.transform.position.z);
                        Instantiate(_bullet, pos, transform.rotation);
                        _bullet.transform.forward = _playerObj.transform.position;
                        _time = 0;
                    }
                }
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

    public override void Active()
    {
        //Player�Ɍ����ꂽ���̏����B�]�T������΃m�b�N�o�b�N����
        _enemyHP -= 2;
        _setValues.GetEnemyHP = _enemyHP;
        _hpSlider.value = _enemyHP;
        Debug.Log("Hit!!!");

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
        [Header("Enemy�̈ړ����x")]
        [SerializeField] private float moveSpeed = 5;
        public float GetMoveSpeed => moveSpeed;
        /// <summary>Player�Ƃ̍ő�̊ԍ���</summary>
        [Header("Player�Ƃ̍ő�̊ԍ���")]
        [SerializeField] private float _maxRange = 8;
        public float GetMaxRange => _maxRange;
        /// <summary>Player�Ƃ�1�ԋ߂��ԍ���</summary>
        [Header("Player�Ƃ̊ԍ���")]
        [SerializeField] private float _minRange = 4;
        public float GetMinRange => _minRange;
        [Header("�U����CoolTime")]
        [SerializeField] float coolTime = 5;
        public float GetCoolTime => coolTime;
        [Header("MoveCoolTime")]
        [SerializeField] float moveCoolTime = 1;
        public float GetMoveCT => moveCoolTime;
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

        [Header("HpSlider")]
        [SerializeField] Slider hpSlider;
        public Slider GetHpSlider => hpSlider;
        [Header("�G�t�F�N�g")]
        [SerializeField] GameObject effect;
        public GameObject GetEffect => effect;
    }
}
