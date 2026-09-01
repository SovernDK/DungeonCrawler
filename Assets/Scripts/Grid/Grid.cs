using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using Crawler.Actors;
using Sirenix.Utilities;


namespace Crawler
{
    public class Grid : MonoBehaviour
    {
        #region Privates
        [SerializeField] private bool gizmos;
        [SerializeField] private LayerMask tilesetLayer;
        [SerializeField] private Vector3Int dimensions;

        private Dictionary<Vector3Int, Tile> map;
        private List<SaveableObject> saveableObjects;
        private List<Spawner> spawners;
        private Dictionary<Vector3Int, TileController> mapComponents;
        private Pathfinding pathfinding;
        #endregion

        private List<Actor> actors;
        private Database db;
        public Dictionary<Vector3Int, Tile> Map { get => map; set => map = value; }
        public List<Spawner> Spawners { get => spawners; set => spawners = value; }
        public Dictionary<Vector3Int, TileController> MapComponents { get => mapComponents; set => mapComponents = value; }
        public Vector3Int Dimensions { get => dimensions; set => dimensions = value; }
        public LayerMask TilesetLayer { get => tilesetLayer; set => tilesetLayer = value; }
        public Pathfinding Pathfinding { get => pathfinding; set => pathfinding = value; }

        public void Initialize(Database db)
        {
            this.db = db;

            map = new Dictionary<Vector3Int, Tile>();
            mapComponents = new Dictionary<Vector3Int, TileController>();
            spawners = new List<Spawner>();
            saveableObjects = new List<SaveableObject>();
            actors = new List<Actor>();

            pathfinding = GetComponent<Pathfinding>();
            pathfinding.Initialize(this);
        }

        public void BuildMap(string name, List<SaveableObject> objects = null)
        {
            string filePath = Path.Combine(Application.persistentDataPath, name);

            if (!File.Exists(filePath))
            {
                Debug.LogError("File not found: " + filePath);
            }

            string json = File.ReadAllText(filePath);

            Deserialize(json);
            UpdateGrid();

            if(objects != null)
            {
                saveableObjects = objects;
            }

            InitializeMap();
        }

        [Button("Save Tileset")]
        void CreateGrid()
        {
            spawners = new List<Spawner>();
            saveableObjects = new List<SaveableObject>();

            map = new Dictionary<Vector3Int, Tile>();
            actors = new List<Actor>();
            
            for(int x = 0; x <= dimensions.x; x++)
            {
                for(int y = 0; y <= dimensions.y; y++)
                {
                    Vector3 rayStart = new Vector3(x * dimensions.z, 15, y * dimensions.z);
                    RaycastHit hit;

                    if(Physics.Raycast(rayStart, Vector3.down, out hit, 100f, tilesetLayer))
                    {
                        GameObject parent = hit.collider.transform.parent.gameObject;
                        parent.transform.position = new Vector3(Mathf.Round(parent.transform.position.x),
                                                                                parent.transform.position.y,
                                                                                Mathf.Round(parent.transform.position.z));                

                        TileController tileController = parent.GetComponent<TileController>();
                        Vector3Int position = new Vector3Int(x, 0, y);
                        
                        tileController.Tile.Coordinates = position;
                        tileController.Tile.WorldPositionY = parent.transform.position.y;
                        tileController.Tile.Rotation = Mathf.Round(tileController.transform.rotation.eulerAngles.y);

                        map.Add(position, tileController.Tile);

                        IEnumerable<ISaveableObject> saveables = FindObjectsOfType<MonoBehaviour>().OfType<ISaveableObject>();
                        saveables.ForEach(save => saveableObjects.Add(save.Save()));
                    }
                }
            }

            FindObjectsOfType<SpawnerController>(true).ToList().ForEach(controller => {
                spawners.Add(controller.spawner);
            });

            string filePath = Path.Combine(Application.persistentDataPath, "start_map.json");
            string content = Serialize();

            File.WriteAllText(filePath, content);

            Debug.Log("Map Saved to " + filePath);
        }

        public TileController GetTile(Vector3Int position)
        {
            TileController tile;
            if(mapComponents.TryGetValue(position, out tile))
            {
                return tile;
            }

            return null;
        }

        public TileController MoveActorToTile(Vector3Int start, Vector3Int target)
        {
            if(IsContained(target) && target != start)
            {
                if (mapComponents.TryGetValue(target, out TileController tileController))
                {
                    if(CanMove(start, target - start) && !tileController.Tile.IsOccupied)
                    {
                        return tileController;
                    }
                }
            }

            return null;
        }

        public bool IsContained(Vector3Int position)
        {
            return position.x >= 0 && position.x <= dimensions.x &&
                   position.z >= 0 && position.z <= dimensions.y;
        }

