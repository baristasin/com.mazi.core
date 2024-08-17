using System;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace Mazi.BuffetIAPurchasing
{
    public class Buffet : MonoBehaviour, IDetailedStoreListener
    {
        public static Buffet Instance;

        private IStoreController m_StoreController;

        private void Awake()
        {
            InitializeIAPItems();
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
        }

        public void InitializeIAPItems()
        {
            var buffetIAPItems = Resources.Load<BuffetIAPItemContainer>("BuffetIAP/BuffetIAPItemContainer");

            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());            

            for (int i = 0; i < buffetIAPItems.BuffetIAPItems.Count; i++)
            {
                builder.AddProduct(buffetIAPItems.BuffetIAPItems[i].ItemId, buffetIAPItems.BuffetIAPItems[i].ProductType);
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
            if (product.definition.id == "com.mycompany.noads") // TODO
            {
                // Add NoAds
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