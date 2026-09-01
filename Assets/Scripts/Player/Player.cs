using System.Collections;
using Sirenix.OdinInspector;
using Crawler.Actors;
using Crawler.FSM;
using Sheet;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using Crawler;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IActorContext, IPoolable
{
    #region Public
    [TabGroup("Interactions")] 
    public LayerMask interactableLayer;

    [TabGroup("Interactions")] 
    public LayerMask collidingLayers;

    [TabGroup("Interactions")] 
    public float interactionRange;

    [TabGroup("Actor")] 
    public float movementSpeed;

    [TabGroup("Actor")] 
    public float rotationSpeed;
    
    [TabGroup("FX")] [InlineProperty] 
    public EffectPrototype footsteps;
    
    [TabGroup("FX")] [InlineProperty] 
    public EffectPrototype damaged;
    #endregion

    #region Private
    private string actorName = "Player";
    [SerializeField] [TabGroup("Visuals")] private Transform rightHandItems;
    [SerializeField] [TabGroup("Visuals")] private Transform leftHandItems;
    [SerializeField] [InlineProperty] [TabGroup("FSM")] private PlayerStateMachine fsm;
    [SerializeField] [ReadOnly] [TitleGroup("Read only")] private EAimContext aimContext;
    [SerializeField] [ReadOnly] [TitleGroup("Read only")] private Interactable currentInteractable;
    private Actor actor;
    private Spell currentSpell;
    private MainContext mainContext;
    private bool done;
    private bool weaponAnimationEnded = false;
    #endregion

    #region Getters and setters
    public Actor Actor { get => actor; set => actor = value; }
    public PlayerStateMachine Fsm { get => fsm; set => fsm = value; }
    public Interactable CurrentInteractable { get => currentInteractable; set => currentInteractable = value; }
    public Spell CurrentSpell { get => currentSpell; set => currentSpell = value; }
    public EAimContext AimContext { get => aimContext; set => aimContext = value; }
    public bool Done { get => done; set => done = value; }
    public bool Alive => actor.Alive;
    public MainContext MainContext => mainContext;
    public string ActorName { get => actorName; set => actorName = value; }
    public bool WeaponAnimationEnded { get => weaponAnimationEnded; set => weaponAnimationEnded = value; }
    public bool Busy { get => false; set => value = false; }
    #endregion

    #region Delegates
    public delegate void Moved();
    public event Moved onMoved;
    #endregion

    #region IPoolable Properties
    public string Name => gameObject.name;
    public GameObject Prefab => gameObject;
    public int AmountToPool => 1;
    public bool ShouldExpand => false;

    #endregion


    public void Initialize(MainContext context)
    {   
        mainContext = context;

        actor = GetComponent<Actor>();
        actor.Initialize(context, this);
        
        fsm = new PlayerStateMachine();
        fsm.Init(this);
        fsm.SetState((int) EPlayerState.Actor);

        Actor.InventoryController.Equipment.onEquiped += DisplayRightHand;
        Actor.InventoryController.Equipment.onEquiped += DisplayLeftHand;
        onMoved += DetectTraps;
        actor.OnDamaged.AddListener(GetHit);

    }

    public void DisplayRightHand()
    {
        EquipmentSlot slot = Actor.InventoryController.Equipment.Slots[(int) EEquipmentSlot.RightHand];
        rightHandItems.GetComponent<HandVisualHandler>().HideAll();

        if(slot.Item.Id != -1)
        {
            rightHandItems.GetComponent<HandVisualHandler>().Show(slot.Item.WeaponType);
        }
    }

    public void DisplayLeftHand()
    {
        EquipmentSlot slot = Actor.InventoryController.Equipment.Slots[(int) EEquipmentSlot.LeftHand];
        leftHandItems.GetComponent<HandVisualHandler>().HideAll();
        if(slot.Item.Id != -1)
        {
            leftHandItems.GetComponent<HandVisualHandler>().Show(slot.Item.Id);
        }
    }

    public void DetectTraps()
    {
        List<Trap> traps = mainContext.grid.FindAllTrapsInRange(actor.CurrentTile.Tile.Coordinates, 2);
        traps.ForEach(trap => {
            if(trap.TrapData.discoveryLevel <= Actor.CharacterSheet.Attributes[(int) EAttribute.Wisdom].Value)
            {
                trap.Discover();
            }
        });
    }

    private void GetHit()
    {
        Camera.main.DOShakePosition(.3f, .10f, 10, 90, true, ShakeRandomnessMode.Harmonic);
        Gamemaster.Instance.effectsManager.AddEffect(damaged.gameObject.name, transform.position, Quaternion.identity, footsteps.delay);
    }

    public void CastSpell(int id)
    {
        mainContext.mainScene.CloseWindow("Spellbook");
        currentSpell = mainContext.database.spells.Find(card => card.Spell.Id == id).Spell.Copy();
        actor.CurrentSpell = currentSpell;

        if(currentSpell.Actions[0].Target == ETarget.Self)
        {
            (Gamemaster.Instance.context as MainContext).gameFlow.AddCommand("SpellCast", actor);
            fsm.isActing = true;
        }
        else
        {
            aimContext = EAimContext.Spell;
            fsm.SetState((int) EPlayerState.Aim);
        }
    }

    #region Inputs and Interactions
    public void Interactor(int range)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        bool overUi = EventSystem.current.IsPointerOverGameObject();

        int layers = interactableLayer | collidingLayers;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layers) && !overUi)
        {
            if(collidingLayers == (collidingLayers | (1 << hit.collider.gameObject.layer)))
            {
                currentInteractable = null;
                return;
            }

            Interactable interactable = hit.collider.gameObject.GetComponent<Interactable>();
            
            Item weapon = actor.InventoryController.Equipment.Slots[(int) EEquipmentSlot.RightHand].Item;
            if(interactable.CompareTag("Enemy"))
            {
                if(interactable.TryGetComponent(out Enemy enemy))
                {
                    float distance = Vector3Int.Distance(enemy.actor.CurrentTile.Tile.Coordinates, actor.CurrentTile.Tile.Coordinates);
                    Vector3 direction = enemy.actor.CurrentTile.Tile.Coordinates - actor.CurrentTile.Tile.Coordinates;
                    direction = direction.normalized;
                    if(distance > range && distance > 1) 
                    {
                        currentInteractable = null;
                        return;
                    }
                    if(direction != transform.forward)
                    {
                        currentInteractable = null;
                        return;
                    }
                }
            }
            else if(Vector3.Distance(interactable.transform.position, transform.position) > interactionRange) return;

            if (interactable != null)
            {
                if (currentInteractable != interactable)
                {
                    if (currentInteractable != null)
                    {
                        currentInteractable.OnExit();
                    }

                    currentInteractable = interactable;
                    currentInteractable.OnEnter();
                }
            }
        }
        else
        {
            if (currentInteractable != null)
            {
                currentInteractable.OnExit();
                currentInteractable = null;
            }
        }
    }

    public IEnumerator OnTurnStart()
    {
        Gamemaster.Instance.inputManager.GameInputs.PlayerControl.OpenInventoryWindow.performed += OnInventoryOpened;
        Gamemaster.Instance.inputManager.GameInputs.PlayerControl.OpenSpellbookWindow.performed += OnSpellbookOpened;

        actor.OnTurnStart();
        
        yield return null;
    }

    public IEnumerator OnTurnEnd()
    {
        Gamemaster.Instance.inputManager.GameInputs.PlayerControl.OpenInventoryWindow.performed -= OnInventoryOpened;
        Gamemaster.Instance.inputManager.GameInputs.PlayerControl.OpenSpellbookWindow.performed -= OnSpellbookOpened;

        actor.OnTurnEnd();
        fsm.SetState((int) EPlayerState.Actor);
        // mainContext.mainScene.CloseAllWindows();

        yield return null;
    }

    public IEnumerator OnActionStart()
    {
        actor.OnActionStart();
        yield return null;
    }

    public IEnumerator OnActionEnd()
    {
        actor.OnActionEnd();
        yield return null;
    }

    public void Inputs()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            actor.CharacterSheet.Change(EDerivedAttribute.Vitality, -10);
            CharacterState state = mainContext.database.states.Find(state => state.State.Id == 1).State;
            actor.AddState(state);
            fsm.isActing = true;
        }

        if(Input.GetKeyDown(KeyCode.F1))
        {
            mainContext.mainScene.ToggleWindow("Dev");
        }

        if(Gamemaster.Instance.inputManager.Movement.y == 1)
        {
            mainContext.gameFlow.AddCommand("MoveForward", actor);
            fsm.isActing = true;
        }
        else if(Gamemaster.Instance.inputManager.Movement.y == -1)
        {
            mainContext.gameFlow.AddCommand("MoveBack", actor);
            fsm.isActing = true;
        }
        else if(Gamemaster.Instance.inputManager.Movement.x == -1)
        {
            mainContext.gameFlow.AddCommand("MoveLeft", actor);
            fsm.isActing = true;
        }
        else if(Gamemaster.Instance.inputManager.Movement.x == 1)
        {
            mainContext.gameFlow.AddCommand("MoveRight", actor);
            fsm.isActing = true;
        }
        else if(Gamemaster.Instance.inputManager.Rotation == 1)
        {
            mainContext.gameFlow.AddCommand("RotateRight", actor);
            fsm.isActing = true;
        }
        else if(Gamemaster.Instance.inputManager.Rotation == -1)
        {
            mainContext.gameFlow.AddCommand("RotateLeft", actor);
            fsm.isActing = true;
        }
    }
    #endregion

    private void OnSpellbookOpened(InputAction.CallbackContext context)
    {
        mainContext.mainScene.ToggleWindow("Spellbook");
    }
    
    private void OnInventoryOpened(InputAction.CallbackContext context)
    {
        mainContext.mainScene.ToggleWindow("Inventory");
        if(!mainContext.mainScene.IsWindowOpen("Inventory"))
            mainContext.mainScene.infoPanel.gameObject.SetActive(false);
    }

    public void LookAtEnemy()
    {
        // Camera.main.
    }

    public void ResetCamera()
    {
        // Camera.main.
    }

    #region IActorContext
    public IEnumerator PlayAttack()
    {
        Animator animator = rightHandItems.GetComponent<Animator>();
        EquipmentSlot slot = Actor.InventoryController.Equipment.Slots[(int) EEquipmentSlot.RightHand];

        animator.SetFloat("WeaponId", (int) slot.Item.WeaponType);
        animator.SetTrigger("Attack");

        Camera.main.DOShakePosition(.25f, .15f, 0, 0, false, ShakeRandomnessMode.Full);
        yield return new WaitUntil(() => weaponAnimationEnded);

        weaponAnimationEnded = false;
    }

    public IEnumerator PlayMove(TileController tile)
    {
        Gamemaster.Instance.effectsManager.AddEffect(footsteps.gameObject.name, transform.position, Quaternion.identity, footsteps.delay);
        yield return actor.transform.DOMove(tile.GetActorStand(), movementSpeed).SetEase(Ease.InOutQuad).WaitForCompletion();
        onMoved?.Invoke();
    }

    public IEnumerator PlayRotate(float newRotation)
    {   
        yield return actor.transform.DORotate(new Vector3(0, newRotation * 90, 0), rotationSpeed).SetEase(Ease.InOutQuad).WaitForCompletion();
    }

    public IEnumerator PlayMoveBlocked()
    {
        yield return Camera.main.DOShakePosition(.5f, .15f, 10, 90, true, ShakeRandomnessMode.Harmonic).WaitForCompletion();
    }

    public IEnumerator Act()
    {
        fsm.isActing = false;

        yield return StartCoroutine(fsm.Update());
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

public enum EAimContext 
{
    Spell
}
