using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableState", menuName =
    "ScriptableObjects2/ScriptableState", order = 3)]
public class ScriptableState2 : ScriptableObject
{
    public ScriptableAction action;    
    public List<ScriptableState2> scriptableStateTransitions;
}
