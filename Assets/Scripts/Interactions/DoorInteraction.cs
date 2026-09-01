using Crawler;
using UnityEngine;

namespace Crawler 
{
    public class DoorInteraction : Interactable
    {
        [SerializeField] private TileFeatureDoor feature;

        public TileFeatureDoor Feature { get => feature; set => feature = value; }

        public override void OnInit()
        {
            base.OnInit();
            
            if(feature.open)
                transform.rotation = Quaternion.Euler(Vector3.up * 80);
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnInteractedWith(Player player)
        {
            base.OnInteractedWith(player);

            if(feature.open) 
            {
                transform.Rotate(Vector3.up * -80);
                feature.open = false;
            }
            else 
            {
                transform.Rotate(Vector3.up * 80);
                feature.open = true;
            }
        }

        public TileFeatureDoor GetSerializableObject()
        {
            Feature.Core.name = gameObject.name;
            Feature.Core.position = transform.position;
            Feature.Core.euler = transform.rotation.eulerAngles;
            
            return Feature;
        }
    }
}