package mono.com.pdfview.subsamplincscaleimageview;


public class SubsamplingScaleImageView_OnStateChangedListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.pdfview.subsamplincscaleimageview.SubsamplingScaleImageView.OnStateChangedListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCenterChanged:(Landroid/graphics/PointF;I)V:GetOnCenterChanged_Landroid_graphics_PointF_IHandler:PDFViewAndroid.SubscaleView.SubsamplingScaleImageView/IOnStateChangedListenerInvoker, Xamarin.Bindings.PDFView-Android\n" +
			"n_onScaleChanged:(FI)V:GetOnScaleChanged_FIHandler:PDFViewAndroid.SubscaleView.SubsamplingScaleImageView/IOnStateChangedListenerInvoker, Xamarin.Bindings.PDFView-Android\n" +
			"";
		mono.android.Runtime.register ("PDFViewAndroid.SubscaleView.SubsamplingScaleImageView+IOnStateChangedListenerImplementor, Xamarin.Bindings.PDFView-Android", SubsamplingScaleImageView_OnStateChangedListenerImplementor.class, __md_methods);
	}


	public SubsamplingScaleImageView_OnStateChangedListenerImplementor ()
	{
		super ();
		if (getClass () == SubsamplingScaleImageView_OnStateChangedListenerImplementor.class)
			mono.android.TypeManager.Activate ("PDFViewAndroid.SubscaleView.SubsamplingScaleImageView+IOnStateChangedListenerImplementor, Xamarin.Bindings.PDFView-Android", "", this, new java.lang.Object[] {  });
	}


	public void onCenterChanged (android.graphics.PointF p0, int p1)
	{
		n_onCenterChanged (p0, p1);
	}

	private native void n_onCenterChanged (android.graphics.PointF p0, int p1);


	public void onScaleChanged (float p0, int p1)
	{
		n_onScaleChanged (p0, p1);
	}

	private native void n_onScaleChanged (float p0, int p1);

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
