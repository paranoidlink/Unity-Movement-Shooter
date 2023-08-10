using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shooting : MonoBehaviour
{
 //Gun Stats
 private int damage;
 private float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
 private int magazineSize, bulletsPerTap;
 private bool allowButtonHold;
 int bulletsLeft, bulletsShot;

 //bools
 bool shooting, readyToShoot, reloading, changingWeapon;

 //Reference
 public Camera fpsCam;
 public Transform attackPoint;
 public RaycastHit rayHit;
 public LayerMask whatIsEnemy;
 public LayerMask whatIsGround;
 public GameObject assaultRifle;
 public GameObject sniperRifle;
 public GameObject shotgun;
 private GameObject currentWeaponModel;
 private int weaponKeyPressed;

 //Graphics
 public GameObject muzzleFlash, bulletHoleGraphic;
 public TextMeshProUGUI text;

 private void Start()
 {
    bulletsLeft = magazineSize;
    readyToShoot = true;


    //Setting the default gun to the assault rifle
    currentWeaponModel = assaultRifle;
    shotgun.SetActive(false);
    sniperRifle.SetActive(false);
    damage = 20;
    timeBetweenShooting = 0.05f;
    spread = 0.02f;
    range = 200;
    reloadTime = 1;
    timeBetweenShots = 0;
    bulletsPerTap = 1;
    magazineSize = 30;
    allowButtonHold = true;
    bulletsLeft = magazineSize;
 }

 private void Update()
 {
    MyInput();

    //SetText
    text.SetText(bulletsLeft +" / " + magazineSize);
 }

 private void MyInput()
 {
    if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
    else shooting = Input.GetKeyDown(KeyCode.Mouse0);

    if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

    //Shoot
    if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
    {
        bulletsShot = bulletsPerTap;
        Shoot();
    }

    //Change Weapon
    if(Input.GetKeyDown(KeyCode.Alpha1))
    {
        changingWeapon = true;
        weaponKeyPressed = 1;
    }
    else if(Input.GetKeyDown(KeyCode.Alpha2))
    {
        changingWeapon = true;
        weaponKeyPressed = 2;
    }
    else if (Input.GetKeyDown(KeyCode.Alpha3))
    {
        changingWeapon = true;
        weaponKeyPressed = 3;
    }

    if (changingWeapon && !shooting) ChangeWeapon();

    
 }

private void Shoot()
{
    readyToShoot = false;

    //Spread
    float x = Random.Range(-spread, spread);
    float y = Random.Range(-spread, spread);

    //Calculate Direction with Spread
    Vector3 direction = fpsCam.transform.forward + new Vector3(x,y,0);

    //RayCast
    if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range))
    {
        Debug.Log(rayHit.collider.name);
        
    

    if (rayHit.collider.CompareTag("Enemy"))
        rayHit.collider.GetComponent<EnemyAi>().TakeDamage(damage);
    }

    //Graphics
    // Calculate rotation based on the normal of the surface
    Quaternion rotation = Quaternion.LookRotation(rayHit.normal);
    Instantiate(bulletHoleGraphic, rayHit.point, rotation);

    //Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);


    bulletsLeft--;
    bulletsShot--;
    Invoke("ResetShot", timeBetweenShooting);

    if(bulletsShot > 0 && bulletsLeft > 0)
    Invoke("Shoot", timeBetweenShots);
}

private void ResetShot()
{
    readyToShoot = true;
}

 private void Reload()
{
    reloading = true;
    Invoke("ReloadFinished", reloadTime);
}

private void ReloadFinished()
{
    bulletsLeft = magazineSize;
    reloading = false;
}

private void ChangeWeapon()
{
    changingWeapon = false;
    currentWeaponModel.SetActive(false);

    switch(weaponKeyPressed)
    {
        case 3:
            sniperRifle.SetActive(true);
            currentWeaponModel = sniperRifle;
            damage = 100;
            timeBetweenShooting = 2;
            spread = 0;
            range = 1000;
            reloadTime = 3;
            timeBetweenShots = 0;
            bulletsPerTap = 1;
            magazineSize = 5;
            allowButtonHold = false;
            bulletsLeft = magazineSize;
            break;
        case 2:
            shotgun.SetActive(true);
            currentWeaponModel = shotgun;
            damage = 10;
            timeBetweenShooting = 1;
            spread = 0.1f;
            range = 100;
            reloadTime = 1.5f;
            timeBetweenShots = 0;
            bulletsPerTap = 8;
            magazineSize = 16;
            allowButtonHold = false;
            bulletsLeft = magazineSize;
            break;
        case 1:
            assaultRifle.SetActive(true);
            currentWeaponModel = assaultRifle;
            damage = 20;
            timeBetweenShooting = 0.05f;
            spread = 0.02f;
            range = 200;
            reloadTime = 1;
            timeBetweenShots = 0;
            bulletsPerTap = 1;
            magazineSize = 30;
            allowButtonHold = true;
            bulletsLeft = magazineSize;
            break;
    }



}

}
