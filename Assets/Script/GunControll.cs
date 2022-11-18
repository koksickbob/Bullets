using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControll : MonoBehaviour
{
    public Gun currentGun; //Gun ��ũ��Ʈ ����
    public float currentFireRate;
    public bool currentReload = false;
    public AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        GunFireRateformula(); //�ѱ� �߻� ����
        TryFire();
        TryReload();
    }
    private void GunFireRateformula()
    {
        if (currentFireRate > 0)
            currentFireRate -= Time.deltaTime; //1�ʴ� 1�� ����
    }

    private void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0)
        {
            Fire();
        }
    }
    private void Fire() // ���� �ʿ�
    {
        currentFireRate = currentGun.fireRate;
        Shoot();
    }
    
    private void Shoot() // ���� �ʿ�
    {
        PlaySE(currentGun.fire_Sound);
        currentGun.muzzleFlash.Play();
        Debug.Log("�Ѿ� �߻���");
    }

    private void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && currentReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            // fire, shoot ���� �� �ۼ�
        }
    }
    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
}
