using System;
using Sirenix.OdinInspector;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace Mazi.BuffetIAPurchasing
{
    [Serializable]
    public class BuffetIAPItem
    {
        public ProductType ProductType;
        public string ItemId;
        public int Quantity;
    }
}
