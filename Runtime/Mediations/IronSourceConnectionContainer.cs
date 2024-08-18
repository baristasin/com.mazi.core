using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mazi.Mediations
{
    [CreateAssetMenu(fileName = "IronSourceConnectionContainer", menuName = "ScriptableObjects/IronSourceConnectionContainer", order = 1)]
    public class IronSourceConnectionContainer : ScriptableObject
    {
        public string IronSourceId;
    }
}