using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("CrossHair�̐ݒ�")]
    [SerializeField] Image _crossHair;
    /// <summary>�˒�</summary>
    [Header("�˒��̒���")]
    [SerializeField] float _shotRange = 15f;
    /// <summary>Player�ɒǏ]���Ă���J����</summary>
    [Header("Player�ɒǏ]����J������ݒ�")]
    [SerializeField] GameObject _followCamera;
    /// <summary>Player�ɐݒ肵�Ă���e</summary>
    [Header("Player�̏e��ݒ�")]
    [SerializeField] GameObject _gun;
    /// <summary>Player���U��������x</summary
    [Header("player�̐U��������x")]
    [SerializeField] float _lookSpeed = 1;
    [Header("�e�������郌�C���[")]
    [SerializeField] LayerMask _hitLayer = default;
    [Header("Player�̈ړ����x")]
    [SerializeField] float _moveSpeed = 5f;
    CharacterController _controller;
   
    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //CharacterController�œ�����
        Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        dir = Camera.main.transform.TransformDirection(dir);
        dir.y = 0;
        _controller.SimpleMove(dir * _moveSpeed);//* Time.deltaTime);

        //Cinemachine�̌����Ă����������Ă���Plyer�̌��������𒲐�����
        Vector3 _camera = Camera.main.transform.TransformDirection(Vector3.forward);
        _gun.transform.forward = _camera;
        _camera.y = 0;
        transform.forward = _camera;
        Vector3 _hair = _crossHair.transform.position;

        //Ray���΂��Ēe�̓����蔻������
        Ray ray = Camera.main.ScreenPointToRay(_crossHair.rectTransform.position);

        //�����script������Ă��Ă����ɏ�����Ă��鏈�������s����
        if (Physics.Raycast(ray, out RaycastHit hit, _shotRange, _hitLayer))
        {
            _crossHair.color = Color.white;

            if (hit.collider.gameObject.TryGetComponent<ShotActive>(out var _active)
                && Input.GetButtonDown("Fire1"))
            {
                //����Ă���script�̏������Ăяo��
                _active.Active();
            }
        }
        else
        {
            _crossHair.color = Color.black;
        }
    }
}
