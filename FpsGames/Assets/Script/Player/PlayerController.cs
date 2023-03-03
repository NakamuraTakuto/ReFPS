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

    // Update is called once per frame
    void Update()
    {
        //CharacterController�œ�����
        Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        dir = Camera.main.transform.TransformDirection(dir);
        dir.y = 0;
        _controller.SimpleMove(dir * _moveSpeed);

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

            if (hit.collider.gameObject.TryGetComponent<ActiveBase>(out var _active)
                && Input.GetButtonDown("Fire1") && GameManager.Instance.ShotOk)
            {
                //����Ă���script�̏������Ăяo��
                _active.Active();
                GameManager.Instance
                    .MagazineBullets--;
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
        [Header("CrossHair�̐ݒ�")]
        [SerializeField] Image _crossHair;
        public Image GetCrossHair => _crossHair;
        /// <summary>Player�ɐݒ肷��e</summary>
        [Header("Player�ɐݒ肷��e")]
        [SerializeField] GameObject _gun;
        public GameObject GetGun => _gun;
        [Header("�e�������郌�C���[")]
        [SerializeField] LayerMask _hitLayer = default;
        public LayerMask GetHitLayer => _hitLayer;
    }

    [System.Serializable]
    class SetValues
    {
        /// <summary>�˒�</summary>
        [Header("�˒��̒���")]
        [SerializeField] float _shotRange = 15f;
        public float GetShotRange => _shotRange;
        [Header("Player�̈ړ����x")]
        [SerializeField] float _moveSpeed = 5f;
        public float GetMoveSpeed => _moveSpeed;
    }
}
