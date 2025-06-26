using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(EnemyCore))]
public class EnemyScriptEditor : Editor
{
    private EnemyCore.EnemyType previousEnemyType;
    private SerializedProperty enemyType;
    private SerializedProperty maxHealth;
    private SerializedProperty defense;
    private SerializedProperty damage;
    private SerializedProperty attackRatePerSecond;
    private SerializedProperty movemantSpeed;
    private SerializedProperty enemyImage;
    private void OnEnable()
    {
        // Cache the previous EnemyType on editor initialization
        EnemyCore enemyCore = (EnemyCore)target;
        previousEnemyType = enemyCore.enemyType;

        // Get the serialized version of attackRatePerSecond
        //enemyImage = serializedObject.FindProperty("enemyImage");
        attackRatePerSecond = serializedObject.FindProperty("attackRatePerSecond");
        movemantSpeed = serializedObject.FindProperty("movemantSpeed");
        maxHealth = serializedObject.FindProperty("maxHealth");
        defense = serializedObject.FindProperty("defense");
        damage = serializedObject.FindProperty("damage");
        enemyType = serializedObject.FindProperty("enemyType");

        HandleEnemyTypeChange(enemyCore);
    }

    public override void OnInspectorGUI()
    {
        EnemyCore enemyCore = (EnemyCore)target;

        // Draw default inspector (to keep other fields visible)
        //DrawDefaultInspector();

        // Track when the enemy type is changed
        if (enemyCore.enemyType != previousEnemyType)
        {
            HandleEnemyTypeChange(enemyCore);
            previousEnemyType = enemyCore.enemyType;
        }
        // ======================================================== PROPERTIES DetailLogic ========================= 
       
        // Change the title of the 'attackRatePerSecond' based on enemy type
        string attackRateLabel = (enemyCore.enemyType == EnemyCore.EnemyType.CloseRange) ? "Attack Speed" : "Fire Rate";


        // ======================================================== PROPERTIES ARANGMENT ========================= 
        //EditorGUILayout.PropertyField(enemyImage);
        EditorGUILayout.PropertyField(enemyType);
        EditorGUILayout.PropertyField(maxHealth);
        EditorGUILayout.PropertyField(defense);
        EditorGUILayout.PropertyField(damage);
        EditorGUILayout.PropertyField(attackRatePerSecond, new GUIContent(attackRateLabel));
        EditorGUILayout.PropertyField(movemantSpeed);

        // ======================================================== ERROR MESSAES OR WARNINGS ========================= 
        if (enemyCore.enemyType == EnemyCore.EnemyType.None)
        {
            EditorGUILayout.HelpBox("Warning: Enemy Type is set to None. Please select a valid type.", MessageType.Error);
        }


        // ======================================================== SEPERAE SUBJECT =========================
        // Always make the object dirty when changes are made
        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties(); // Apply changes to the serialized object
            EditorUtility.SetDirty(target);
        }
    }

    private void HandleEnemyTypeChange(EnemyCore enemyCore)
    {
        // If the enemy is a CloseRange (Melee), instantiate the weapon
        if (enemyCore.enemyType == EnemyCore.EnemyType.CloseRange)
        {
            CombatScript combatScript = enemyCore.GetComponent<CombatScript>();
            if (combatScript != null)
            {
                DestroyImmediate(combatScript);
            }
            foreach (Transform child in enemyCore.transform.GetChild(0))
            {
                if (child.CompareTag("Weapon"))
                {
                    return; // Remove the weapon child if it's melee
                }
            }

            GameObject weaponPrf = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Templates/EnemyMeleWeapon.prefab");
            GameObject weapon = Instantiate(weaponPrf, enemyCore.transform.GetChild(0));
            weapon.transform.localPosition = new Vector3(0f, 1, 2f); // Adjust weapon position
            weapon.tag = "Weapon"; // Tag the weapon to identify it in case we need to remove it later

        }
        else if (enemyCore.enemyType == EnemyCore.EnemyType.Ranged)
        {
            foreach (Transform child in enemyCore.transform.GetChild(0))
            {
                if (child.CompareTag("Weapon"))
                {
                    DestroyImmediate(child.gameObject); // Remove the weapon child if it's melee
                }
            }
            if (enemyCore.GetComponent<CombatScript>() == null)
            {
                enemyCore.gameObject.AddComponent<CombatScript>();
            }
        }
    }
}

