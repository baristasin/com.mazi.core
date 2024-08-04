using Assets.Mazi.Interfaces;
using System;
using System.Collections;
using UnityEngine;

namespace Mazi.Mediations
{
    public class IronSourceMediation : MonoBehaviour, IMediationType
    {
        public event Action<bool> OnRewardedVideoDismissed;

        public void Initialize()
        {

            IronSource.Agent.init("135a6c241", IronSourceAdUnits.REWARDED_VIDEO, IronSourceAdUnits.INTERSTITIAL, IronSourceAdUnits.BANNER);

            IronSource.Agent.validateIntegration();

            IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
            IronSourceEvents.onRewardedVideoAdClickedEvent += RewardedVideoAdClickedEvent;
            IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent;
            IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += RewardedVideoAvailabilityChangedEvent;
            IronSourceEvents.onRewardedVideoAdStartedEvent += RewardedVideoAdStartedEvent;
            IronSourceEvents.onRewardedVideoAdEndedEvent += RewardedVideoAdEndedEvent;
            IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent;
            IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedVideoAdShowFailedEvent;


            IronSourceEvents.onInterstitialAdReadyEvent += InterstitialAdReadyEvent;
            IronSourceEvents.onInterstitialAdLoadFailedEvent += OnInterstitialAdLoadFailedEvent;           
        }

        private void OnInterstitialAdLoadFailedEvent(IronSourceError error)
        {

        }

        private void InterstitialAdReadyEvent()
        {

        }

        //Invoked when the RewardedVideo ad view has opened.
        //Your Activity will lose focus. Please avoid performing heavy 
        //tasks till the video ad will be closed.
        void RewardedVideoAdOpenedEvent()
        {
        }
        //Invoked when the RewardedVideo ad view is about to be closed.
        //Your activity will now regain its focus.
        void RewardedVideoAdClosedEvent()
        {
        }
        //Invoked when there is a change in the ad availability status.
        //@param - available - value will change to true when rewarded videos are available. 
        //You can then show the video by calling showRewardedVideo().
        //Value will change to false when no videos are available.
        void RewardedVideoAvailabilityChangedEvent(bool available)
        {
            //Change the in-app 'Traffic Driver' state according to availability.
            bool rewardedVideoAvailability = available;
        }

        //Invoked when the user completed the video and should be rewarded. 
        //If using server-to-server callbacks you may ignore this events and wait for 
        // the callback from the  ironSource server.
        //@param - placement - placement object which contains the reward data
        void RewardedVideoAdRewardedEvent(IronSourcePlacement placement)
        {
            OnRewardedVideoDismissed?.Invoke(true);
        }
        //Invoked when the Rewarded Video failed to show
        //@param description - string - contains information about the failure.
        void RewardedVideoAdShowFailedEvent(IronSourceError error)
        {
            OnRewardedVideoDismissed?.Invoke(false);
        }

        // ----------------------------------------------------------------------------------------
        // Note: the events below are not available for all supported rewarded video ad networks. 
        // Check which events are available per ad network you choose to include in your build. 
        // We recommend only using events which register to ALL ad networks you include in your build. 
        // ----------------------------------------------------------------------------------------

        //Invoked when the video ad starts playing. 
        void RewardedVideoAdStartedEvent()
        {
        }
        //Invoked when the video ad finishes playing. 
        void RewardedVideoAdEndedEvent()
        {
        }
        //Invoked when the video ad is clicked. 
        void RewardedVideoAdClickedEvent(IronSourcePlacement placement)
        {
        }

        public void LoadRewarded()
        {
            IronSource.Agent.loadManualRewardedVideo();
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
            IronSource.Agent.showInterstitial("DefaultInterstitial");
        }
    }
}