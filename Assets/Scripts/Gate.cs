using DG.Tweening;
using UnityEngine;

namespace Crawler 
{
    public class Gate : MonoBehaviour, ISaveableObject, IPoolable
    {
        public GameObject gate;
        public bool opened;
        public int id;

        private GateData gateData;

        public string Name => Utility.GetObjectNameWithoutSuffix(gameObject);

        public GameObject Prefab => gameObject;

        public int AmountToPool => 5;

        public bool ShouldExpand => true;

        public GateData GateData { get => gateData; set => gateData = value; }

        public void Open()
        {
            Vector3 pos = transform.position + Vector3.up * 2;
            gate.transform.DOMove(pos, 1f);
        }

        #region ISaveable 
        public string GetObjectId()
        {
            return GetInstanceID().ToString();
        }

        public SaveableObject Save()
        {
            GateData data = new GateData
            {
                opened = opened,
                id = id
            };

            return new SaveableObject
            {
                name = Name,
                objectId = GetInstanceID().ToString(),
                type = GetType().ToString(),
                position = transform.position,
                rotation = transform.rotation,
                data = JsonUtility.ToJson(data)
            };
        }

        public void Load(SaveableObject saveData)
        {
            if (saveData.data != null) 
            {
                GateData = JsonUtility.FromJson<GateData>(saveData.data);
            }
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

    public class GateData 
    {
        public bool opened;
        public int id;
    } 
}