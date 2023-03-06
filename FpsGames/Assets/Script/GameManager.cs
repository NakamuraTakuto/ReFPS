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
    /// <summary>正解だった時に色を変えるObj</summary>
    List<GameObject> _lampObjList;
    /// <summary>gimmickの順番を管理するための変数</summary>
    int _gimmickConter = 0;
    /// <summary>残弾数を表示するためのImg</summary>
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

        //残弾が無くなったときに実行
        if (MagazineBullets <= 0)
        {
            //Playerの射撃処理をできないように回し、CT用のtimerを回す
            ShotOk = false;
            _time += Time.deltaTime;

            //timerがCtに達したときに実行
            if (_time >= _setValues.GetUseTimeForReload)
            {
                GetComponent<AudioSource>().Play();
                //timerをリセットして残弾を最大に戻す
                _time = 0;
                MagazineBullets = _setValues.GetCapacity;

                //残弾を表すUIを再表示する
                for (int i = 0; i < _setValues.GetCapacity; i++)
                {

                    BulletImageList[i].SetActive(true);
                    GunSound[i].SetActive(false);
                }
                //Playerの射撃処理を可にする
                ShotOk = true;
            }
        }
    }

    //ギミックが順番通りに撃たれているかの判定処理
    public void GimmickJudge(GameObject _onHitObj)
    {
        //撃たれるべきObjと実際に撃たれたObjの比較判定
        if (_onHitObj == _gimmickObjList[_gimmickConter])
        {
            //正解だった時にlampの色を変える
            _lampObjList[_gimmickConter].GetComponent<Renderer>().material
                .color = Color.red;
            _gimmickConter++;
            
            //全て順番通りに撃った時に実行
            if (_gimmickConter >= _gimmickObjList.Count)
            {
                GimmickClear();
            }
        }
        else
        {
            //順番を間違えた場合、リセット
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
    [Header("BulletImage")]
    [SerializeField] List<GameObject> bulletImageList = new();
    public List<GameObject> GetBulletImageList => bulletImageList;
}