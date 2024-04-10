using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController2 : StateController
{
    public float AttackDistance;
    public float LifePoints;
    public ScriptableState2 Patrol, Follow, Attack, Die, RunState;

    public GameObject target = null;
    private float distance = 0;
    private float nextHurt = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        target = collision.gameObject;
        CheckDistance();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        StateTransition(Patrol);
        target = null;
    }
    private void CheckDistance()
    {
        distance = (target.transform.position - transform.position).magnitude;
        if (distance > AttackDistance)  StateTransition(Follow);
        else  StateTransition(Attack);
    }
    void Update()
    {
        if (target != null)       CheckDistance();
        if (Input.GetKey("space") && Time.time >= nextHurt)
        {
            OnHurt(1);
            nextHurt = Time.time + 0.3f;
        }
    }

    public void OnHurt(float damage)
    {
        GetComponent<AudioSource>().Play();
        LifePoints -= damage;
        if (LifePoints <= 0)    Death();
        if (LifePoints <= 2)    Run();
    }
    private void Run()
    {
        StateTransition(RunState);
    }
    private void Death()
    {
        StateTransition(Die);
    } 

}
