// References:
// https://gist.github.com/tomkail/ba4136e6aa990f4dc94e0d39ec6a058c
// Developed by Tom Kail at Inkle
// Released under the MIT Licence as held at https://opensource.org/licenses/MIT

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ScriptableData.Editor
{
	[CustomPropertyDrawer(typeof(ScriptableObject), true)]
	public class ExtendedScriptableObjectDrawer : PropertyDrawer
	{
		// Permamently exclude classes from being affected by the drawer.
		private static readonly string[] ignoredFullClassNames = new string[] { "TMPro.TMP_FontAsset" };
		private static readonly GUIContent buttonContent = EditorGUIUtility.IconContent("d_ScriptableObject On Icon");
		private const int buttonWidth = 16;

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			float totalHeight = EditorGUIUtility.singleLineHeight;

			SerializedObject serializedObject = null;
			if (property.objectReferenceValue is ScriptableObject scriptableObject)
			{
				serializedObject = new SerializedObject(scriptableObject);
			}

			if (serializedObject == null || !serializedObject.HasVisableSubProperties() || fieldInfo.HasAttribute<NonExtendableAttribute>())
			{
				return totalHeight;
			}

			if (property.isExpanded)
			{
				// Iterate over all the values and draw them.
				SerializedProperty p = serializedObject.GetIterator();

				if (p.NextVisible(true))
				{
					do
					{
						// Skip drawing the class file.
						if (EditorUtils.IsIgnoredProperty(p))
						{
							continue;
						}

						float height = EditorGUI.GetPropertyHeight(p, null, true) + EditorGUIUtility.standardVerticalSpacing;
						totalHeight += height;
					}
					while (p.NextVisible(false));
				}

				// Add a tiny bit of height if open for the background
				totalHeight += EditorGUIUtility.standardVerticalSpacing;
			}

			return totalHeight;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			Type fieldType = EditorUtils.GetFieldType(fieldInfo);

			// Draw with default drawer.
			if (fieldType == null || HasIgnoredClassName(fieldType) || fieldInfo.HasAttribute<NonExtendableAttribute>())
			{
				EditorGUI.PropertyField(position, property, label);
				EditorGUI.EndProperty();
				return;
			}

			Rect foldoutRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);
			Rect indentedPosition = EditorGUI.IndentedRect(position);
			float indentOffset = indentedPosition.x - position.x;
			Rect propertyRect = new Rect(position.x + (EditorGUIUtility.labelWidth - indentOffset + 2), position.y, position.width - (EditorGUIUtility.labelWidth - indentOffset + 2), EditorGUIUtility.singleLineHeight);

			SerializedObject serializedObject = null;
			if(property.objectReferenceValue is ScriptableObject scriptableObject)
			{
				serializedObject = new SerializedObject(scriptableObject);
			}

			// Check for sub properties and if the property can be expanded.
			if (serializedObject != null && serializedObject.HasVisableSubProperties())
			{
				property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, new GUIContent(property.displayName), true);

				if (property.isExpanded)
				{
					// Draw sub properties.
					serializedObject.Draw(position);
				}
			}
			else
			{
				EditorGUI.LabelField(foldoutRect, new GUIContent(property.displayName));

				if (property.objectReferenceValue == null)
				{
					propertyRect.width -= buttonWidth;

					// Draw create button.
					Rect buttonRect = new Rect(position.x + position.width - buttonWidth, position.y + 1, buttonWidth, EditorGUIUtility.singleLineHeight);
					DrawScriptableObjectCreateButton(buttonRect, property, fieldType);
				}
			}

			EditorGUI.BeginChangeCheck();
			// Draw object field.
			EditorGUI.PropertyField(propertyRect, property, GUIContent.none, false);

			if (EditorGUI.EndChangeCheck())
			{
				property.serializedObject.ApplyModifiedProperties();
			}

			property.serializedObject.ApplyModifiedProperties();
			EditorGUI.EndProperty();
		}

		private static void DrawScriptableObjectCreateButton(Rect position, SerializedProperty property, Type type)
		{
			if (GUI.Button(position, buttonContent, EditorStyles.iconButton))
			{
				GenericMenu typeChooser = new GenericMenu();
				IEnumerable<Type> types = type.Assembly.GetTypes().Where(t => type.IsAssignableFrom(t));
				foreach (Type t in types)
				{
					if (t.IsAbstract)
					{
						continue;
					}

					typeChooser.AddItem(new GUIContent(t.Name), false, (o) => {
						property.objectReferenceValue = EditorUtils.CreateAssetWithSavePrompt(o as Type, EditorUtils.GetSelectedAssetPath(property));
						property.serializedObject.ApplyModifiedProperties();
					}, t);
				}
				typeChooser.ShowAsContext();
			}
		}

		private static bool HasIgnoredClassName(Type type)
		{
			return ignoredFullClassNames.Contains(type.FullName);
		}
	}
}