using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameEvent))]
public class EventEditor : Editor
{
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        GameEvent gameEvent = target as GameEvent;
        if (GUILayout.Button("Raise")) {
            gameEvent.RaiseTest();
        }
    }
}