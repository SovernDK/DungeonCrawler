using System;
using UnityEngine;

namespace Crawler 
{
    [Serializable]
    public class TileFeature
    {
        public string name;
        public Vector3Int coordinates;
        public Vector3 position;
        public Vector3 euler;
    }

    [Serializable]
    public class LootableData
    {
        public int[] items;
        public bool locked;
    }

    [Serializable]
    public class TileFeatureDoor
    {
        [SerializeField] private TileFeature core;
        public bool open;

        public TileFeature Core { get => core; set => core = value; }
    }
}
