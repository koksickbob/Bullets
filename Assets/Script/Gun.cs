using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("�ѱ� ����")]
    public string gunName;
    public float range;
    public float accuracy;
    public float fireRate;
    public float reloadTime;

    public int damage;
    [Header("�Ѿ�")]
    public int reloadBulletCount;
    public int currentBulletCount;
    public int maxBulletCount;
    public int carryBulletCount;

    [Header("�ݵ�")]
    public float retroActionForce;
    public float retroActionFineSightForce;

    public Vector3 findSightOriginPos;

    public Animator anim;
    public ParticleSystem muzzleFlash;
    public AudioClip fire_Sound;


    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
