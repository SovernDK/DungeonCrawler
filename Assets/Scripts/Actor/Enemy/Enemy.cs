using System.Collections;
using System.Collections.Generic;
using Crawler;
using Crawler.Actors;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Actor))]
public class Enemy : Interactable, IActorContext, IPoolable
{
    private string actorName = "Enemy";
    [TabGroup("Actor")] 
    public Actor actor;
    [TabGroup("Enemy")] [InlineEditor] 
    public EnemyCard card;
    [TabGroup("Enemy")] 
    public Animator animator;
    [TabGroup("Loot")] [InlineEditor] 
    public List<ItemCard> loot;
    [TabGroup("Loot")] 
    public Transform lootSpawnPoint;
    [TabGroup("Actor")] 
    public float movementSpeed;
    [TabGroup("Actor")] 
    public float knockbackSpeed;
    [TabGroup("Actor")] 
    public float rotationSpeed;
    [TabGroup("AI")] 
    public List<AgentAction> agentActions;
    [TabGroup("AI")] 
    public Actor follow;
    [TabGroup("FX")] 
    public List<Renderer> renderers;
    [TabGroup("FX")] [InlineProperty] 
    public EffectPrototype moveEffect;

    [SerializeField] [TabGroup("AI")] private float actionWeightGain;
    [SerializeField] [TabGroup("AI")] private float actionWeightLoss;
    [SerializeField] [TabGroup("AI")] private float actionWeightLimit;

    private MainContext mainContext;

    private bool done;
    private bool busy;
    private bool deathAnimationEnded;
    private bool attackAnimationEnded;

    public bool Done { get => done; set => done = value; }
    public bool Alive => actor.Alive;
    public MainContext MainContext => mainContext;
    public string ActorName { get => actorName; set => actorName = value; }
    public bool DeathAnimationEnded { get => deathAnimationEnded; set => deathAnimationEnded = value; }
    public bool AttackAnimationEnded { get => attackAnimationEnded; set => attackAnimationEnded = value; }
    public bool Busy { get => busy; set => busy = value; }

    #region IPoolable Properties
    public string Name => gameObject.name;
    public GameObject Prefab => gameObject;
    public int AmountToPool => 2;
    public bool ShouldExpand => true;
    #endregion

    #region IActorContext
    public virtual void Initialize(MainContext context)
    {
        mainContext = context;

        actor.Initialize(mainContext, this);
        actor.OnDamaged.AddListener(GetHit);

        foreach(EquipmentSlotProto next in card.equipments)
        {
            actor.InventoryController.Equipment.Equip(next.slot, next.card.Item);
        }

        foreach(AttributeProto next in card.attributes)
        {
            actor.CharacterSheet.Set(next.attribute, next.value);
        }

        foreach(DerivedAttributeProto next in card.derivedAttributes)
        {
            actor.CharacterSheet.SetMax(next.attribute, next.value);
            // actor.CharacterSheet.Set(next.attribute, next.value);
        }

        card.resistances.ForEach(resistance => 
        {
            actor.CharacterSheet.Resistances[(int) resistance.Element].Value = resistance.Value;
        });

        actor.Target = mainContext.player.Actor;
    }

    public IEnumerator PlayMove(TileController tileController)
    {
        if(moveEffect.gameObject != null)
            Gamemaster.Instance.effectsManager.AddEffect(moveEffect.gameObject.name, transform.position, Quaternion.identity, moveEffect.delay);
        actor.transform.DOMove(tileController.GetActorStand(), movementSpeed).SetEase(Ease.InOutQuad); 
        yield return null;
    }

    public IEnumerator PlayKnockback(TileController tileController)
    {
        yield return actor.transform.DOMove(tileController.GetActorStand(), knockbackSpeed).SetEase(Ease.InOutQuad).WaitForCompletion();
    }

    public IEnumerator PlayRotate(float newRotation)
    {   
        actor.transform.DORotate(new Vector3(0, newRotation * 90, 0), rotationSpeed).SetEase(Ease.InOutQuad);
        yield return null;
    }

    public IEnumerator PlayGetHit()
    {
        animator.SetTrigger("GetHit");
        yield return new WaitUntil(() => Busy);
        Busy = false;
    }

    public IEnumerator OnTurnStart()
    {
        actor.OnTurnStart();
        yield return null;
    }
    public IEnumerator OnTurnEnd()
    {
        actor.OnTurnEnd();
        yield return null;
    }
    public IEnumerator OnActionStart()
    {
        actor.OnActionStart();
        yield break;
    }
    public IEnumerator OnActionEnd()
    {
        actor.OnActionEnd();
        yield break;
    }

