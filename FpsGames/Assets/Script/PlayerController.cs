using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("CrossHair‚Ìİ’è")]
    [SerializeField] Image _crossHair;
    /// <summary>Ë’ö</summary>
    [Header("Ë’ö‚Ì’·‚³")]
    [SerializeField] float _shotRange = 15f;
    /// <summary>Player‚É’Ç]‚µ‚Ä‚­‚éƒJƒƒ‰</summary>
    [Header("Player‚É’Ç]‚·‚éƒJƒƒ‰‚ğİ’è")]
    [SerializeField] GameObject _followCamera;
    /// <summary>Player‚Éİ’è‚µ‚Ä‚¢‚ée</summary>
    [Header("Player‚Ìe‚ğİ’è")]
    [SerializeField] GameObject _gun;
    /// <summary>Player‚ªU‚èŒü‚­‘¬“x</summary
    [Header("player‚ÌU‚èŒü‚­‘¬“x")]
    [SerializeField] float _lookSpeed = 1;
    [Header("’e‚ª“–‚½‚éƒŒƒCƒ„[")]
    [SerializeField] LayerMask _hitLayer = default;
    [Header("Player‚ÌˆÚ“®‘¬“x")]
    [SerializeField] float _moveSpeed = 5f;
    CharacterController _controller;
   
    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //CharacterController‚Å“®‚©‚·
        Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        dir = Camera.main.transform.TransformDirection(dir);
        dir.y = 0;
        _controller.SimpleMove(dir * _moveSpeed);//* Time.deltaTime);

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

            if (hit.collider.gameObject.TryGetComponent<ShotActive>(out var _active)
                && Input.GetButtonDown("Fire1"))
            {
                //æ‚Á‚Ä‚«‚½script‚Ìˆ—‚ğŒÄ‚Ño‚·
                _active.Active();
            }
        }
        else
        {
            _crossHair.color = Color.black;
        }
    }
}
