using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] SetValues _setValues;
    public SetValues GetSetValues
    {
        set { _setValues = value; }
        get { return _setValues; }
    }
    [SerializeField] List<GameObject> _gunSound;
    public List<GameObject> GunSound => _gunSound;
    [SerializeField] AttachmentObj _attach;
    public static GameManager Instance = default;
    Slider _playerHpSlider;
    List<GameObject> _gimmickObjList;
    /// <summary>�������������ɐF��ς���Obj</summary>
    List<GameObject> _lampObjList;
    /// <summary>gimmick�̏��Ԃ��Ǘ����邽�߂̕ϐ�</summary>
    int _gimmickConter = 0;
    /// <summary>�c�e����\�����邽�߂�Img</summary>
    public List<GameObject> BulletImageList;
    public bool GimmickActive = true;
    public bool ShotOk = true;
    public int MagazineBullets = 6;
    float _time;
    Color _defaultColor;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    void Start()
    {
        _playerHpSlider = _attach.GetPlayerHpSlider;
        _playerHpSlider.maxValue = _setValues.GetPlayerHP;
        _gimmickObjList = _attach.GetGimmickObjList;
        _lampObjList = _attach.GetLampObjList;
        BulletImageList = _attach.GetBulletImageList;
        _defaultColor = _lampObjList[0].GetComponent<Renderer>().material.color;

        for (int i = 0; i < BulletImageList.Count; i++)
        {
            BulletImageList[i].SetActive(true);
        }
    }

    void Update()
    {
        _playerHpSlider.value = _setValues.GetPlayerHP;

        //�c�e�������Ȃ����Ƃ��Ɏ��s
        if (MagazineBullets <= 0)
        {
            //Player�̎ˌ��������ł��Ȃ��悤�ɉ񂵁ACT�p��timer����
            ShotOk = false;
            _time += Time.deltaTime;

            //timer��Ct�ɒB�����Ƃ��Ɏ��s
            if (_time >= _setValues.GetUseTimeForReload)
            {
                GetComponent<AudioSource>().Play();
                //timer�����Z�b�g���Ďc�e���ő�ɖ߂�
                _time = 0;
                MagazineBullets = _setValues.GetCapacity;

                //�c�e��\��UI���ĕ\������
                for (int i = 0; i < _setValues.GetCapacity; i++)
                {

                    BulletImageList[i].SetActive(true);
                    GunSound[i].SetActive(false);
                }
                //Player�̎ˌ��������ɂ���
                ShotOk = true;
            }
        }
    }

    //�M�~�b�N�����Ԓʂ�Ɍ�����Ă��邩�̔��菈��
    public void GimmickJudge(GameObject _onHitObj)
    {
        //�������ׂ�Obj�Ǝ��ۂɌ����ꂽObj�̔�r����
        if (_onHitObj == _gimmickObjList[_gimmickConter])
        {
            //��������������lamp�̐F��ς���
            _lampObjList[_gimmickConter].GetComponent<Renderer>().material
                .color = Color.red;
            _gimmickConter++;
            
            //�S�ď��Ԓʂ�Ɍ��������Ɏ��s
            if (_gimmickConter >= _gimmickObjList.Count)
            {
                GimmickClear();
            }
        }
        else
        {
            //���Ԃ��ԈႦ���ꍇ�A���Z�b�g
            if (_gimmickConter > 0)
            {
                for (int i = _gimmickConter; i >= 0; i--)
                {
                    _lampObjList[i].GetComponent<Renderer>()
                        .material.color = _defaultColor;
                }
                _gimmickConter = 0;
            }
        }
    }

    void GimmickClear()
    {
        GimmickActive = false;
        Debug.Log("�M�~�b�N�N���A!!");
    }
}

[System.Serializable]
public class SetValues
{
    [Header("PlayerHP")]
    [SerializeField] int playerHP = 15;
    public int GetPlayerHP
    {
        set
        { playerHP = value; }
        get
        { return playerHP;}
    }
    [Header("reload�Ɏg�p���鎞��")]
    [SerializeField] float useTimeForReload = 1.5f;
    public float GetUseTimeForReload => useTimeForReload;
    [Header("�e�̑��e��")]
    [SerializeField] int bulletCapacity = 6;
    public int GetCapacity => bulletCapacity;
}

[System.Serializable]
class AttachmentObj
{
    [Header("PlayerHP")]
    [SerializeField] Slider playerHpSlider;
    public Slider GetPlayerHpSlider => playerHpSlider;
    [Header("�M�~�b�NObj")]
    [SerializeField] List<GameObject> gimmickObjList = new();
    public List<GameObject> GetGimmickObjList => gimmickObjList;
    [Header("������gimmick���������������ɐF��ύX����Obj")]
    [SerializeField] List<GameObject> lampObjList = new List<GameObject>();
    public List<GameObject> GetLampObjList => lampObjList;
    [Header("BulletImage")]
    [SerializeField] List<GameObject> bulletImageList = new();
    public List<GameObject> GetBulletImageList => bulletImageList;
}