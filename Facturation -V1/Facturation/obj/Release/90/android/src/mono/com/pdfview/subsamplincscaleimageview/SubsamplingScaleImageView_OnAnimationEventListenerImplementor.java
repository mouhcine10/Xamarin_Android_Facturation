package mono.com.pdfview.subsamplincscaleimageview;


public class SubsamplingScaleImageView_OnAnimationEventListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.pdfview.subsamplincscaleimageview.SubsamplingScaleImageView.OnAnimationEventListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onComplete:()V:GetOnCompleteHandler:PDFViewAndroid.SubscaleView.SubsamplingScaleImageView/IOnAnimationEventListenerInvoker, Xamarin.Bindings.PDFView-Android\n" +
			"n_onInterruptedByNewAnim:()V:GetOnInterruptedByNewAnimHandler:PDFViewAndroid.SubscaleView.SubsamplingScaleImageView/IOnAnimationEventListenerInvoker, Xamarin.Bindings.PDFView-Android\n" +
			"n_onInterruptedByUser:()V:GetOnInterruptedByUserHandler:PDFViewAndroid.SubscaleView.SubsamplingScaleImageView/IOnAnimationEventListenerInvoker, Xamarin.Bindings.PDFView-Android\n" +
			"";
		mono.android.Runtime.register ("PDFViewAndroid.SubscaleView.SubsamplingScaleImageView+IOnAnimationEventListenerImplementor, Xamarin.Bindings.PDFView-Android", SubsamplingScaleImageView_OnAnimationEventListenerImplementor.class, __md_methods);
	}


	public SubsamplingScaleImageView_OnAnimationEventListenerImplementor ()
	{
		super ();
		if (getClass () == SubsamplingScaleImageView_OnAnimationEventListenerImplementor.class)
			mono.android.TypeManager.Activate ("PDFViewAndroid.SubscaleView.SubsamplingScaleImageView+IOnAnimationEventListenerImplementor, Xamarin.Bindings.PDFView-Android", "", this, new java.lang.Object[] {  });
	}


	public void onComplete ()
	{
		n_onComplete ();
	}

	private native void n_onComplete ();


	public void onInterruptedByNewAnim ()
	{
		n_onInterruptedByNewAnim ();
	}

	private native void n_onInterruptedByNewAnim ();


	public void onInterruptedByUser ()
	{
		n_onInterruptedByUser ();
	}

	private native void n_onInterruptedByUser ();

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
