using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateController : MonoBehaviour
{
    public ScriptableState2 currentState;
    void Update()
    {
        //currentState.action.OnUpdate(this);
    }
    public void StateTransition(ScriptableState2 state)
    {
        if(currentState.scriptableStateTransitions.Contains(state))
        {
            currentState.action.OnFinishedState();
            currentState = state;
           // currentState.action.OnSetState(this);
        }
    }
}
