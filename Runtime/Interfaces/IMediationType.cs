using System;
using System.Collections;
using UnityEngine;

namespace Assets.Mazi.Interfaces
{
    public class RewardedVideoResultData
    {
        public bool _isSuccess;

        public RewardedVideoResultData(bool isSuccess)
        {
            _isSuccess = isSuccess;
        }
    }

    public interface IMediationType
    {
        public event Action<bool> OnRewardedVideoDismissed;        
        public void LoadRewarded();
        public void ShowRewarded();
        public void LoadInterstitial();
        public void ShowInterstitial();
    }
}