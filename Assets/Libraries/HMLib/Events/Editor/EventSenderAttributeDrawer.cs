using UnityEditor;
using UnityEngine;
using System;
using System.Reflection;

[CustomPropertyDrawer(typeof(EventSenderAttribute))]
public class EventSenderDrawer : PropertyDrawer {

    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {                        

        //var eventSenderAttribute = attribute as EventSenderAttribute;            
        var gameEvent = property.objectReferenceValue as GameEvent;
        
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        if (gameEvent == null) {

            // Calculate rects
            var objectRect = new Rect(position.x, position.y, position.width - 60, position.height);
            var buttonRect = new Rect(position.x + position.width - 60, position.y, 60, position.height);

            // Draw fields
            EditorGUI.ObjectField(objectRect, property, GUIContent.none);
            if (GUI.Button(buttonRect, "create")) {                

                // Get type of property using reflection                            
                var bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.Public;
                var targetObjectClassType = property.serializedObject.targetObject.GetType();
                var field = targetObjectClassType.GetField(property.propertyPath, bindingFlags);        
                
				var asset = ScriptableObject.CreateInstance(field.FieldType);
				ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
					asset.GetInstanceID(),
					ScriptableObject.CreateInstance<EndNameEdit>(),
					string.Format("{0}.asset", targetObjectClassType.Name + "." + ObjectNames.NicifyVariableName(field.Name).Replace(" ", "")),
					AssetPreview.GetMiniThumbnail(asset), 
					null);

				property.objectReferenceValue = asset;							
            }            
        }
        else {
            EditorGUI.ObjectField(position, property, GUIContent.none);
        }

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {

         return EditorGUI.GetPropertyHeight(property);
    }
}