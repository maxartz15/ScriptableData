using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Random = UnityEngine.Random;

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

		public static void Draw(this SerializedObject serializedObject, Rect position)
		{
			// Get position with indentation.
			Rect indentedPosition = EditorGUI.IndentedRect(position);

			EditorGUI.indentLevel++;
			// Draw a background that shows us clearly which fields are part of the ScriptableObject.
			GUI.Box(new Rect(indentedPosition.x, position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing - 1, position.width, position.height - EditorGUIUtility.singleLineHeight - EditorGUIUtility.standardVerticalSpacing), "");
			// Draw color coded side to indicate the scriptable object instance.
			if (serializedObject.targetObject != null)
			{
				Random.InitState(serializedObject.targetObject.GetInstanceID());
				EditorGUI.DrawRect(new Rect(indentedPosition.x - EditorGUIUtility.standardVerticalSpacing, position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing - 1, EditorGUIUtility.standardVerticalSpacing, position.height - EditorGUIUtility.singleLineHeight - EditorGUIUtility.standardVerticalSpacing), Random.ColorHSV(0f, 1f, 1f, 1f, 0.75f, 0.75f));
			}

			Rect pRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing, position.width, 0);

			// Iterate over all the values and draw them.
			SerializedProperty p = serializedObject.GetIterator();

			EditorGUI.BeginChangeCheck();

			if (p.NextVisible(true))
			{
				do
				{
					if (IsIgnoredProperty(p))
					{
						continue;
					}

					pRect.height = EditorGUI.GetPropertyHeight(p, new GUIContent(p.displayName), true);
					// Draw property.
					EditorGUI.PropertyField(pRect, p, true);

					pRect.y += pRect.height + EditorGUIUtility.standardVerticalSpacing;
				}
				while (p.NextVisible(false));
			}

			if (EditorGUI.EndChangeCheck())
			{
				serializedObject.ApplyModifiedProperties();
			}

			EditorGUI.indentLevel--;
		}

		public static bool HasVisableSubProperties(this SerializedObject serializedObject)
		{
			SerializedProperty p = serializedObject.GetIterator();

			if (p.NextVisible(true))
			{
				do
				{
					if (IsIgnoredProperty(p))
					{
						continue;
					}

					return true;
				}
				while (p.NextVisible(false));
			}

			return false;
		}

		public static bool IsIgnoredProperty(SerializedProperty property)
		{
			// Skip drawing the class file.
			if (property.name == "m_Script")
			{
				return true;
			}

			return false;
		}
	}
}