using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ScriptableData.Editor
{
    public static class EditorUtils
    {
        public static string GetSelectedAssetPath(SerializedProperty property)
        {
            string selectedAssetPath = "Assets";

            if (property.serializedObject.targetObject is MonoBehaviour behaviour)
            {
                MonoScript ms = MonoScript.FromMonoBehaviour(behaviour);
                selectedAssetPath = System.IO.Path.GetDirectoryName(AssetDatabase.GetAssetPath(ms));
            }

            return selectedAssetPath;
        }

        public static ScriptableObject CreateAssetWithSavePrompt(Type type, string path)
        {
            path = EditorUtility.SaveFilePanelInProject("Save ScriptableObject", type.Name + ".asset", "asset", "Enter a file name for the ScriptableObject.", path);
            if (string.IsNullOrWhiteSpace(path)) { 
                return null;
            }
            ScriptableObject asset = ScriptableObject.CreateInstance(type);
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            EditorGUIUtility.PingObject(asset);
            return asset;
        }

        public static bool HasAttribute<T>(this FieldInfo fieldInfo)
        {
            Attribute[] attributes = Attribute.GetCustomAttributes(fieldInfo);

            foreach (Attribute a in attributes)
			{
                if (a is T)
                {
                    return true;
                }
			}

            return false;
        }

        public static Type GetFieldType(FieldInfo fieldInfo)
		{
            Type type = fieldInfo.FieldType;

            if (type.IsArray)
            {
                type = type.GetElementType();
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                type = type.GetGenericArguments()[0];
            }

            return type;
        }
    }
}