        public bool CanMove(Vector3Int current, Vector3Int direction)
        {
            if(mapComponents.TryGetValue(current, out TileController tileController))
            {
                WalkableDirection walkableDirection = tileController.Tile.Directions.Find(dir =>
                    dir.direction == tileController.transform.InverseTransformDirection(direction));

                if(walkableDirection == null)
                {
                    Debug.LogError("Can't move: " + current + ", " + direction);
                }

                return walkableDirection.isWalkable;
            }
            else 
            {
                Debug.LogError("Trying to access non existent TileController");
            }

            return false;
        }

        public Tile[] GetPassableNeighbours(Tile tile)
        {
            List<Tile> result = new List<Tile>();

            new List<Vector3Int>() 
            {
                new Vector3Int(0, 0, 1),  // Forward
                new Vector3Int(1, 0, 0),  // Left
                new Vector3Int(0, 0, -1), // Backward
                new Vector3Int(-1, 0, 0)  // Right
            }.ForEach(next => 
            {
                if(map.TryGetValue(tile.Coordinates + next, out Tile current))
                {
                    if(CanMove(tile.Coordinates, next) && !current.IsOccupied) 
                        result.Add(current);
                }
            });

            return result.ToArray();
        }

        public Tile[] GetNeighbours(Tile tile)
        {
            List<Tile> result = new List<Tile>();

            new List<Vector3Int>() 
            {
                new Vector3Int(0, 0, 1), // Forward
                new Vector3Int(1, 0, 0), // Left
                new Vector3Int(0, 0, -1), // Backward
                new Vector3Int(-1, 0, 0) // Right
            }.ForEach(next => 
            {
                if(map.TryGetValue(tile.Coordinates + next, out Tile current))
                {
                    result.Add(current);
                }
            });

            return result.ToArray();
        }

        public List<Actor> FindActorsInRange(Vector3Int center, int range)
        {
            List<Actor> actorsInRange = new List<Actor>();
            Queue<Tile> frontier = new Queue<Tile>();

            frontier.Enqueue(map[center]);

            while(frontier.Count > 0)
            {
                Tile current = frontier.Dequeue();
                if(Vector3Int.Distance(center, current.Coordinates) > range) break;

                foreach(Tile next in GetNeighbours(current))
                {
                    Actor actor = actors.Find(actor => actor.CurrentTile.Tile.Coordinates == next.Coordinates);
                    if(actor != null) 
                        actorsInRange.Add(actor);
                }
            }

            Actor targetActor = actors.Find(actor => actor.CurrentTile.Tile.Coordinates == center);
            if(targetActor != null)
                actorsInRange.Add(targetActor);

            return actorsInRange;
        }

        public void InsertTile(int tileId, Vector3Int position, float rotation = 0)
        {
            if(!IsContained(position))
            {
                Debug.LogError("Tried to add tile in position beyond dimensions!");
                return;
            }

            Tile tile = new Tile(tileId)
            {
                Rotation = rotation,
                Coordinates = position
            };
            
            if(map.TryGetValue(position, out _))
            {
                map[position] = tile;
            }
            else
            {
                map.Add(position, tile);      
            }

            UpdateGrid();
        }

        public void EraseTile(Vector3Int position)
        {
            if(map.TryGetValue(position, out _))
            {
                map.Remove(position);
            }

            UpdateGrid();
        }

        public string Serialize()
        {
            TileMapWrapper wrapper = new TileMapWrapper();
            foreach (KeyValuePair<Vector3Int, Tile> kvp in map)
            {
                wrapper.tiles.Add(new TileEntry { key = kvp.Key, value = kvp.Value });
            }

            IEnumerable<ISaveableObject> saveables = FindObjectsOfType<MonoBehaviour>().OfType<ISaveableObject>();
            foreach (ISaveableObject saveable in saveables)
            {
                wrapper.saveableObjects.Add(saveable.Save());
            }

            foreach (Spawner spawner in spawners)
            {
                wrapper.spawners.Add(new TileSpawnerEntry { value = spawner });
            }

            return JsonUtility.ToJson(wrapper, true);
        }

        public void Deserialize(string content)
        {
            TileMapWrapper wrapper = JsonUtility.FromJson<TileMapWrapper>(content);

            Dictionary<Vector3Int, Tile> map = new Dictionary<Vector3Int, Tile>();
            List<TileFeature> features = new List<TileFeature>();

            foreach (TileEntry entry in wrapper.tiles)
            {
                map.Add(entry.key, entry.value);
            }

            foreach (SaveableObject saveData in wrapper.saveableObjects)
            {
                saveableObjects.Add(saveData);
            }

            foreach(TileSpawnerEntry spawner in wrapper.spawners)
            {
                spawners.Add(spawner.value);
            }

            this.map = map;
        }

