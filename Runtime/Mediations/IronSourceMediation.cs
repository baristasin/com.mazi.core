using Assets.Mazi.Interfaces;
using System;
using System.Collections;
using UnityEngine;

namespace Mazi.Mediations
{
    public class IronSourceMediation : MonoBehaviour, IMediationType
    {
        public event Action<bool> OnRewardedVideoDismissed;
        public event Action<bool> OnRewardedLoadProcessCompleted;
        public event Action<bool> OnInterstitialLoadProcessCompleted;
        public event Action<bool> OnBannerLoadProcessCompleted;

        private IronSourceConnectionContainer _ironSourceConnectionContainer;

        public void Initialize()
        {
            _ironSourceConnectionContainer = Resources.Load<IronSourceConnectionContainer>("MediationContainers/IronSourceConnectionContainer");

            IronSource.Agent.init(_ironSourceConnectionContainer.IronSourceId, IronSourceAdUnits.REWARDED_VIDEO, IronSourceAdUnits.INTERSTITIAL, IronSourceAdUnits.BANNER);
                        
            IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoOnAdShowFailedEvent;
            IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;

            IronSourceRewardedVideoEvents.onAdReadyEvent += RewardedVideoReadyForShow;
            IronSourceRewardedVideoEvents.onAdLoadFailedEvent += RewardedVideoFailedToLoad;

            IronSourceBannerEvents.onAdLoadedEvent += BannerReadyForShow;
            IronSourceBannerEvents.onAdLoadFailedEvent += BannerFailedToLoad;

            IronSourceInterstitialEvents.onAdReadyEvent += InterstitialReadyForShow;
            IronSourceInterstitialEvents.onAdLoadFailedEvent += InterstitialFailedToLoad;

            IronSource.Agent.validateIntegration();      
        }

        private void InterstitialReadyForShow(IronSourceAdInfo obj)
        {
            OnInterstitialLoadProcessCompleted?.Invoke(true);
        }

        private void InterstitialFailedToLoad(IronSourceError obj)
        {
            OnInterstitialLoadProcessCompleted?.Invoke(false);
        }
      
        private void BannerReadyForShow(IronSourceAdInfo obj)
        {
            OnBannerLoadProcessCompleted?.Invoke(true);
        }

        private void BannerFailedToLoad(IronSourceError obj)
        {
            OnBannerLoadProcessCompleted?.Invoke(false);
        }

        private void RewardedVideoReadyForShow(IronSourceAdInfo obj)
        {
            OnRewardedLoadProcessCompleted?.Invoke(true);
        }

        private void RewardedVideoFailedToLoad(IronSourceError obj)
        {
            OnRewardedLoadProcessCompleted?.Invoke(false);
        }

        void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
        {
            OnRewardedVideoDismissed?.Invoke(true);
        }

        void RewardedVideoOnAdShowFailedEvent(IronSourceError error, IronSourceAdInfo adInfo)
        {
            OnRewardedVideoDismissed?.Invoke(false);
        }               

        public void LoadRewarded()
        {
            IronSource.Agent.loadRewardedVideo();
        }

        public void ShowRewarded()
        {
            IronSource.Agent.showRewardedVideo();
        }

        public void LoadInterstitial()
        {
            IronSource.Agent.loadInterstitial();
        }

        public void ShowInterstitial()
        {
            IronSource.Agent.showInterstitial();
        }

        public void LoadBanner()
        {
            IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
        }

        public void ShowBanner()
        {
            IronSource.Agent.displayBanner();
        }

        private void OnDestroy()
        {
            IronSourceRewardedVideoEvents.onAdShowFailedEvent -= RewardedVideoOnAdShowFailedEvent;
            IronSourceRewardedVideoEvents.onAdRewardedEvent -= RewardedVideoOnAdRewardedEvent;

            IronSourceRewardedVideoEvents.onAdReadyEvent -= RewardedVideoReadyForShow;
            IronSourceRewardedVideoEvents.onAdLoadFailedEvent -= RewardedVideoFailedToLoad;

            IronSourceBannerEvents.onAdLoadedEvent -= BannerReadyForShow;
            IronSourceBannerEvents.onAdLoadFailedEvent -= BannerFailedToLoad;

            IronSourceInterstitialEvents.onAdReadyEvent -= InterstitialReadyForShow;
            IronSourceInterstitialEvents.onAdLoadFailedEvent -= InterstitialFailedToLoad;
        }
    }
}