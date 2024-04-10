using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    Transform bulletTr;
    Rigidbody bulletRb;

    public float bulletPower = 0f;
    public float lifeTime = 4f;

    private float time = 0f;

    public float bulletDamage = 1;

    Vector3 lastBulletPos;

    public LayerMask hitboxMask;

    // Start is called before the first frame update
    void Start()
    {
        bulletTr = GetComponent<Transform>();
        bulletRb = GetComponent<Rigidbody>();

        bulletRb.velocity = this.transform.forward * bulletPower;

        hitboxMask = LayerMask.NameToLayer("Hitbox");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time += Time.deltaTime;

        DetectCollision();

        if (time >= lifeTime)
        {
            Destroy(this.gameObject);
        }
    }

    public void DetectCollision()
    {
        Vector3 bulletNewPost = bulletTr.position;
        Vector3 bulletDirection = lastBulletPos - bulletNewPost;

        RaycastHit hit;

        if (Physics.Raycast(bulletNewPost, bulletDirection.normalized, out hit, bulletDirection.magnitude)) 
        {
            GameObject go = hit.collider.gameObject;
            if (go.layer == hitboxMask)
            {
                BodyPartHitCheck playerBodyPart = go.GetComponent<BodyPartHitCheck>();

                if (playerBodyPart != null)
                {
                    playerBodyPart.TakeHit(bulletDamage);
                    Debug.Log("Disparo en " + playerBodyPart.BodyName);
                }
            }
        }

        lastBulletPos = bulletNewPost;
    }
}
