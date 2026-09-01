using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Crawler 
{
    [Serializable]
    public class Tile
    {
        //Unchangable
        [SerializeField] [BoxGroup("Tile")]
        int id;

        [SerializeField] [BoxGroup("Tile")] [HorizontalGroup("Tile/States")]
        bool isPassable;

        [SerializeField] [BoxGroup("Tile")] [HorizontalGroup("Tile/States")]
        bool isOccupied;

        [SerializeField] [BoxGroup("Tile")]
        private TileType type;

        [SerializeField] [BoxGroup("Tile")] 
        private TileGroup group;

        //Changeable
        [SerializeField] [BoxGroup("Tile")]
        float rotation;

        [SerializeField]  [BoxGroup("Tile")]
        private Vector3Int coordinates;

        [SerializeField] [BoxGroup("Tile")] 
        private ETileFeature feature;

        [SerializeField] [BoxGroup("Tile")] 
        private List<WalkableDirection> directions;
        
        [SerializeField]
        private float worldPositionY;

        public int Id { get => id; set => id = value; }
        public bool IsPassable { get => isPassable; set => isPassable = value; }
        public bool IsOccupied { get => isOccupied; set => isOccupied = value; }
        public float Rotation { get => rotation; set => rotation = value; }
        public Vector3Int Coordinates { get => coordinates; set => coordinates = value; }
        public TileType Type { get => type; set => type = value; }
        public TileGroup Group { get => group; set => group = value; }
        public ETileFeature Feature { get => feature; set => feature = value; }
        public List<WalkableDirection> Directions { get => directions; set => directions = value; }
        public float WorldPositionY { get => worldPositionY; set => worldPositionY = value; }

        public Tile(int id)
        {
            this.id = id;
        }
    }

    [Serializable]
    public class WalkableDirection 
    {
        public Vector3Int direction;
        public bool isWalkable;
    }

    public enum TileType
    {
        Corner, Wall, Floor, Hall, Doorway
    }

    public enum TileGroup
    {
        Stone, Cave, Lava
    }

    public enum ETileFeature 
    {
        None, Chest, Door
    }
}