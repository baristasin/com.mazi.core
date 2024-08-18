using System;
using Sirenix.OdinInspector;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace Mazi.BuffetIAPurchasing
{
    public enum ItemSpecialty
    {
        NonSpecial,
        NoAds
    }

    [Serializable]
    public class BuffetIAPItem
    {
        public ProductType ProductType;
        public ItemSpecialty ItemSpecialty;
        public string ItemId;        
        public int Quantity;
    }
}
