package mono.com.google.android.gms.ads.purchase;


public class InAppPurchaseListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.google.android.gms.ads.purchase.InAppPurchaseListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onInAppPurchaseRequested:(Lcom/google/android/gms/ads/purchase/InAppPurchase;)V:GetOnInAppPurchaseRequested_Lcom_google_android_gms_ads_purchase_InAppPurchase_Handler:Android.Gms.Ads.Purchase.IInAppPurchaseListenerInvoker, Xamarin.GooglePlayServices.Ads.Lite\n" +
			"";
		mono.android.Runtime.register ("Android.Gms.Ads.Purchase.IInAppPurchaseListenerImplementor, Xamarin.GooglePlayServices.Ads.Lite", InAppPurchaseListenerImplementor.class, __md_methods);
	}


	public InAppPurchaseListenerImplementor ()
	{
		super ();
		if (getClass () == InAppPurchaseListenerImplementor.class)
			mono.android.TypeManager.Activate ("Android.Gms.Ads.Purchase.IInAppPurchaseListenerImplementor, Xamarin.GooglePlayServices.Ads.Lite", "", this, new java.lang.Object[] {  });
	}


	public void onInAppPurchaseRequested (com.google.android.gms.ads.purchase.InAppPurchase p0)
	{
		n_onInAppPurchaseRequested (p0);
	}

	private native void n_onInAppPurchaseRequested (com.google.android.gms.ads.purchase.InAppPurchase p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}