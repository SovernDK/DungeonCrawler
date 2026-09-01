using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Crawler 
{
    public class ContainerInteraction : Lootable, ISaveableObject, IPoolable
    {
        [SerializeField] [TabGroup("Lootable")] private Animator animator;
        public Animator Animator { get => animator; set => animator = value; }

        public bool locked;

        [ShowIf("@locked == true")]
        public GameObject chestLock;
        public bool disappearIfEmpty;

        [InlineProperty]
        public EffectPrototype soundOnOpen;

        #region IPoolable Properties
        public string Name => Utility.GetObjectNameWithoutSuffix(gameObject);

        public GameObject Prefab => gameObject;

        public int AmountToPool => 5;

        public bool ShouldExpand => true;
        #endregion

        public override void Opened()
        {
            animator.SetTrigger("Opened");
            if(soundOnOpen.gameObject != null)
            {
                Gamemaster.Instance.effectsManager.AddEffect(soundOnOpen.gameObject.name, transform.position, Quaternion.identity, soundOnOpen.delay);
            }
            base.Opened();
        }

        public override void Closed()
        {
            animator.SetTrigger("Opened");
            
            base.Closed();

            if(disappearIfEmpty && LootableData.items.Length <= 0)
            {
                gameObject.SetActive(false);
            }
        }

        public override void OnInteractedWith(Player player)
        {
            if(LootableData.locked)
            {
                Item lockpick = (Gamemaster.Instance.context as MainContext).config.lockpick.Item.Copy();
                if(player.Actor.InventoryController.Inventory.ContainsItem(lockpick.Id))
                {
                    LootableData.locked = false;
                    chestLock.SetActive(false);
                }
                else
                {
                    Gamemaster.Instance.gamelog.NewEntry(null, null, "You need lockpicks to open locked chest");
                }
            }
            else
            {
                base.OnInteractedWith(player);
            }
        }

        string ISaveableObject.GetObjectId()
        {
            return GetInstanceID().ToString();
        }

        SaveableObject ISaveableObject.Save()
        {
            LootableData = new LootableData();
            if(!loaded) 
            {
                LootableData.items = new int[ItemCards.Length];

                int i = 0;
                foreach(ItemCard card in ItemCards)
                {
                    LootableData.items[i] = card.Item.Id;
                    i++;
                }

                LootableData.locked = locked;
            }

            return new SaveableObject
            {
                name = Name,
                objectId = GetInstanceID().ToString(),
                type = GetType().ToString(),
                position = transform.position,
                rotation = transform.rotation,
                data = JsonUtility.ToJson(LootableData)
            };
        }

        void ISaveableObject.Load(SaveableObject saveData)
        {
            if (saveData.data != null) 
            {
                LootableData = JsonUtility.FromJson<LootableData>(saveData.data);
            }
        }

        #region IPoolable Members
        public void OnObjectEnabled()
        {
            
        }

        public void OnObjectDisbaled()
        {
            
        }

        public void OnSpawned()
        {
            
        }
        #endregion
    }
}