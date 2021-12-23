// References:
// https://gist.github.com/tomkail/ba4136e6aa990f4dc94e0d39ec6a058c
// Developed by Tom Kail at Inkle
// Released under the MIT Licence as held at https://opensource.org/licenses/MIT

using System;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

namespace ScriptableData.Editor
{
    [CustomPropertyDrawer(typeof(ScriptableObject), true)]
    public class ExtendedScriptableObjectDrawer : PropertyDrawer
    {
        private static readonly string[] ignoreClassFullNames = new string[] { "TMPro.TMP_FontAsset" };
        private const int buttonWidth = 20;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float totalHeight = EditorGUIUtility.singleLineHeight;
            if (property.objectReferenceValue == null || !HasVisableSubProperties(property) || fieldInfo.HasAttribute<NonExtendableAttribute>())
            {
                return totalHeight;
            }

            if (property.isExpanded)
            {
				ScriptableObject data = property.objectReferenceValue as ScriptableObject;

                if (data == null)
                { 
                    return EditorGUIUtility.singleLineHeight;
                }

                SerializedObject serializedObject = new SerializedObject(data);
                SerializedProperty prop = serializedObject.GetIterator();
                if (prop.NextVisible(true))
                {
                    do
                    {
                        if (prop.name == "m_Script")
                        { 
                            continue;
                        }

						SerializedProperty subProp = serializedObject.FindProperty(prop.name);
                        float height = EditorGUI.GetPropertyHeight(subProp, null, true) + EditorGUIUtility.standardVerticalSpacing;
                        totalHeight += height;
                    }
                    while (prop.NextVisible(false));
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

            if (fieldType == null || HasIgnoredClassName(fieldType) || fieldInfo.HasAttribute<NonExtendableAttribute>())
            {
                EditorGUI.PropertyField(position, property, label);
                EditorGUI.EndProperty();
                return;
            }

            ScriptableObject propertySO = null;
            if (!property.hasMultipleDifferentValues && property.serializedObject.targetObject != null && property.serializedObject.targetObject is ScriptableObject)
            {
                propertySO = property.serializedObject.targetObject as ScriptableObject;
            }

            GUIContent guiContent = new GUIContent(property.displayName);
			Rect foldoutRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);
            if (property.objectReferenceValue != null && HasVisableSubProperties(property))
            {
                property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, guiContent, true);
            }
            else
            {
                foldoutRect.x += 10;
                EditorGUI.Foldout(foldoutRect, property.isExpanded, guiContent, true, EditorStyles.label);
            }

			Rect indentedPosition = EditorGUI.IndentedRect(position);
			float indentOffset = indentedPosition.x - position.x;
            Rect propertyRect = new Rect(position.x + (EditorGUIUtility.labelWidth - indentOffset + 2), position.y, position.width - (EditorGUIUtility.labelWidth - indentOffset + 2), EditorGUIUtility.singleLineHeight);

            if (propertySO != null || property.objectReferenceValue == null)
            {
                propertyRect.width -= buttonWidth;
            }

            EditorGUI.BeginChangeCheck();

            Object assignedObject = EditorGUI.ObjectField(propertyRect, GUIContent.none, property.objectReferenceValue, fieldType, false);

            if (EditorGUI.EndChangeCheck())
            {
                property.objectReferenceValue = assignedObject;
                property.serializedObject.ApplyModifiedProperties();
            }

			Rect buttonRect = new Rect(position.x + position.width - buttonWidth, position.y, buttonWidth, EditorGUIUtility.singleLineHeight);

            if (property.propertyType == SerializedPropertyType.ObjectReference && property.objectReferenceValue != null)
            {
                if (property.isExpanded)
                {
                    DrawScriptableObjectChildFields(position, property);
                }
            }
            else
            {
                DrawScriptableObjectCreateButton(buttonRect, property, fieldType);
            }

            property.serializedObject.ApplyModifiedProperties();
            EditorGUI.EndProperty();
        }

        private void DrawScriptableObjectChildFields(Rect position, SerializedProperty property)
		{
            ScriptableObject scriptableObject = (ScriptableObject)property.objectReferenceValue;

            if (scriptableObject == null)
            {
                return;
            }

            // Draw a background that shows us clearly which fields are part of the ScriptableObject
            GUI.Box(new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing - 1, position.width, position.height - EditorGUIUtility.singleLineHeight - EditorGUIUtility.standardVerticalSpacing), "");

            EditorGUI.indentLevel++;
            SerializedObject serializedObject = new SerializedObject(scriptableObject);

            // Iterate over all the values and draw them
            SerializedProperty prop = serializedObject.GetIterator();
            float y = position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            EditorGUI.BeginChangeCheck();

            if (prop.NextVisible(true))
            {
                do
                {
                    // Don't bother drawing the class file
                    if (prop.name == "m_Script") continue;
                    float height = EditorGUI.GetPropertyHeight(prop, new GUIContent(prop.displayName), true);
                    EditorGUI.PropertyField(new Rect(position.x, y, position.width /*- buttonWidth*/, height), prop, true);
                    y += height + EditorGUIUtility.standardVerticalSpacing;
                }
                while (prop.NextVisible(false));
            }

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }

            EditorGUI.indentLevel--;
        }

        private void DrawScriptableObjectCreateButton(Rect position, SerializedProperty property, Type type)
		{
            if (GUI.Button(position, "+"))
            {
                if (type.IsAbstract)
                {
                    GenericMenu typeChooser = new GenericMenu();
                    foreach (var elem in type.Assembly.GetTypes().Where(t => type.IsAssignableFrom(t)))
                    {
                        if (elem.IsAbstract) continue;

                        typeChooser.AddItem(new GUIContent(elem.Name), false, (elem) => {
                            property.objectReferenceValue = EditorUtils.CreateAssetWithSavePrompt(elem as Type, EditorUtils.GetSelectedAssetPath(property));
                            property.serializedObject.ApplyModifiedProperties();
                        }, elem);
                    }
                    typeChooser.ShowAsContext();
                }
                else
                {
                    property.objectReferenceValue = EditorUtils.CreateAssetWithSavePrompt(type, EditorUtils.GetSelectedAssetPath(property));
                }
            }
        }

        private bool HasIgnoredClassName(Type type)
		{
            return ignoreClassFullNames.Contains(type.FullName);
        }

        private bool HasVisableSubProperties(SerializedProperty property)
		{
            ScriptableObject scriptableObject = (ScriptableObject)property.objectReferenceValue;

            if (scriptableObject != null)
            {
                SerializedObject serializedObject = new SerializedObject(scriptableObject);
                SerializedProperty prop = serializedObject.GetIterator();
                while (prop.NextVisible(true))
                {
                    if (prop.name == "m_Script") continue;
                    return true; //if theres any visible property other than m_script
                }
            }
            return false;
        }
    }
}