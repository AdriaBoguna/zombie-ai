using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ScriptablePatrol", menuName = "ScriptableObjects2/ScriptableAction/ScriptablePatrol", order = 4)]
public class ScriptablePatrol : ScriptableAction
{
    public override void OnFinishedState()
    {
        GameManager.gm.UpdateText("donde se metió?");
    }

    public override void OnSetState(StateController2 sc)
    {
        base.OnSetState(sc);
        GameManager.gm.UpdateText("vamo a patrulla");
    }

    public override void OnUpdate()
    {
        GameManager.gm.UpdateText("apatrullando la ciuda");
    }
}
