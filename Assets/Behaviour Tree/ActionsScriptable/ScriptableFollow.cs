using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ScriptableFollow", menuName = "ScriptableObjects2/ScriptableAction/ScriptableFollow")]

public class ScriptableFollow : ScriptableAction
{
    private ChaseBehaviour _chaseBehaviour;
    private EnemyController3 _enemyController;
    public override void OnFinishedState()
    {
        _chaseBehaviour.StopChasing();
    }

    public override void OnSetState(StateController2 sc)
    {
        base.OnSetState(sc);
        GameManager.gm.UpdateText("Te persigo");
        _chaseBehaviour = sc.GetComponent<ChaseBehaviour>();
        _enemyController = (EnemyController3)sc;
    }

    public override void OnUpdate()
    {
        _chaseBehaviour.Chase(_enemyController.target.transform, sc.transform);
    }
}
