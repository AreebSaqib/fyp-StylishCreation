using UnityEngine;
using System;
//using UnityEngine.Purchasing;

public class GSF_InAppController : MonoBehaviour
    //, IStoreListener
{
    public static GSF_InAppController Instance { get; private set; }
    //private Action onPurchaseComplete = null;

    //private IStoreController m_Controller;
    //private IAppleExtensions m_AppleExtensions;
    //private IGooglePlayStoreExtensions m_GooglePlayStoreExtensions;

    //private bool m_IsGooglePlayStoreSelected;

    ///// <summary>
    ///// This will be called when Unity IAP has finished initialising.
    ///// </summary>
    //public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    //{
    //    m_Controller = controller;
    //    m_AppleExtensions = extensions.GetExtension<IAppleExtensions>();
    //    m_GooglePlayStoreExtensions = extensions.GetExtension<IGooglePlayStoreExtensions>();

    //    m_AppleExtensions.RegisterPurchaseDeferredListener(OnDeferred);
    //}

    ///// <summary>
    ///// iOS Specific.
    ///// This is called as part of Apple's 'Ask to buy' functionality,
    ///// when a purchase is requested by a minor and referred to a parent
    ///// for approval.
    /////
    ///// When the purchase is approved or rejected, the normal purchase events
    ///// will fire.
    ///// </summary>
    ///// <param name="item">Item.</param>
    //private void OnDeferred(Product item)
    //{
    //    Debug.Log("Purchase deferred: " + item.definition.id);
    //}

    ///// <summary>
    ///// This will be called when a purchase completes.
    ///// </summary>
    //public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    //{
    //    m_Controller.ConfirmPendingPurchase(e.purchasedProduct);

    //    if (onPurchaseComplete != null)
    //        onPurchaseComplete.Invoke();

    //    return PurchaseProcessingResult.Complete;
    //}

    ///// <summary>
    ///// This will be called if an attempted purchase fails.
    ///// </summary>
    //public void OnPurchaseFailed(Product item, PurchaseFailureReason r)
    //{
    //    Debug.Log("Purchase failed: " + item.definition.id);
    //    Debug.Log(r);
    //}

    //public void OnInitializeFailed(InitializationFailureReason error)
    //{
    //    Debug.Log("Billing failed to initialize!");
    //    switch (error)
    //    {
    //        case InitializationFailureReason.AppNotKnown:
    //            Debug.LogError("Is your App correctly uploaded on the relevant publisher console?");
    //            break;
    //        case InitializationFailureReason.PurchasingUnavailable:
    //            // Ask the user if billing is disabled in device settings.
    //            Debug.Log("Billing disabled!");
    //            break;
    //        case InitializationFailureReason.NoProductsAvailable:
    //            // Developer configuration error; check product metadata.
    //            Debug.Log("No products available for purchase!");
    //            break;
    //    }
    //}

    ///// <summary>
    ///// This will be called after a call to IAppleExtensions.RestoreTransactions().
    ///// </summary>
    //private void OnTransactionsRestored(bool success)
    //{
    //    Debug.Log("Transactions restored." + success);
    //}

    public void PurchaseInAppProduct(string productID, Action onComplete = null)
    {
        //if (m_Controller == null)
        //{
        //    Debug.LogError("Purchasing is not initialized");
        //    return;
        //}

        //onPurchaseComplete = onComplete;

        //m_Controller.InitiatePurchase(m_Controller.products.WithID(productID), "developerPayload");
    }

    public void RestoreButtonClick()
    {
        //if (m_IsGooglePlayStoreSelected)
        //{
        //    m_GooglePlayStoreExtensions.RestoreTransactions(OnTransactionsRestored);
        //}
        //else
        //{
        //    m_AppleExtensions.RestoreTransactions(OnTransactionsRestored);
        //}
    }

    //public void Awake()
    //{

    //    if (Instance == null)
    //    {
    //        Instance = this;
    //        DontDestroyOnLoad(this.gameObject);
    //    }
    //    else
    //    {
    //        Destroy(this.gameObject);
    //    }

    //    var module = StandardPurchasingModule.Instance();
    //    var builder = ConfigurationBuilder.Instance(module);

    //    // Set this to true to enable the Microsoft IAP simulator for local testing.
    //    builder.Configure<IMicrosoftConfiguration>().useMockBillingSystem = false;
    //    m_IsGooglePlayStoreSelected = Application.platform == RuntimePlatform.Android && module.appStore == AppStore.GooglePlay;

    //    var catalog = ProductCatalog.LoadDefaultCatalog();

    //    foreach (var product in catalog.allValidProducts)
    //    {
    //        if (product.allStoreIDs.Count > 0)
    //        {
    //            var ids = new IDs();
    //            foreach (var storeID in product.allStoreIDs)
    //            {
    //                ids.Add(storeID.id, storeID.store);
    //            }
    //            builder.AddProduct(product.id, product.type, ids);
    //        }
    //        else
    //        {
    //            builder.AddProduct(product.id, product.type);
    //        }
    //    }

    //    Action initializeUnityIap = () => {
    //        // Now we're ready to initialize Unity IAP.
    //        UnityPurchasing.Initialize(this, builder);
    //    };

    //    initializeUnityIap();
    //}
}