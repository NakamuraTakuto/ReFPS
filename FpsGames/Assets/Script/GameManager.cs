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
        
    }

    void Update()
    {
        
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
}

[System.Serializable]
class AttachmentObj
{

}