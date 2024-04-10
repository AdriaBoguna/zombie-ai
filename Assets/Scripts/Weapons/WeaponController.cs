using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Transform shootSpawn;

    public bool shooting = false;

    public float shootDelay = 0f;
    public float lastShootTime = 0f;

    public GameObject itemPrefab;
    public GameObject bulletPrefab;

    public Sprite weaponIcon;

    public enum ShootMode
    {
        Single,
        Auto
    }

    public ShootMode currentShootMode = ShootMode.Single;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            shooting = true;
            Shoot();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            shooting = false;
        }

        Debug.DrawLine(shootSpawn.position, shootSpawn.forward * 10f, Color.red);
        Debug.DrawLine(Camera.main.transform.position, Camera.main.transform.forward * 10f, Color.blue);

        RaycastHit cameraHit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out cameraHit))
        {
            Vector3 shootDirection = cameraHit.point - shootSpawn.position;
            shootSpawn.rotation = Quaternion.LookRotation(shootDirection);

        }
    }

    public void Shoot()
    {
        if (Time.time - lastShootTime > shootDelay) 
        {
            if (shooting) 
            {
                switch (currentShootMode)
                {
                    case ShootMode.Single:
                        InstantiateBullet();
                        break;
                    case ShootMode.Auto:
                        StartCoroutine(AutomaticShoot());
                        break;
                }
            }
        }
    }

    public void InstantiateBullet()
    {
        Instantiate(bulletPrefab, shootSpawn.position, shootSpawn.rotation);
    }

    IEnumerator AutomaticShoot()
    {
        while (shooting)
        {
            InstantiateBullet();
            yield return new WaitForSeconds(shootDelay);
        }
    }
}
