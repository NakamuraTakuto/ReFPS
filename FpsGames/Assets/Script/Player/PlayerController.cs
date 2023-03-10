using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] AttachmentObj _attach;
    [SerializeField] SetValues _setValues;
    CharacterController _controller;
    private float _moveSpeed;
    private float _shotRange;
    private GameObject _gun;
    private Image _crossHair;
    private LayerMask _hitLayer;

   
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _moveSpeed = _setValues.GetMoveSpeed;
        _shotRange = _setValues.GetShotRange;
        _gun = _attach.GetGun;
        _crossHair = _attach.GetCrossHair;
        _hitLayer = _attach.GetHitLayer;
    }

    void Update()
    {
        //CharacterControllerで動かす
        Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        dir = Camera.main.transform.TransformDirection(dir);
        dir.y = 0;
        _controller.SimpleMove(dir * _moveSpeed);

        //Cinemachineの向いてる方向を取ってきてPlyerの向く方向を調整する
        Vector3 _camera = Camera.main.transform.TransformDirection(Vector3.forward);
        _gun.transform.forward = _camera;
        _camera.y = 0;
        transform.forward = _camera;
        Vector3 _hair = _crossHair.transform.position;

        //Rayを飛ばして弾の当たり判定を取る
        Ray ray = Camera.main.ScreenPointToRay(_crossHair.rectTransform.position);

        //特定のscriptを取ってきてそこに書かれている処理を実行する
        if (Physics.Raycast(ray, out RaycastHit hit, _shotRange, _hitLayer))
        {
            _crossHair.color = Color.white;

            if (hit.collider.gameObject.TryGetComponent<ActiveBase>(out var _active)
                && Input.GetButtonDown("Fire1") && GameManager.Instance.ShotOk)
            {
                //取ってきたscriptの処理を呼び出す
                _active.Active();
                GameManager.Instance
                    .MagazineBullets--;
                GameManager.Instance.
                    GunSound[GameManager.Instance.MagazineBullets].SetActive(true);
                GameManager.Instance.
                    BulletImageList[GameManager.Instance.MagazineBullets].SetActive(false);
            }
        }
        else
        {
            _crossHair.color = Color.black;
        }
    }

    [System.Serializable]
    class AttachmentObj
    {
        [Header("CrossHairの設定")]
        [SerializeField] Image _crossHair;
        public Image GetCrossHair => _crossHair;
        /// <summary>Playerに設定する銃</summary>
        [Header("Playerに設定する銃")]
        [SerializeField] GameObject _gun;
        public GameObject GetGun => _gun;
        [Header("弾が当たるレイヤー")]
        [SerializeField] LayerMask _hitLayer = default;
        public LayerMask GetHitLayer => _hitLayer;
    }

    [System.Serializable]
    class SetValues
    {
        /// <summary>射程</summary>
        [Header("射程の長さ")]
        [SerializeField] float _shotRange = 15f;
        public float GetShotRange => _shotRange;
        [Header("Playerの移動速度")]
        [SerializeField] float _moveSpeed = 5f;
        public float GetMoveSpeed => _moveSpeed;
    }
}
