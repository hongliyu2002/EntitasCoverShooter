using System;
using Entitas;
using Entitas.Unity.VisualDebugging;
using UnityEngine;
using UnityEditor;

public class GameObject_TypeDrawer : ITypeDrawer {
    public bool HandlesType(Type type) {
        return type == typeof(GameObject);
    }

    public object DrawAndGetNewValue(Type memberType, string memberName, object value, Entity entity, int index, IComponent component) {
        return EditorGUILayout.TextField (((GameObject)value).name);
    }
}