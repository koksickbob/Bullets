using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControll : MonoBehaviour
{
    public Gun currentGun; //Gun 스크립트 적용
    public float currentFireRate;
    public bool currentReload = false;
    public AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        GunFireRateformula(); //총기 발사 수식
        TryFire();
        TryReload();
    }
    private void GunFireRateformula()
    {
        if (currentFireRate > 0)
            currentFireRate -= Time.deltaTime; //1초당 1씩 감소
    }

    private void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0)
        {
            Fire();
        }
    }
    private void Fire() // 수정 필요
    {
        currentFireRate = currentGun.fireRate;
        Shoot();
    }
    
    private void Shoot() // 수정 필요
    {
        PlaySE(currentGun.fire_Sound);
        currentGun.muzzleFlash.Play();
        Debug.Log("총알 발사함");
    }

    private void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && currentReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            // fire, shoot 수정 후 작성
        }
    }
    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
}
