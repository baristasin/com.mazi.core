using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mazi.UserDataContainer
{
    public static class UserData
    {
        public static bool IsNoAdsBought
        {
            get
            {
                return PlayerPrefs.GetInt("IsNoAdsBought", 0) == 0 ? false : true;
            }
            set
            {
                PlayerPrefs.SetInt("IsNoAdsBought", value == true ? 1 : 0);
            }
        }
    }
}