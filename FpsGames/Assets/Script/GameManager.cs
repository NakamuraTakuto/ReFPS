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
    [SerializeField] AttachmentObj _attach;
    public static GameManager Instance = default;
    Slider _playerHpSlider;
    List<GameObject> _gimmickObjList;
    /// <summary>正解だった時に色を変えるObj</summary>
    List<GameObject> _lampObjList;
    /// <summary>gimmickの順番を管理するための変数</summary>
    int _gimmickConter = 0;
    public bool GimmickActive = true;
    public bool ShotOk = true;
    public int MagazineBullets = 6;
    float _time;

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
    }

    void Update()
    {
        _playerHpSlider.value = _setValues.GetPlayerHP;

        if (MagazineBullets <= 0)
        {
            ShotOk = false;
            _time += Time.deltaTime;

            if (_time >= _setValues.GetUseTimeForReload)
            {
                _time = 0;
                MagazineBullets = _setValues.GetCapacity;
                ShotOk = true;
            }
        }
    }

    public void GimmickJudge(GameObject _onHitObj)
    {
        if (_onHitObj == _gimmickObjList[_gimmickConter])
        {
            _lampObjList[_gimmickConter].GetComponent<Renderer>().material
                .color = Color.red;
            _gimmickConter++;
            
            if (_gimmickConter >= _gimmickObjList.Count)
            {
                GimmickClear();
            }
        }
        else
        {
            if (_gimmickConter > 0)
            {
                for (int i = _gimmickConter; i >= 0; i--)
                {
                    _lampObjList[i].GetComponent<Renderer>()
                        .material.color = Color.blue;
                }
                _gimmickConter = 0;
            }
        }
    }

    void GimmickClear()
    {
        GimmickActive = false;
        Debug.Log("ギミッククリア!!");
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
    [Header("reloadに使用する時間")]
    [SerializeField] float useTimeForReload = 1.5f;
    public float GetUseTimeForReload => useTimeForReload;
    [Header("銃の装弾数")]
    [SerializeField] int bulletCapacity = 6;
    public int GetCapacity => bulletCapacity;
}

[System.Serializable]
class AttachmentObj
{
    [Header("PlayerHP")]
    [SerializeField] Slider playerHpSlider;
    public Slider GetPlayerHpSlider => playerHpSlider;
    [Header("ギミックObj")]
    [SerializeField] List<GameObject> gimmickObjList = new();
    public List<GameObject> GetGimmickObjList => gimmickObjList;
    [Header("撃ったgimmickが正解だった時に色を変更するObj")]
    [SerializeField] List<GameObject> lampObjList = new List<GameObject>();
    public List<GameObject> GetLampObjList => lampObjList;
}