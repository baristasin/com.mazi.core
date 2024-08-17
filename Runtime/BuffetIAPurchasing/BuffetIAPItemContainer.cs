using System.Collections;
using System.Collections.Generic;
using Mazi.BuffetIAPurchasing;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffetIAPItemContainer", menuName = "ScriptableObjects/BuffetIAPItemContainer", order = 1)]
public class BuffetIAPItemContainer : ScriptableObject
{
    public List<BuffetIAPItem> BuffetIAPItems;

    [Button]
    public void AddItem(BuffetIAPItem buffetIAPItem)
    {
        BuffetIAPItems.Add(buffetIAPItem);
    }
}
