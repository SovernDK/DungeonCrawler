using System;
using System.Linq;
using System.Collections.Generic;
using Crawler.Commands;
using Sheet;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Crawler.Actors 
{
    public class Actor : MonoBehaviour, ISaveableObject
    {
        private ActorSaveableData actorSaveableData;

        #region Private
        [SerializeField] 
        private bool alive = true;

        [SerializeField]
        private string poolTag;
        
        [SerializeField] [InlineProperty] [HideLabel] [TabGroup("Character Sheet")] 
        private CharacterSheet characterSheet;

        [SerializeField] [InlineProperty] [HideLabel] [TabGroup("Spellbook")] 
        private Spellbook spellbook;

        [SerializeField] [InlineProperty] [HideLabel] [TabGroup("Inventory")] 
        private InventoryController inventoryController;

        [SerializeField] [ReadOnly] [TabGroup("Readonly")] 
        private Actor target;

        [SerializeField] [DisableInPlayMode] [InlineProperty] [TabGroup("Readonly")] 
        private Spell currentSpell;

        [SerializeField] [DisableInPlayMode] [TabGroup("Readonly")] 
        private float rotation;

        [SerializeField] [TabGroup("Readonly")]
        private Vector3 damageEffectAnchor;

        [SerializeField] [TabGroup("Action points")]
        private float actionPoints;

        [SerializeField] [TabGroup("Action points")]
        private float maxActionPoints;

        [SerializeField] [TabGroup("Action points")]
        private float baseActionPoints;

        [SerializeField] [TabGroup("Readonly")] 
        private TileController currentTile;

        [TabGroup("FX")] [InlineProperty] 
        public List<EffectPrototype> damagedEffects;
        
        private List<ActorCommand> commands;
        private MainContext context;
        private IActorContext actorContext;
        private UnityEvent onDamaged = new UnityEvent();
        private UnityEvent onDeath = new UnityEvent();
        private UnityEvent onActionPointsChange = new UnityEvent();
        private UnityEvent onEquipmentChangedEnd = new UnityEvent();
        #endregion

        #region Getters and setters
        public TileController CurrentTile { get => currentTile; set => currentTile = value; }
        public MainContext Context { get => context; set => context = value; }
        public List<ActorCommand> Commands { get => commands; set => commands = value; }
        public float Rotation { get => rotation; set => rotation = value; }
        public CharacterSheet CharacterSheet { get => characterSheet; set => characterSheet = value; }
        public Spellbook Spellbook { get => spellbook; set => spellbook = value; }
        public InventoryController InventoryController { get => inventoryController; set => inventoryController = value; }
        public Actor Target { get => target; set => target = value; }
        public Spell CurrentSpell { get => currentSpell; set => currentSpell = value; }
        public IActorContext ActorContext { get => actorContext; set => actorContext = value; }
        public Vector3 DamageEffectAnchor { get => damageEffectAnchor; set => damageEffectAnchor = value; }
        public bool Alive { get => alive; set => alive = value; }
        public ActorSaveableData ActorSaveableData { get => actorSaveableData; set => actorSaveableData = value; }
        public UnityEvent OnDeath { get => onDeath; set => onDeath = value; }
        public UnityEvent OnDamaged { get => onDamaged; set => onDamaged = value; }
        public float ActionPoints { get => actionPoints; 
            set {
                actionPoints = Mathf.Clamp(value, 0, MaxActionPoints); 
                onActionPointsChange.Invoke();
            }
        }
        public float MaxActionPoints { get => maxActionPoints; set => maxActionPoints = value; }
        public UnityEvent OnActionPointsChange { get => onActionPointsChange; set => onActionPointsChange = value; }
        public float BaseActionPoints { get => baseActionPoints; set => baseActionPoints = value; }
        public UnityEvent OnEquipmentChangedEnd { get => onEquipmentChangedEnd; set => onEquipmentChangedEnd = value; }
        #endregion

        public void Initialize(MainContext context, IActorContext actorContext)
        {
            this.context = context;
            this.actorContext = actorContext;

            alive = true;
            Config config = context.config;

            characterSheet.Init(config.attributeMaxValue, config.derivedAttributeMaxValue);
            spellbook.Init();
            inventoryController.Init(context);

            inventoryController.Equipment.onEquiped += OnEquipmentChanged;
            inventoryController.Equipment.onUnequiped += OnEquipmentChanged;

            commands = new List<ActorCommand>
            {
                new ActorMoveCommand(context, this, "MoveForward", Vector3Int.forward),
                new ActorMoveCommand(context, this, "MoveBack", Vector3Int.back),
                new ActorMoveCommand(context, this, "MoveLeft", Vector3Int.left),
                new ActorMoveCommand(context, this, "MoveRight", Vector3Int.right),
                new ActorMoveTowardCommand(context, this, "MoveToward"),
                new ActorRotateCommand(context, this, "RotateRight", -1),
                new ActorRotateCommand(context, this, "RotateLeft", 1),
                new ActorAttackCommand(context, this, "Attack"),
                new ActorSpellCastCommand(context, this, "SpellCast"),
                new ActorKnockbackCommand(context, this, "Knockforward", Vector3Int.forward),
                new ActorKnockbackCommand(context, this, "Knockback", Vector3Int.back),
                new ActorKnockbackCommand(context, this, "Knockleft", Vector3Int.left),
                new ActorKnockbackCommand(context, this, "Knockright", Vector3Int.right),
                new ActorGetHitCommand(context, this, "GetHit"),
            };
        }

        public void OnTurnStart()
        {
            characterSheet.ExecuteStatesAtBegginingOfTurn(this);
            actorContext.Done = false;
            ActionPoints = maxActionPoints;
        }
        public void OnTurnEnd()
        {
            characterSheet.ExecuteStatesAtEndOfTurn(this);
            Item item = inventoryController.Equipment.Slots[(int) EEquipmentSlot.LeftHand].GetItem();
            if(item.CanBeExhausted)
            {
                item.CurrentTurnsToExhausted -= 1;
                if(item.CurrentTurnsToExhausted == 0)
                {
                    inventoryController.Equipment.Unequip(EEquipmentSlot.LeftHand);
                    Gamemaster.Instance.gamelog.NewEntry(this, null, "Torch has been extinguished");
                }
            }
        }
        public void OnActionStart()
        {
            
        }
        public void OnActionEnd()
        {
            if(actionPoints == 0)
            {
                actorContext.Done = true;
            }
        }
        public void AddState(CharacterState state)
        {
            characterSheet.ApplyState(state);
        }

        public float GetPropertyValue(string propertyKey)
        {
            Dictionary<string, float> properties = new Dictionary<string, float>();
            
            foreach(EAttribute attribute in Enum.GetValues(typeof(EAttribute)))
            {
                string key = Enum.GetName(typeof(EAttribute), attribute);
                float value = characterSheet.Attributes[(int) attribute].Value;

                properties.Add(key, value);
            }

            foreach(EDerivedAttribute derived in Enum.GetValues(typeof(EDerivedAttribute)))
            {
                string key = Enum.GetName(typeof(EDerivedAttribute), derived);
                float value = characterSheet.DerivedAttributes[(int) derived].Value;

                properties.Add(key, value);
            }
            
            return properties[propertyKey];
        }

        public void IsDead()
        {
            if(alive) {
                alive = false;
                StartCoroutine(actorContext.PlayDeath());
            }
        }

        public void Damaged()
        {
            damagedEffects.ForEach(fx => 
            {
                Gamemaster.Instance.effectsManager.AddEffect(fx.gameObject.name, transform.position, Quaternion.identity, fx.delay);
            });

            if(characterSheet.DerivedAttributes[(int) EDerivedAttribute.Vitality].Value <= 0)
            {
                IsDead();
            }
            else
            {
                onDamaged?.Invoke();
            }
        }

        #region ISaveableObject
        public string GetObjectId()
        {
            return GetInstanceID().ToString();
        }

        public SaveableObject Save()
        {
            actorSaveableData = new ActorSaveableData
            {
                characterSheet = characterSheet,
                spellbook = spellbook,
                rotation = rotation,
                coordinates = CurrentTile.Tile.Coordinates
            };

            return new SaveableObject
            {
                name = poolTag,
                objectId = GetInstanceID().ToString(),
                type = GetType().ToString(),
                position = transform.position,
                rotation = transform.rotation,
                data = JsonUtility.ToJson(actorSaveableData)
            };
        }

        public void Load(SaveableObject saveData)
        {
            if (saveData.data != null) 
            {
                actorSaveableData = JsonUtility.FromJson<ActorSaveableData>(saveData.data);
                characterSheet = actorSaveableData.characterSheet;
                spellbook = actorSaveableData.spellbook;
                rotation = actorSaveableData.rotation;
            }
        }
        #endregion

        public Resistance[] GetResistances()
        {
            Resistance[] resistances = characterSheet.Resistances;
            inventoryController.Equipment.GetArmor().ForEach(armor => {
                armor.Resistances.ForEach(armorRes => 
                {
                    resistances[(int) armorRes.Element].Value += armorRes.Value;
                });
            });

            return resistances;
        }

        public void OnEquipmentChanged()
        {
            characterSheet.Set(EAttribute.Defence, 0);

            characterSheet.Resistances.ForEach(resistance => 
            {
                characterSheet.Set(resistance.Element, characterSheet.BaseResistances[(int) resistance.Element].Value);
            });
            
            inventoryController.Equipment.GetArmor().ForEach(armor => {
                armor.Resistances.ForEach(armorRes => 
                {
                    characterSheet.Change(armorRes.Element, armorRes.Value);
                });
                characterSheet.Change(EAttribute.Defence, armor.Defence);
            });

            characterSheet.Set(EAttribute.Might, 0);
            characterSheet.Set(EAttribute.Wisdom, 0);

            characterSheet.ResetToBase(EDerivedAttribute.Vitality);
            characterSheet.ResetToBase(EDerivedAttribute.Mana);
            MaxActionPoints = BaseActionPoints;

            inventoryController.Equipment.Slots.ForEach(slot => 
            {
                if(slot.Item.Benefits != null)
                {
                    MaxActionPoints += slot.Item.Benefits.actionPoints;
                    characterSheet.ChangeMax(EDerivedAttribute.Vitality, slot.Item.Benefits.maxHP);
                    characterSheet.ChangeMax(EDerivedAttribute.Mana, slot.Item.Benefits.maxMana);
                    characterSheet.Change(EAttribute.Might, slot.Item.Benefits.might);
                    characterSheet.Change(EAttribute.Wisdom, slot.Item.Benefits.wisdom);
                }
            });

            onEquipmentChangedEnd?.Invoke();
        }

        private void OnDrawGizmos() 
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position + DamageEffectAnchor, .1f);    
        }
    }

    public class ActorSaveableData
    {
        public CharacterSheet characterSheet;
        public Spellbook spellbook;
        public float rotation;
        public Vector3Int coordinates;
    }
}