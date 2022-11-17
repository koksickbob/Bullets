using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("ÃÑ±â ¼º´É")]
    public string gunName;
    public float range;
    public float accuracy;
    public float fireRate;
    public float reloadTime;

    public int damage;
    [Header("ÃÑ¾Ë")]
    public int reloadBulletCount;
    public int currentBulletCount;
    public int maxBulletCount;
    public int carryBulletCount;

    [Header("¹Ýµ¿")]
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
