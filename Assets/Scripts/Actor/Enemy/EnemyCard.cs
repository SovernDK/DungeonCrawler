using System;
using System.Collections.Generic;
using Sheet;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Data/Enemy")]
public class EnemyCard : ScriptableObject
{
    private int id;
    public int Id { get => id; set => id = value; }
    public List<EquipmentSlotProto> equipments;
    public List<AttributeProto> attributes;
    public List<DerivedAttributeProto> derivedAttributes;
    public List<Resistance> resistances;
}

[Serializable]
public class EquipmentSlotProto 
{
    public EEquipmentSlot slot;
    public ItemCard card;
}

[Serializable]
public class AttributeProto 
{
    public EAttribute attribute;
    public float value;
}

[Serializable]
public class DerivedAttributeProto
{
    public EDerivedAttribute attribute;
    public float value;
}