    public IEnumerator Act()
    {
        if(Alive) 
        {
            DetermineRotationTowardTarget();

            List<AgentAction> availableActions = new List<AgentAction>();
            
            agentActions.ForEach(action => 
            {
                if(action.Check(actor))
                {
                    availableActions.Add(action);
                }
            });

            availableActions.Sort((obj1, obj2) => (obj2.Priority - obj2.Weight).CompareTo(obj1.Priority - obj1.Weight));

            if(availableActions.Count > 0) 
            {
                AgentAction action = availableActions[0];

                actor.CurrentSpell = (action.Spell != null) ? action.Spell.Spell : null;
                mainContext.gameFlow.AddCommand(action.Command, actor);
                action.Weight += actionWeightGain;

                availableActions.RemoveAt(0);
                availableActions.ForEach(action => 
                {
                    action.Weight = Mathf.Clamp(action.Weight - actionWeightLoss, 0, 100);
                    if(action.Weight >= actionWeightLimit) { //Reset weights if they get to big
                        action.Weight = 0;
                    }
                });
            }
            else
            {
                actor.ActionPoints = 0;
                done = true;
            }
        }

        yield return null;
    }

    private void DetermineRotationTowardTarget()
    {
        //TODO: What if enemy is diagonally positioned toward player?
        Vector3 direction = actor.Target.CurrentTile.Tile.Coordinates - actor.CurrentTile.Tile.Coordinates;
        
        direction = direction.normalized;

        if(direction == Vector3.forward)
        {
            actor.Rotation = 3;
            StartCoroutine(PlayRotate(actor.Rotation));
        }
        else if(direction == Vector3.back)
        {
            actor.Rotation = 1;
            StartCoroutine(PlayRotate(actor.Rotation));
        }
        else if(direction == Vector3.left)
        {
            actor.Rotation = 2;
            StartCoroutine(PlayRotate(actor.Rotation));
        }
        else if(direction == Vector3.right)
        {
            actor.Rotation = 0;
            StartCoroutine(PlayRotate(actor.Rotation));
        }
    }

    public IEnumerator PlayAttack()
    {
        animator.SetTrigger("Attack");
        yield return new WaitUntil(() => attackAnimationEnded);
        attackAnimationEnded = false;
    }

    public IEnumerator PlaySpellCasting()
    {
        animator.SetTrigger("Spellcasting");
        yield return null;
    }

    [Button("PlayeDeath")]
    public IEnumerator PlayDeath()
    {
        animator.SetTrigger("Death");
        GetComponent<Actor>().CurrentTile.Tile.IsOccupied = false;
        
        LootableData data = new LootableData();
        
        List<int> ids = new List<int>();
        loot.ForEach(card => {
            ids.Add(card.Item.Id);
        });
        data.items = ids.ToArray();

        yield return new WaitUntil(() => deathAnimationEnded);

        if(loot.Count > 0) 
        {
            GameObject bag = PoolManager.Instance.SpawnFromPool("Bag", lootSpawnPoint.position, Quaternion.identity);
            bag.GetComponent<Lootable>().LootableData = data;
        }

        deathAnimationEnded = false;
        gameObject.SetActive(false);
    }
    #endregion

    public virtual void MoveTowardTarget(Vector3Int start, Vector3Int goal)
    {
        Queue<Tile> path = mainContext.grid.Pathfinding.CreatePath(start, goal);
    
        Tile next = path.Dequeue();
        if(next.Coordinates == start) {
            next = path.Dequeue();
        }
        else if(next.Coordinates == goal) 
        {
            return;
        }

        Vector3Int direction = actor.CurrentTile.Tile.Coordinates - next.Coordinates;

        if(direction == Vector3Int.forward)
        {
            mainContext.gameFlow.AddCommand("MoveForward", actor);
        }
        else if(direction == Vector3Int.right)
        {
            mainContext.gameFlow.AddCommand("MoveRight", actor);
        }
        else if(direction == Vector3Int.back)
        {
            mainContext.gameFlow.AddCommand("MoveBack", actor);
        }
        else if(direction == Vector3Int.left)
        {
            mainContext.gameFlow.AddCommand("MoveLeft", actor);
        }
    }

    public void Knockback(int strength, Vector3Int direction)
    {
        if(direction == Vector3.forward)
        {
            mainContext.gameFlow.AddCommand("Knockforward", actor);
        }
        else if(direction == Vector3.back)
        {
            mainContext.gameFlow.AddCommand("Knockback", actor);
        }
        else if(direction == Vector3.left)
        {
            mainContext.gameFlow.AddCommand("Knockleft", actor);
        }
        else if(direction == Vector3Int.right)
        {
            mainContext.gameFlow.AddCommand("Knockright", actor);
        }
    }

    public virtual void GetHit()
    {
        mainContext.gameFlow.AddCommand("GetHit", actor);
    }

    #region Interactable
    public override void OnInit()
    {
        base.OnInit();

        transform.rotation = Quaternion.Euler(Vector3.up * actor.Rotation * 90);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Gamemaster.Instance.gamePreferences.SetCursor("Attack");
    }

    public override void OnExit()
    {
        base.OnExit();
        Gamemaster.Instance.gamePreferences.SetCursor("Default");
    }

    public override void OnInteractedWith(Player player)
    {
        base.OnInteractedWith(player);
    }
    #endregion
    
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