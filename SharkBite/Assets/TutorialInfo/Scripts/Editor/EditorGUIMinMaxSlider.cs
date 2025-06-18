using UnityEngine;
using UnityEditor;

public class EditorGUIMinMaxSlider : EditorWindow
{
    float minVal = -10;
    float minLimit = -20;
    float maxVal = 10;
    float maxLimit = 20;

    [MenuItem("Examples/Editor GUI Move Object Randomly")]
    static void Init()
    {
        var window = GetWindow<EditorGUIMinMaxSlider>();
        window.Show();
    }

    void OnGUI()
    {
        EditorGUI.MinMaxSlider(
            new Rect(0, 0, position.width, 20),
            new GUIContent("Random range:"),
            ref minVal, ref maxVal,
            minLimit, maxLimit);
        if (GUI.Button(new Rect(0, 25, position.width, position.height - 25), "Randomize!"))
        {
            PlaceRandomly();
        }
    }

    void PlaceRandomly()
    {
        if (Selection.activeTransform)
        {
            Selection.activeTransform.position =
                new Vector3(
                    Random.Range(minVal, maxVal),
                    Random.Range(minVal, maxVal),
                    Random.Range(minVal, maxVal)
                );
        }
        else
        {
            Debug.LogError("Select a GameObject to randomize its position.");
        }
    }
}
