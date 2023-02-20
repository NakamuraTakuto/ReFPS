using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotActive : MonoBehaviour
{
    [Header("Objで行いたい処理を選択")]
    [SerializeField] ActionType _type = ActionType.Enemy;

     public void Active()
    {
        switch(_type)
        {
            case ActionType.Enemy:
                /*RigidbodyとEnemyContollerをとってきて
                 * 後ろへ行く様に力を加える、HPを減らす*/
                break;

            case ActionType.Trick:
                /*ギミックのboolを書き換える。
                 * Objの色を変更する*/
                break;

            case ActionType.Test:
                gameObject.GetComponent<Renderer>().material.color = Color.red;
                break;
            case ActionType.aaa:
                gameObject.GetComponent<Renderer>().material.color = Color.blue;
                break;
        }
    }



    enum ActionType
    {
        Enemy,
        Trick,
        Test,
        aaa,
    }
}
