#if UNITY_EDITOR
using System.Collections.Generic;
using Codice.Client.BaseCommands;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using System.Linq;

public class EntityIDAssigner:AssetPostprocessor
{
    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deledtedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        Dictionary<EntityType, List<EntityDef_SO>> entitiesByType = FindAllEntities();

        foreach (string assetPath in importedAssets)
        {
            EntityDef_SO entityAsset = AssetDatabase.LoadAssetAtPath<EntityDef_SO>(assetPath);

            if (entityAsset != null && entityAsset.entityID == -1)
            {
                EntityType currentType = entityAsset.entityType;

                if (entitiesByType.TryGetValue(currentType, out List<EntityDef_SO> entitiesOfSameType))
                {
                    int startID = entityAsset.StartID;

                    int maxID = entitiesOfSameType.Any() ?
                    entitiesOfSameType.Where(e => e.entityID >= startID)
                    .Max(e => e.entityID) : startID - 1;

                    int nextID = Mathf.Max(startID, maxID + 1);

                    entityAsset.SetID(nextID);

                    EditorUtility.SetDirty(entityAsset);
                }
            }
        }
        AssetDatabase.SaveAssets();
    }
    
    private static Dictionary<EntityType, List<EntityDef_SO>> FindAllEntities()
    {
        string[] guids = AssetDatabase.FindAssets("t:EntityDef_SO");

        Dictionary<EntityType, List<EntityDef_SO>> entitiesByType = new();

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            EntityDef_SO entity = AssetDatabase.LoadAssetAtPath<EntityDef_SO>(path);

            if (entity != null)
            {
                EntityType type = entity.entityType;
                if (!entitiesByType.ContainsKey(type))
                {
                    entitiesByType[type] = new List<EntityDef_SO>();
                }
                entitiesByType[type].Add(entity);
            }
        }

        return entitiesByType;
    }
}
#endif