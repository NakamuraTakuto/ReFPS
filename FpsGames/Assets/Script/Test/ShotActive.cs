using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotActive : MonoBehaviour
{
    [Header("Obj�ōs������������I��")]
    [SerializeField] ActionType _type = ActionType.Enemy;
    public void Active()
    {
        //�����ꂽObj�ɂ���ď����𕪂���
        switch(_type)
        {
            case ActionType.Enemy:
                /*Rigidbody��EnemyContoller���Ƃ��Ă���
                 * ���֍s���l�ɗ͂�������AHP�����炷*/
                break;

            case ActionType.Trick:
                /*�M�~�b�N��bool������������B
                 * Obj�̐F��ύX����*/
                break;

            case ActionType.Test:
                gameObject.GetComponent<Renderer>().material.color = Color.red;
                break;
            case ActionType.aaa:
                gameObject.GetComponent<Renderer>().material.color = Color.blue;
                break;
        }
    }


    //Obj�̎��
    enum ActionType
    {
        Enemy,
        Trick,
        Test,
        aaa,
    }
}
