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
        //CharacterController‚Å“®‚©‚·
        Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        dir = Camera.main.transform.TransformDirection(dir);
        dir.y = 0;
        _controller.SimpleMove(dir * _moveSpeed);

        //Cinemachine‚ÌŒü‚¢‚Ä‚é•ûŒü‚ğæ‚Á‚Ä‚«‚ÄPlyer‚ÌŒü‚­•ûŒü‚ğ’²®‚·‚é
        Vector3 _camera = Camera.main.transform.TransformDirection(Vector3.forward);
        _gun.transform.forward = _camera;
        _camera.y = 0;
        transform.forward = _camera;
        Vector3 _hair = _crossHair.transform.position;

        //Ray‚ğ”ò‚Î‚µ‚Ä’e‚Ì“–‚½‚è”»’è‚ğæ‚é
        Ray ray = Camera.main.ScreenPointToRay(_crossHair.rectTransform.position);

        //“Á’è‚Ìscript‚ğæ‚Á‚Ä‚«‚Ä‚»‚±‚É‘‚©‚ê‚Ä‚¢‚éˆ—‚ğÀs‚·‚é
        if (Physics.Raycast(ray, out RaycastHit hit, _shotRange, _hitLayer))
        {
            _crossHair.color = Color.white;

            if (hit.collider.gameObject.TryGetComponent<ActiveBase>(out var _active)
                && Input.GetButtonDown("Fire1") && GameManager.Instance.ShotOk)
            {
                //æ‚Á‚Ä‚«‚½script‚Ìˆ—‚ğŒÄ‚Ño‚·
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
        [Header("CrossHair‚Ìİ’è")]
        [SerializeField] Image _crossHair;
        public Image GetCrossHair => _crossHair;
        /// <summary>Player‚Éİ’è‚·‚ée</summary>
        [Header("Player‚Éİ’è‚·‚ée")]
        [SerializeField] GameObject _gun;
        public GameObject GetGun => _gun;
        [Header("’e‚ª“–‚½‚éƒŒƒCƒ„[")]
        [SerializeField] LayerMask _hitLayer = default;
        public LayerMask GetHitLayer => _hitLayer;
    }

    [System.Serializable]
    class SetValues
    {
        /// <summary>Ë’ö</summary>
        [Header("Ë’ö‚Ì’·‚³")]
        [SerializeField] float _shotRange = 15f;
        public float GetShotRange => _shotRange;
        [Header("Player‚ÌˆÚ“®‘¬“x")]
        [SerializeField] float _moveSpeed = 5f;
        public float GetMoveSpeed => _moveSpeed;
    }
}
