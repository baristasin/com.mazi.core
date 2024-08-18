using System;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Mazi.UserDataContainer;

namespace Mazi.BuffetIAPurchasing
{
    public class Buffet : MonoBehaviour, IDetailedStoreListener
    {
        public static Buffet Instance;

        public event Action OnNoAdsBought;

        public bool IsNoAdsBought => _isNoAdsBought;

        private IStoreController m_StoreController;

        private bool _isNoAdsBought;

        private BuffetIAPItemContainer _buffetIAPItemContainer;

        async void Awake()
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

            try
            {
                await UnityServices.InitializeAsync();
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log($"Sign in anonymously succeeded! PlayerID: {AuthenticationService.Instance.PlayerId}");
                InitializeIAPItems();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            _isNoAdsBought = UserData.IsNoAdsBought;
        }

        public void InitializeIAPItems()
        {
            _buffetIAPItemContainer = Resources.Load<BuffetIAPItemContainer>("BuffetIAP/BuffetIAPItemContainer");

            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());            

            for (int i = 0; i < _buffetIAPItemContainer.BuffetIAPItems.Count; i++)
            {
                builder.AddProduct(_buffetIAPItemContainer.BuffetIAPItems[i].ItemId, _buffetIAPItemContainer.BuffetIAPItems[i].ProductType);
            }

            UnityPurchasing.Initialize(this, builder);
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            Debug.Log("In-App Purchasing successfully initialized");
            m_StoreController = controller;
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            OnInitializeFailed(error, null);
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            var errorMessage = $"Purchasing failed to initialize. Reason: {error}.";

            if (message != null)
            {
                errorMessage += $" More details: {message}";
            }

            Debug.Log(errorMessage);
        }

        public void BuyItem(string itemId)
        {
            m_StoreController.InitiatePurchase(itemId);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            //Retrieve the purchased product
            var product = purchaseEvent.purchasedProduct;

            //Add the purchased product to the players inventory
            if (product.definition.id == _buffetIAPItemContainer.BuffetIAPItems.Find(x => x.ItemSpecialty == ItemSpecialty.NoAds).ItemId) // TODO
            {
                _isNoAdsBought = true;
                UserData.IsNoAdsBought = true;
                OnNoAdsBought?.Invoke();
            }

            Debug.Log($"Purchase Complete - Product: {product.definition.id}");

            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            Debug.Log($"Purchase failed - Product: '{product.definition.id}', PurchaseFailureReason: {failureDescription.reason}");

        }
    
        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            Debug.Log($"Purchase failed - Product: '{product.definition.id}'," + $" Purchase failure details: {failureReason}");
        }


    }
}