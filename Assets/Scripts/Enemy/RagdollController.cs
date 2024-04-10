using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    Animator playerAnim;

    Rigidbody playerBody;
    Rigidbody[] playerBones;

    PlayerController PLAYER;

    public List<HitMultiplier> hitStats;

    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GetComponent<Animator>();

        playerBody = GetComponentInParent<Rigidbody>();
        playerBones = GetComponentsInChildren<Rigidbody>();

        PLAYER = GetComponentInParent<PlayerController>();

        SetUp();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUp()
    {
        LayerMask layerOfHits = LayerMask.NameToLayer("Hitbox");

        foreach (Rigidbody bone in playerBones)
        {
            bone.collisionDetectionMode = CollisionDetectionMode.Continuous;
            bone.gameObject.layer = layerOfHits;

            BodyPartHitCheck partToCheck = bone.gameObject.AddComponent<BodyPartHitCheck>();

            partToCheck.PLAYER = PLAYER;

            string bName = bone.gameObject.name.ToLower();

            foreach (HitMultiplier hit in hitStats)
            {
                if (bName.Contains(hit.boneName))
                {
                    partToCheck.Multiplier = hit.multiplyBy;
                    partToCheck.BodyName = hit.boneName;
                    break;
                }
            }
        }
        Active(false);
    }

    public void Active(bool state)
    {
        foreach (Rigidbody bone in playerBones)
        {
            Collider c = bone.GetComponent<Collider>();

            if (bone.useGravity != state)
            {
                c.isTrigger = !state;
                bone.isKinematic = !state;
                bone.useGravity = state;
                if (!bone.isKinematic)
                {
                    bone.velocity = playerBody.velocity;
                }
            }
        }

        playerAnim.enabled =  !state;
        playerBody.useGravity = !state;
        playerBody.detectCollisions = !state;
        playerBody.isKinematic = state;
    }
}

[System.Serializable]

public class HitMultiplier
{
    public string boneName = "head";
    public float multiplyBy = 1;
}