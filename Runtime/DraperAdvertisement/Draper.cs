using Assets.Mazi.Interfaces;
using Mazi.Mediations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mazi.DraperAdvertisement
{
    public class Draper : MonoBehaviour
    {
        public static Draper Instance;

        private IMediationType _mediationType;

        [SerializeField] private IronSourceMediation _ironSourceMediation;

        [SerializeField] private bool _isInterstitialActive;
        [SerializeField] private bool _isBannerActive;
        [SerializeField] private bool _isRewardedActive;

        private Action _rewardedCallback;

        private void Awake()
        {
            _mediationType = _ironSourceMediation;

            _ironSourceMediation.Initialize();

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

            #region LoadingAds
            if (_isInterstitialActive)
            {
                LoadInterstitial();
            }
            if (_isRewardedActive)
            {
                LoadRewarded();
            _mediationType.OnRewardedVideoDismissed += RewardedVideoDismissed;
            }
            if (_isBannerActive)
            {
                LoadBanner();
            }
            #endregion
        }

        private void OnDestroy()
        {
            _mediationType.OnRewardedVideoDismissed -= RewardedVideoDismissed;
        }

        private void RewardedVideoDismissed(bool isSuccess)
        {
            LoadRewarded();

            if (isSuccess)
            {
                _rewardedCallback?.Invoke();
            }
            else
            {
                Debug.Log($"Rewarded Video failed");
            }
        }

        public void LoadInterstitial()
        {
            _mediationType.LoadInterstitial();
            //Request Ads
        }
        public void LoadBanner()
        {
            //Request Ads
        }
        public void LoadRewarded()
        {
            _mediationType.LoadRewarded();
            //Request Ads
        }

        public void ShowInterstitial()
        {
            _mediationType.ShowInterstitial();
            LoadInterstitial();
        }
        public void ShowBanner()
        {
        }
        public void ShowRewarded(Action rewardedCallback)
        {
            _rewardedCallback = rewardedCallback;
            _mediationType.ShowRewarded();
        }
    }
}