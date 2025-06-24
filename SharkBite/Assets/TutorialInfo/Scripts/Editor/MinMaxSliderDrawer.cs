#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(MinMaxSliderAttribute))]
public class MinMaxSliderDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        MinMaxSliderAttribute attr = (MinMaxSliderAttribute)attribute;

        if (property.propertyType == SerializedPropertyType.Vector2)
        {
            Vector2 val = property.vector2Value;
            float min = val.x;
            float max = val.y;

            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, label);

            // Split the position rect
            float fieldWidth = 40f;
            float spacing = 5f;
            float sliderWidth = position.width - fieldWidth * 2f - spacing * 2f;

            Rect minFieldRect = new Rect(position.x, position.y, fieldWidth, position.height);
            Rect sliderRect = new Rect(minFieldRect.xMax + spacing, position.y, sliderWidth, position.height);
            Rect maxFieldRect = new Rect(sliderRect.xMax + spacing, position.y, fieldWidth, position.height);

            // Draw int fields
            min =EditorGUI.FloatField(minFieldRect, min);
            max = EditorGUI.FloatField(maxFieldRect, max);

            // Draw slider
            EditorGUI.MinMaxSlider(sliderRect, ref min, ref max, attr.Min, attr.Max);

            // Apply clamped and rounded values
            val.x = Mathf.Clamp(min, attr.Min, attr.Max);
            val.y = Mathf.Clamp(max, attr.Min, attr.Max);
            property.vector2Value = val;

            EditorGUI.EndProperty();
        }
        else if (property.propertyType == SerializedPropertyType.Vector2Int)
        {
            Vector2Int val = property.vector2IntValue;
            float min = val.x;
            float max = val.y;

            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, label);

            // Split the position rect
            float fieldWidth = 40f;
            float spacing = 5f;
            float sliderWidth = position.width - fieldWidth * 2f - spacing * 2f;

            Rect minFieldRect = new Rect(position.x, position.y, fieldWidth, position.height);
            Rect sliderRect = new Rect(minFieldRect.xMax + spacing, position.y, sliderWidth, position.height);
            Rect maxFieldRect = new Rect(sliderRect.xMax + spacing, position.y, fieldWidth, position.height);

            // Draw int fields
            min = Mathf.FloorToInt(EditorGUI.FloatField(minFieldRect, min));
            max = Mathf.FloorToInt(EditorGUI.FloatField(maxFieldRect, max));

            // Draw slider
            EditorGUI.MinMaxSlider(sliderRect, ref min, ref max, attr.Min, attr.Max);

            // Apply clamped and rounded values
            val.x = Mathf.FloorToInt(Mathf.Clamp(min, attr.Min, attr.Max));
            val.y = Mathf.FloorToInt(Mathf.Clamp(max, attr.Min, attr.Max));
            property.vector2IntValue = val;

            EditorGUI.EndProperty();
        }
        else
        {
            EditorGUI.LabelField(position, label.text, "Use [MinMaxSlider] on Vector2");
        }
    }
}
#endif