using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ScriptableAttack", menuName =
    "ScriptableObjects2/ScriptableAction/ScriptableAttack", order = 1)]
public class ScriptableAttack : ScriptableAction
{
    public override void OnFinishedState()
    {
        GameManager.gm.UpdateText("va te perdono");
    }

    public override void OnSetState(StateController2 sc)
    {
        base.OnSetState(sc);
        GameManager.gm.UpdateText("a q te meto");
    }

    public override void OnUpdate()
    {
        GameManager.gm.UpdateText("que te meto asin");
    }
}