        public void SetActorPosition(Actor actor, Vector3Int coordinates)
        {
            TileController controller;
            if(mapComponents.TryGetValue(coordinates, out controller))
            {
                actor.CurrentTile = controller;
                actor.transform.position = controller.GetActorStand();
                controller.Tile.IsOccupied = true;
            }
        }

        public void SetTrapPosition(Trap trap)
        {
            if(mapComponents.TryGetValue(trap.TrapData.coordinates, out TileController controller))
            {
                controller.Trap = trap;
            }
        }

        public void UpdateGrid()
        {
            foreach(KeyValuePair<Vector3Int, TileController> keyValuePair in mapComponents)
            {
                keyValuePair.Value.gameObject.SetActive(false);
            }

            foreach(KeyValuePair<Vector3Int, Tile> keyValuePair in map)
            {
                GameObject tilePrefab = db.tileset.Find(tile => tile.GetComponent<TileController>().Tile.Id == keyValuePair.Value.Id);
                Vector3Int coordinates = keyValuePair.Key;

                TileController tileController;
                if(!mapComponents.TryGetValue(coordinates, out tileController)) 
                {
                    float y = keyValuePair.Value.WorldPositionY;
                    Vector3 spawnPosition = coordinates * dimensions.z;
                    spawnPosition.y = y;
                    
                    //Create
                    GameObject tilePrefabClone = PoolManager.Instance.SpawnFromPool(tilePrefab.name, spawnPosition, Quaternion.identity);
                    TileController tileControllerClone = tilePrefabClone.GetComponent<TileController>();
                    
                    if(tileControllerClone == null)
                    {
                        Debug.LogError("Error: " + tilePrefab.name);
                    }

                    tileControllerClone.Tile.Rotation = keyValuePair.Value.Rotation;
                    tileControllerClone.Tile.Coordinates = coordinates;
                    tileControllerClone.Tile.IsOccupied = keyValuePair.Value.IsOccupied;
                
                    tileControllerClone.Draw();
                    mapComponents.Add(coordinates, tileControllerClone);
                }
            }
        }

        public List<Trap> FindAllTrapsInRange(Vector3Int coordinates, int range)
        {
            Queue<Tile> frontier = new Queue<Tile>();
            frontier.Enqueue(map[coordinates]);

            List<Trap> traps = new List<Trap>();

            while(frontier.Count > 0)
            {
                Tile current = frontier.Dequeue();
                if(Vector3Int.Distance(coordinates, current.Coordinates) > range) break;

                foreach(Tile next in GetNeighbours(current))
                {
                    if(mapComponents.TryGetValue(next.Coordinates, out TileController controller))
                    {
                        if(controller.Trap != null) traps.Add(controller.Trap);
                    }
                }
            }

            return traps;
        }

        private void InitializeMap()
        {
            foreach (SaveableObject saveableObject in saveableObjects)
            {
                GameObject spawnedObject = PoolManager.Instance.SpawnFromPool(saveableObject.name, saveableObject.position, saveableObject.rotation);
                spawnedObject.GetComponent<ISaveableObject>().Load(saveableObject);
                if(spawnedObject.TryGetComponent(out Actor actor))
                {
                    SetActorPosition(actor, actor.ActorSaveableData.coordinates);
                }
                else if(spawnedObject.TryGetComponent(out Trap trap))
                {
                    SetTrapPosition(trap);
                }
            }

            foreach(Spawner spawner in spawners)
            {
                GameObject spawnedObject = PoolManager.Instance.SpawnFromPool(spawner.objectPoolTag, spawner.coordinates * dimensions.z, Quaternion.identity);

                Actor spawnedActor = spawnedObject.GetComponent<Actor>();
                SetActorPosition(spawnedActor, spawner.coordinates);

                actors.Add(spawnedActor);
            }
        }

        private void OnDrawGizmos() 
        {
            Gizmos.color = Color.green;

            if(!gizmos) return;

            for(int x = 0; x <= dimensions.x; x++)
            {
                for(int y = 0; y <= dimensions.y; y++)
                {
                    Vector3 center = new Vector3(x * dimensions.z, 0, y * dimensions.z);
                    Gizmos.DrawWireCube(center, Vector3.one * dimensions.z);
                }
            }
        }
    }

    // =====================
    // SERIALIZATION OBEJCTS
    // =====================
    
    [Serializable]
    public class TileMapWrapper
    {
        public List<TileEntry> tiles = new List<TileEntry>();
        public List<SaveableObject> saveableObjects = new List<SaveableObject>();
        public List<TileSpawnerEntry> spawners = new List<TileSpawnerEntry>();
    }

    [Serializable]
    public class TileEntry
    {
        public Vector3Int key;
        public Tile value;
    }

    [Serializable]
    public class TileSpawnerEntry
    {
        public Spawner value;
    }
}
