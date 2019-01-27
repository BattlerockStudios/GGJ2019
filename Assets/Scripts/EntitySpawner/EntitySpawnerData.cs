
#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

public class EntitySpawnerData : ScriptableObject
{
    public GameObject[] entitiesToSpawn;
}

#if UNITY_EDITOR
public class CreateScriptableAsset
{
    [MenuItem("EntitySpawner", menuItem = "Battlerock/CreateEntitySpawnerData", priority = 1)]
    public static void CreateEntitySpawnerScriptableAsset()
    {
        EntitySpawnerData asset = ScriptableObject.CreateInstance<EntitySpawnerData>();

        var directory = "Assets/ScriptableAssets/";
        System.IO.Directory.CreateDirectory(directory);

        AssetDatabase.CreateAsset(asset, directory + asset.GetType().ToString() + ".asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}
#endif