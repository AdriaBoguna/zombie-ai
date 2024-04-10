using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranadeExplosion : MonoBehaviour
{

    Transform granadeTr;
    Rigidbody granadeRb;

    public bool explode = false;
    public float damageArea = 0f;
    public float throwForce = 0f;
    public float explodePower = 0f;
    public float lifeTime = 0f;
    public float explodeDamage = 0f;

    private float time = 0f;

    public LayerMask hitboxMask;

    Vector3 lastGranadePos;

    public bool showDebugGizmos = true;

    // Start is called before the first frame update
    void Start()
    {
        granadeTr = GetComponent<Transform>();
        granadeRb = GetComponent<Rigidbody>();

        hitboxMask = LayerMask.NameToLayer("Hitbox");

        granadeRb.velocity = granadeTr.forward * throwForce;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (explode)
        {
            if (time >= lifeTime)
            {
                ExplodeNow();
                Destroy(this.gameObject);
            }
        }
        else
        {
            DetectCollision();

            if (time >= lifeTime)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void ExplodeNow()
    {
        Vector3 explodePos = granadeTr.position;
        Collider[] checking = Physics.OverlapSphere(explodePos, this.damageArea, ~hitboxMask);

        if (checking.Length > 0 )
        {
            foreach (Collider c in checking )
            {
                GameObject go = c.gameObject;

                if ( go.layer == hitboxMask ) 
                {
                    BodyPartHitCheck playerBodyPart = go.GetComponent<BodyPartHitCheck>();
                    if (playerBodyPart != null)
                    {
                        Vector3 collisionPos = c.ClosestPoint(explodePos);

                        float distance = Vector3.Distance(explodePos, collisionPos);
                        float damageDisminution = distance / damageArea;
                        float finalDmage = explodeDamage - explodeDamage * damageDisminution;

                        playerBodyPart.TakeHit(finalDmage);

                        Debug.Log("Impacto en " + playerBodyPart.BodyName);
                    }
                }
            }
        }
    }

    public void DetectCollision()
    {
        Vector3 granadeNewPost = granadeTr.position;
        Vector3 granadeDirection = lastGranadePos - granadeNewPost;

        RaycastHit hit;

        if (Physics.Raycast(granadeNewPost, granadeDirection.normalized, out hit, granadeDirection.magnitude))
        {
            GameObject go = hit.collider.gameObject;

            if (go.layer == hitboxMask)
            {
                BodyPartHitCheck playerBodyPart = go.GetComponent<BodyPartHitCheck>();

                if (playerBodyPart != null)
                {
                    playerBodyPart.TakeHit(explodeDamage);
                    Debug.Log("Inpacto en " + playerBodyPart.BodyName);
                }
            }
        }

        lastGranadePos = granadeNewPost;
    }

    private void OnDrawGizmos()
    {
        if (showDebugGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(granadeTr.position, damageArea);
        }
    }
}
