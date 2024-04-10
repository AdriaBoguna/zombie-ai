using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController3 : StateController2
{
    public float AttackDistance;
    public float HP;
    private float nextHurt = 0;

    void Update()
    {
        StateTransition();
        if(currentState.action!=null)   currentState.action.OnUpdate();
        if (Input.GetKey("space") && Time.time >= nextHurt)
        {
            OnHurt(1);
            nextHurt = Time.time + 0.3f;
        }
    }

    public void OnHurt(float damage)
    {
        HP -= damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        target = collision.gameObject;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        target = null;
    }
}
