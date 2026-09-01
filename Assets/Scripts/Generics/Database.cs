using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;
using Crawler;
using System;
using Sheet;

[CreateAssetMenu(fileName = "Database", menuName = "Core/Database")]
public class Database : ScriptableObject
{
    [TabGroup("Database", "Items", TabLayouting = TabLayouting.MultiRow)] [ListDrawerSettings(ShowFoldout = false)] [InlineEditor] [Searchable]
    public List<ItemCard> items;

    [TabGroup("Database", "Spells", SdfIconType.Magic, TextColor = "blue")] [ListDrawerSettings(ShowFoldout = false)] [InlineEditor]
    public List<SpellCard> spells;

    [TabGroup("Database", "Tilemap", SdfIconType.Map, TextColor = "green")] [ListDrawerSettings(ShowFoldout = false)] [InlineEditor]
    public List<GameObject> tileset;

    [TabGroup("Database", "Enemies", SdfIconType.Mastodon, TextColor = "red")] [ListDrawerSettings(ShowFoldout = false)] [InlineEditor]
    public List<EnemyCard> enemies;

    [TabGroup("Database", "States")] [ListDrawerSettings(ShowFoldout = false)] [InlineEditor]
    public List<CharacterStateCard> states;

    [TabGroup("Database", "Cursors")] 
    public List<CursorEntry> cursors;

    [TabGroup("Database", "Equipment Slots")] [ListDrawerSettings(ShowFoldout = false)] 
    public List<EquipmentSlot> slots;

    [TabGroup("Database", "Actions")] [ListDrawerSettings(ShowFoldout = false)] [InlineEditor]
    public List<Action> actions;

    #if UNITY_EDITOR
    [Button("Load")]
    private void Load()
    {
        LoadItemCards();
        LoadSpellCards();
        LoadTileset();
        LoadEnemies();
        LoadStates();
        LoadActions();
    }

    [TabGroup("Database", "Items")] [Button("Load Item Cards")]
    private void LoadItemCards()
    {
        items.Clear();

        string[] guids = AssetDatabase.FindAssets("t:ItemCard");
        int id = 0;

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ItemCard itemCard = AssetDatabase.LoadAssetAtPath<ItemCard>(path);
            if (itemCard != null && !itemCard.Item.Name.Equals("Empty"))
            {
                itemCard.Item.Id = id;
                EditorUtility.SetDirty(itemCard);
                items.Add(itemCard);
                id++;
            }
        }

        items.Sort((x, y) => x.Item.Id.CompareTo(y.Item.Id));

        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }

    [TabGroup("Database", "Spells")] [Button("Load Spell Cards")]
    private void LoadSpellCards()
    {
        spells.Clear();

        string[] guids = AssetDatabase.FindAssets("t:SpellCard");
        int id = 0;

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            SpellCard spellCard = AssetDatabase.LoadAssetAtPath<SpellCard>(path);
            if (spellCard != null)
            {
                spellCard.Spell.Id = id;
                EditorUtility.SetDirty(spellCard);
                spells.Add(spellCard);
                id++;
            }
        }

        spells.Sort((x, y) => x.Spell.Id.CompareTo(y.Spell.Id));

        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }

    [TabGroup("Database", "Tilemap")] [Button("Load Tileset")]
    private void LoadTileset()
    {
        tileset.Clear();

        // Look for all prefabs (t:Prefab) that might contain TileController components
        string[] guids = AssetDatabase.FindAssets("t:Prefab");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab != null)
            {
                TileController tileController = prefab.GetComponent<TileController>();
                if (tileController != null)
                {
                    tileset.Add(prefab);
                }
            }
        }

        tileset.Sort((x, y) => {
            TileController tx = x.GetComponent<TileController>();
            TileController ty = y.GetComponent<TileController>();

            return tx.Tile.Id.CompareTo(ty.Tile.Id);
        });

        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }

    [TabGroup("Database", "Actions")] [Button("Load Actions")]
    private void LoadActions()
    {
        actions.Clear();

        string[] guids = AssetDatabase.FindAssets("t:Action");
        int id = 0;

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Action action = AssetDatabase.LoadAssetAtPath<Action>(path);
            if (action != null)
            {
                action.Id = id;
                EditorUtility.SetDirty(action);
                actions.Add(action);
                id++;
            }
        }

        actions.Sort((x, y) => x.Id.CompareTo(y.Id));

        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }

    [TabGroup("Database", "Enemies")] [Button("Load Enemies")]
    private void LoadEnemies()
    {
        enemies.Clear();

        string[] guids = AssetDatabase.FindAssets("t:EnemyCard");
        int id = 0;

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            EnemyCard enemy = AssetDatabase.LoadAssetAtPath<EnemyCard>(path);
            if (enemy != null)
            {
                enemy.Id = id;
                EditorUtility.SetDirty(enemy);
                enemies.Add(enemy);
                id++;
            }
        }

        enemies.Sort((x, y) => x.Id.CompareTo(y.Id));

        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }

    [TabGroup("Database", "States")] [Button("Load States")]
    private void LoadStates()
    {
        states.Clear();

        string[] guids = AssetDatabase.FindAssets("t:CharacterStateCard");
        int id = 0;

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            CharacterStateCard card = AssetDatabase.LoadAssetAtPath<CharacterStateCard>(path);
            if (card != null)
            {
                card.State.Id = id;
                EditorUtility.SetDirty(card);
                states.Add(card);
                id++;
            }
        }

        states.Sort((x, y) => x.State.Id.CompareTo(y.State.Id));

        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }
    #endif
}

[Serializable]
public class CursorEntry
{
    public string name;
    public Vector2 hotSpot; 
    public Texture2D texture;
}
