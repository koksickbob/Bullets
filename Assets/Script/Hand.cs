using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public string handName; // �� ����
    [Header("�� ����")][Tooltip("���� ����")]
    public float range;
    public int damage;
    public float workSpeed;
    public float attackDelay;
    public float attackActivate;
    public float attackDisabled;

    public new Animator animation;



    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
