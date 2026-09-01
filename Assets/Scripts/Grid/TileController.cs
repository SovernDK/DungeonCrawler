using System.Collections.Generic;
using Crawler.Actors;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Crawler 
{
    public class TileController : MonoBehaviour, IPoolable
    {
        [SerializeField] [InlineProperty] [HideLabel] 
        private Tile tile;
        
        [SerializeField] 
        private float offsetY;

        [SerializeField]
        private Trap trap;

        [SerializeField]
        private List<Action> actions;

        public Tile Tile { get => tile; set => tile = value; }
        public float OffsetY { get => offsetY; set => offsetY = value; }
        public Trap Trap { get => trap; set => trap = value; }

        #region IPoolable properties
        public string Name => gameObject.name;
        public GameObject Prefab => gameObject;
        public int AmountToPool => 10;
        public bool ShouldExpand => true;
        #endregion

        public void Draw()
        {
            transform.rotation = Quaternion.Euler(Vector3.up * tile.Rotation);
        }

        public Vector3 GetActorStand()
        {
            return transform.position + (Vector3.up * offsetY);
        }

        public void ExecuteActions(Actor actor)
        {
            actions.ForEach(action => {
                action.Execute(actor, actor);
            });
        }

        #region IPoolable methods
        public void OnObjectDisbaled()
        {
            
        }

        public void OnObjectEnabled()
        {
            
        }

        public void OnSpawned()
        {
            
        }
        #endregion
    }
}