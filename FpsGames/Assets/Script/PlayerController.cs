using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("CrossHairの設定")]
    [SerializeField] Image _crossHair;
    /// <summary>射程</summary>
    [Header("射程の長さ")]
    [SerializeField] float _shotRange = 15f;
    /// <summary>Playerに追従してくるカメラ</summary>
    [Header("Playerに追従するカメラを設定")]
    [SerializeField] GameObject _followCamera;
    /// <summary>Playerに設定している銃</summary>
    [Header("Playerの銃を設定")]
    [SerializeField] GameObject _gun;
    /// <summary>Playerが振り向く速度</summary
    [Header("playerの振り向く速度")]
    [SerializeField] float _lookSpeed = 1;
    [Header("弾が当たるレイヤー")]
    [SerializeField] LayerMask _hitLayer = default;
    [Header("Playerの移動速度")]
    [SerializeField] float _moveSpeed = 5f;
    CharacterController _controller;
   
    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //CharacterControllerで動かす
        Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        dir = Camera.main.transform.TransformDirection(dir);
        dir.y = 0;
        _controller.SimpleMove(dir * _moveSpeed);//* Time.deltaTime);

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

            if (hit.collider.gameObject.TryGetComponent<ShotActive>(out var _active)
                && Input.GetButtonDown("Fire1"))
            {
                //取ってきたscriptの処理を呼び出す
                _active.Active();
            }
        }
        else
        {
            _crossHair.color = Color.black;
        }
    }
}
