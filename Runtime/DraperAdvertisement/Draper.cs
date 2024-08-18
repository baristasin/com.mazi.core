using Assets.Mazi.Interfaces;
using Mazi.BuffetIAPurchasing;
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

        private bool _isInterstitialReadyToShow;
        private bool _isRewardedReadyToShow;
        private bool _isBannerReadyToShow;

        private bool _isNoAdsActive;
        private Action _rewardedCallback;

        private void Awake()
        {
            _mediationType = _ironSourceMediation;

            _ironSourceMediation.Initialize();

            _mediationType.OnInterstitialLoadProcessCompleted += InterstitialLoadProcessCompleted;
            _mediationType.OnRewardedLoadProcessCompleted += RewardedLoadProcessCompleted;
            _mediationType.OnBannerLoadProcessCompleted += BannerLoadProcessCompleted;

            Buffet.Instance.OnNoAdsBought += NoAdsBought;

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
            if (_isInterstitialActive && !_isNoAdsActive)
            {
                LoadInterstitial();
            }
            if (_isRewardedActive)
            {
                LoadRewarded();
                _mediationType.OnRewardedVideoDismissed += RewardedVideoDismissed;
            }
            if (_isBannerActive && !_isNoAdsActive)
            {
                LoadBanner();
            }
            #endregion
        }

        private void NoAdsBought()
        {

        }

        private void BannerLoadProcessCompleted(bool success)
        {
            _isBannerReadyToShow = success;
            if (_isBannerReadyToShow)
            {
                ShowBanner();
            }
            else
            {
                LoadBanner();
            }
        }

        private void RewardedLoadProcessCompleted(bool success)
        {
            _isRewardedReadyToShow = success;
            if (!_isRewardedReadyToShow)
            {
                LoadRewarded();
            }
        }

        private void InterstitialLoadProcessCompleted(bool success)
        {
            _isInterstitialReadyToShow = success;
            if (!_isRewardedReadyToShow)
            {
                LoadInterstitial();
            }
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

        private void LoadInterstitial()
        {
            _mediationType.LoadInterstitial();
            Debug.Log("Inters Request");
        }

        private void LoadBanner()
        {
            if (!Buffet.Instance.IsNoAdsBought)
            {
                _mediationType.LoadBanner();
                Debug.Log("Banner Request");
            }
        }

        private void LoadRewarded()
        {
            _mediationType.LoadRewarded();
            Debug.Log("Rewarded Request");
        }

        private void ShowBanner()
        {
            _mediationType.ShowBanner();
            LoadBanner();
        }

        public void ShowInterstitial()
        {
            if (_isInterstitialReadyToShow && !Buffet.Instance.IsNoAdsBought)
            {
                _mediationType.ShowInterstitial();
                _isInterstitialReadyToShow = false;

                LoadInterstitial();
            }
        }

        public void ShowRewarded(Action rewardedCallback)
        {
            if (_isRewardedReadyToShow)
            {
                _mediationType.ShowRewarded();
                _rewardedCallback = rewardedCallback;
                _isRewardedReadyToShow = false;

                LoadRewarded();
            }
        }

        private void OnDestroy()
        {
            _mediationType.OnRewardedVideoDismissed -= RewardedVideoDismissed;
            _mediationType.OnInterstitialLoadProcessCompleted -= InterstitialLoadProcessCompleted;
            _mediationType.OnRewardedLoadProcessCompleted -= RewardedLoadProcessCompleted;
            _mediationType.OnBannerLoadProcessCompleted -= BannerLoadProcessCompleted;
        }
    }
}