using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchInteraction : Interactable, ISaveableObject, IPoolable
{
    public ItemCard torch;
    public bool takenOut;
    public GameObject torchModel;
    private TorchData torchData;
    public TorchData TorchData { get => torchData; set => torchData = value; }

    #region IPoolable Properties
    public string Name => gameObject.name;

    public GameObject Prefab => gameObject;

    public int AmountToPool => 5;

    public bool ShouldExpand => true;

    #endregion

    public override void OnInteractedWith(Player player)
    {
        base.OnInteractedWith(player);
        if(player.Actor.InventoryController.Inventory.AddItem(torch.Item.Copy()))
        {
            TorchData.takenOut = true;
            torchModel.SetActive(false);
        }
    }

    public string GetObjectId()
    {
        return GetInstanceID().ToString();
    }

    #region IPoolable Members
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

    public SaveableObject Save()
    {
        TorchData = new TorchData()
        {
            takenOut = takenOut
        };

        return new SaveableObject
        {
            name = "Torch",
            objectId = GetInstanceID().ToString(),
            type = GetType().ToString(),
            position = transform.position,
            rotation = transform.rotation,
            data = JsonUtility.ToJson(TorchData)
        };
    }

    void ISaveableObject.Load(SaveableObject saveData)
    {
        if (saveData.data != null) 
        {
            TorchData = JsonUtility.FromJson<TorchData>(saveData.data);
        }

        if(TorchData.takenOut)
        {
            torchModel.SetActive(false);
        }
    }
}

public class TorchData
{
    public bool takenOut;
}
