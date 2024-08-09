using Facebook.Unity;
using GameAnalyticsSDK;
using System;
using System.Collections;
using UnityEngine;

namespace Mazi.GadgetAnalytics
{
    public class Gadget : MonoBehaviour
    {
        public static Gadget Instance;

        private void Awake()
        {
            #region Singleton
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(Instance);
            }
            else
            {
                Destroy(gameObject);
            }
            #endregion

            GameAnalytics_Initialize();

            Facebook_Initialize();
        }

        private void Facebook_Initialize()
        {
            if (FB.IsInitialized)
            {
                FB.ActivateApp();
            }
            else
            {
                //Handle FB.Init
                FB.Init(() => {
                    FB.ActivateApp();
                });
            }
        }

        private void GameAnalytics_Initialize()
        {
            GameAnalytics.Initialize();
        }

        public void SendLevelEndEvent(bool isSuccess, int levelId)
        {
            if (isSuccess)
            {
                TrackEvent("LevelCompleted", levelId);
            }
            else
            {
                TrackEvent("LevelFailed");
            }
        }

        public void TrackEvent(string eventName, float eventValue = 0f)
        {
            if (eventValue == 0f)
            {
                GameAnalytics.NewDesignEvent(eventName);
            }
            else
            {
                GameAnalytics.NewDesignEvent(eventName, eventValue);
            }
        }
    }
}