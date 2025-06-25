using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EnemyPrefabCreatorEditor : EditorWindow
{
    [MenuItem("Enemy Tools/Prefab Creator")]
    public static void ShowWindow()
    {
        // Open the custom window
        GetWindow<EnemyPrefabCreatorEditor>("Prefab Creator");
    }

    // Fields to fill in the data
    private string enemyName = "New Enemy";
    private Sprite previewImage;
    private float imageScale;
    private string prefabPath = "Assets/Prefabs/EnemyPrefabs";
    private GameObject templateEnemyPrefab; // The template prefab to use for the enemy (always same template)
    private GameObject character; // The new prefab created
    private EnemyPrefab newEnemySO; // The ScriptableObject to be created
    private EnemyCore.EnemyType enemyType; // To store the selected enemy type from the enum
    private int maxHealth = 0;
    private int defense= 0;
    private int damage= 0;
    private float attackRatePerSecond= 0;
    private float movemantSpeed = 0;


    private void OnEnable()
    {
        // Automatically assign the template prefab in your project (can be set manually or hardcoded)
        templateEnemyPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Templates/MainTemplate.prefab");

        // Debugging: Check if prefab is correctly loaded
        if (templateEnemyPrefab == null)
        {
            Debug.LogError("Enemy Template Prefab is missing. Please assign it in the Prefab Creator Tool.");
        }
    }

    void OnGUI()
    {
        GUILayout.Label("Create New Enemy Prefab", EditorStyles.boldLabel);

        // Display template enemy prefab (this should always be the template)
        EditorGUILayout.LabelField("Template Enemy Prefab", templateEnemyPrefab.name);

        // Get enemy name input
        enemyName = EditorGUILayout.TextField("Enemy Name", enemyName);

        // Get enemy preview sprite input
        previewImage = (Sprite)EditorGUILayout.ObjectField("Preview Image", previewImage, typeof(Sprite), false);
        character = (GameObject)EditorGUILayout.ObjectField("Character animated prf", character, typeof(GameObject), false);
        imageScale = EditorGUILayout.FloatField("character scale", imageScale = 1);

        enemyType = (EnemyCore.EnemyType)EditorGUILayout.EnumPopup("Enemy Type", enemyType);
        maxHealth = EditorGUILayout.IntField("Unit Health", maxHealth);
        defense = EditorGUILayout.IntField("Unit Defense", defense);
        damage = EditorGUILayout.IntField("Unit Damage", damage);
        attackRatePerSecond = EditorGUILayout.FloatField("Unit Attack Rate", attackRatePerSecond);
        movemantSpeed = EditorGUILayout.FloatField("Unit Movement Speed", movemantSpeed);


        // Prefab save location
        prefabPath = EditorGUILayout.TextField("Prefab Path", prefabPath);

        // Button to create the new prefab and scriptable object
        if (GUILayout.Button("Create Enemy Prefab"))
        {
            CreatePrefabAndScriptableObject();
        }
    }
    bool Validations()
    {
        if (string.IsNullOrEmpty(enemyName))
        {
            EditorUtility.DisplayDialog("Error", "Please provide an enemy name.", "OK");
            return false;
        }

        if (previewImage == null)
        {
            EditorUtility.DisplayDialog("Error", "Please provide a preview image.", "OK");
            return false;
        }

        if (templateEnemyPrefab == null)
        {
            EditorUtility.DisplayDialog("Error", "Template Enemy Prefab is missing. Please assign it in the Prefab Creator Tool.", "OK");
            return false;
        }

        // Ensure the prefab path exists, if not create it
        if (!AssetDatabase.IsValidFolder(prefabPath))
        {
            string parentFolder = "Assets/Prefabs"; // Parent folder for the prefab
            string folderName = prefabPath.Substring(prefabPath.LastIndexOf("/") + 1);

            // Create folder if it doesn't exist
            AssetDatabase.CreateFolder(parentFolder, folderName);
            Debug.Log("Created folder: " + prefabPath);
        }
        return true;
    }

    private void CreatePrefabAndScriptableObject()
    {
        // Validate inputs
        if(!Validations()) return;

        // Step 1: Instantiate the template prefab
        GameObject newGameObject = Instantiate(templateEnemyPrefab);
        newGameObject.name = enemyName; // Set the name of the new prefab
        GameObject characterObject = Instantiate(character, new Vector3(0, 2, 0), Quaternion.identity);
        characterObject.transform.SetParent(newGameObject.transform);

        // Step 2: Apply the settings to the new prefab (set stats, name, etc.)
        EnemyCore enemyCore = newGameObject.GetComponent<EnemyCore>();
        if (enemyCore != null)
        {            
            enemyCore.enemyType = enemyType;
            enemyCore.SetAttackRatePerSecond(attackRatePerSecond); // Example stats
            enemyCore.SetDamage(damage);
            enemyCore.SetHealth(maxHealth);
            enemyCore.SetDefense(defense);
            enemyCore.SetMovementSpeed(movemantSpeed);
        }

        // Step 3: Save the instantiated GameObject as a new prefab
        string prefabSavePath = $"{prefabPath}/{enemyName}.prefab";
        PrefabUtility.SaveAsPrefabAsset(newGameObject, prefabSavePath);

        // Step 4: Create the ScriptableObject for this new enemy
        newEnemySO = ScriptableObject.CreateInstance<EnemyPrefab>();
        newEnemySO.enemyName = enemyName;
        newEnemySO.prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabSavePath);  // Store the newly created GameObject prefab reference in the ScriptableObject
        //newEnemySO.previewImage = previewImage;


        // Step 6: Save the ScriptableObject as an asset in the project
        string assetPath = $"{prefabPath}/{enemyName}.asset";
        //PrefabUtility.SaveAsPrefabAsset(newGameObject, prefabSavePath);
       // AssetDatabase.CreateAsset(newGameObject, prefabSavePath);
        AssetDatabase.CreateAsset(newEnemySO, assetPath);
        AssetDatabase.SaveAssets();

        // Step 7: Refresh the asset database so the new prefab and scriptable object show up
        AssetDatabase.Refresh();

        // Step 8: Optionally, select the newly created prefab and asset
        EditorGUIUtility.PingObject(newGameObject);
        EditorGUIUtility.PingObject(newEnemySO);

        // Notify the user that the prefab and scriptable object were created successfully
        EditorUtility.DisplayDialog("Success", "Prefab and ScriptableObject created successfully!", "OK");
    }


}