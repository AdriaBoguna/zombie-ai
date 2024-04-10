using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartHitCheck : MonoBehaviour
{
    [HideInInspector]
    public PlayerController PLAYER;

    public string BodyName;
    public float Multiplier;
    public float LastDamage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeHit(float damage)
    {
        LastDamage = damage * Multiplier;

        this.PLAYER.TakeDamage(LastDamage);

        Debug.Log(damage + " * " + Multiplier + " = " + LastDamage);
    }
}
