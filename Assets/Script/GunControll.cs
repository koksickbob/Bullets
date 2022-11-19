using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControll : MonoBehaviour
{
    public Gun currentGun;
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
            currentFireRate -= Time.deltaTime; //초당 1발씩 발사
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
        if (!currentReload)
        {
            if (currentGun.currentBulletCount > 0)
                Shoot();
            else
            {
                CancelFineSight();
                StartCoroutine(ReloadCoroutine());
            }
        }
    }
    
    private void Shoot() // 수정 필요
    {
        currentGun.currentBulletCount--;
        currentFireRate = currentGun.fireRate;
        PlaySE(currentGun.fire_Sound);
        currentGun.muzzleFlash.Play();

        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine());

        Debug.Log("Bullet Fired");
    }

    private void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && currentReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            CancelFineSight();
            StartCoroutine(ReloatCoroutine());
        }
    }

    IEnumerator ReloadCoroutine()
    {
        if(currentGun.carryBulletCount> 0)
        {
            currentReload = true;
            currentGun.anim.SetTrigger("Reload");

            currentGun.carryBulletCount += currentGun.currentBulletCount;
            currentGun.currentBulletCount = 0;

            yield return new WaitForSeconds(currentGun.reloadTime);

            if (currentGun.carryBulletCount >= currentGun.reloadBulletCount)
            {
                currentGun.currentBulletCount = currentGun.reloadBulletCount;
                currentGun.carryBulletCount -= currentGun.reloadBulletCount;
            }
            else
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount;
                currentGun.carryBulletCount = 0;
            }
            currentReload = false;
        }
        else
        {
            Debug.Log("No ammo");
        }
    }

    private void TryFineSight()
    {
        if(Input.GetButtonDown("Fire2") && !currentReload)
        {
            FineSight();
        }
    }

    public void CancelFineSight()
    {
        if (isFineSightMode)
            FineSight();
    }
    private void FineSight()
    {
        isFineSightMode = !isFineSightMode;
        currentGun.anim.SetBool("FineSightMode", isFineSightMode);

        if (isFineSightMode)
        {
            StopAllCoroutines();
            StartCoroutine(FineSightActivateCoroutine());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(FineSightDeActivateCoroutine());
        }
    }

    IEnumerator FineSightDeActivateCoroutine()
    {
        while (currentGun.transform.loacalPosition != originPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, originPos, 0.2f);
            yield return null;
        }
    }

    IEnumerator RetroActionCoroutine()
    {
        Vector3 recoilBack = new Vector3(currentGun.retroActionForce, originPos.y, originPos.z);
        Vector3 retroActionRecoilback = new Vector3(currentGun.retroActionFineSightForce, currentGun.fineSightOriginPos.y, currentGun.fineSightOriginPos.z);

        if (!isFineSightMode)
        {
            currentGun.transform.localPosition = originPos;

            while(currentGun.transform.localPosition.x <= currentGun.retroActionForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
                yield return null;
            }
        }
        else
        {
            currentGun.transform.localPosition = currentGun.fineSightOriginPos;

            while (currentGun.transformlocalPosition.x <= currentGun.retroActionFineSightForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilback, 0.4f);
                yield return null;
            }

        }
    }
    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
}
