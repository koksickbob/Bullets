using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class HandHolder : MonoBehaviour
{
    [Header("현재 장착 무기")]
    public Hand currentHand; // 현재 장착된 Hand 타입 무기
    [Header("현재 상태")]
    public bool currentAttack = false;
    public bool currentSwing = false;
    public RaycastHit hitInfo; // 무기에(손) 닿은 것들의 정보
    
    void Start()
    {
        TryAttack();
    }

    private void TryAttack()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!currentAttack)
            {
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    IEnumerator AttackCoroutine()
    {
        currentAttack = true;
        currentHand.animation.SetTrigger("Attack");

        yield return new WaitForSeconds(currentHand.attackActivate);
        currentSwing = true;

        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentHand.attackDisabled);
        currentSwing = false;
    }

    IEnumerator HitCoroutine()
    {
        while (currentSwing)
        {
            if (CheckObject())
            {
                currentSwing = false;
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }

    private bool CheckObject()
    {
        if(Physics.Raycast(transform.position, transform.forward, out hitInfo, currentHand.range))
        {
            return true;
        }
        return false;
    }
    void Update()
    {
        
    }
}
