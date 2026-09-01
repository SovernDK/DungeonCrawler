using Sirenix.Utilities;
using UnityEngine;

namespace Crawler 
{
    public class LeverInteraction : Interactable, ISaveableObject, IPoolable
    {
        public bool toggled;
        public GameObject handle;
        public int gateId;
        public Gate gate;

        private Animator animator;
        private LeverData leverData;

        #region IPoolable Properties
        public string Name => Utility.GetObjectNameWithoutSuffix(gameObject);

        public GameObject Prefab => gameObject;

        public int AmountToPool => 5;

        public bool ShouldExpand => true;

        public LeverData LeverData { get => leverData; set => leverData = value; }
        #endregion

        public override void OnInit()
        {
            base.OnInit();

            animator = GetComponent<Animator>();
        }

        public override void OnInteractedWith(Player player)
        {
            base.OnInteractedWith(player);

            if(!toggled)
            {
                toggled = true;
                animator.SetTrigger("Toggled");
                FindObjectsOfType<Gate>().ForEach(gate => {
                    if(gate.id == leverData.gateId)
                    {
                        gate.Open();
                    } 
                });
            }
        }

        public string GetObjectId()
        {
            return GetInstanceID().ToString();
        }

        public void OnObjectDisbaled()
        {

        }

        public void OnObjectEnabled()
        {

        }

        public void OnSpawned()
        {

        }

        #region ISaveableObject
        public void Load(SaveableObject saveData)
        {
            if (saveData.data != null) 
            {
                LeverData = JsonUtility.FromJson<LeverData>(saveData.data);
            }
        }

        public SaveableObject Save()
        {
            LeverData leverData = new LeverData
            {
                toggled = toggled,
                gateId = gateId
            };

            return new SaveableObject
            {
                name = Name,
                objectId = GetInstanceID().ToString(),
                type = GetType().ToString(),
                position = transform.position,
                rotation = transform.rotation,
                data = JsonUtility.ToJson(leverData)
            };
        }
        #endregion
    }

    public class LeverData
    {
        public bool toggled;
        public int gateId;
    }
}