using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class GameEvent : ScriptableObject {

    #if UNITY_EDITOR
    [Multiline] [SerializeField] string _description;
    #endif

    public abstract void RaiseTest();

    public abstract void Subscribe(System.Action<object, GameEvent> foo);
    public abstract void Unsubscribe(System.Action<object, GameEvent> foo);
}