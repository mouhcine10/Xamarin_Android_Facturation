package mono.com.pdfview.subsamplincscaleimageview;


public class SubsamplingScaleImageView_OnImageEventListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.pdfview.subsamplincscaleimageview.SubsamplingScaleImageView.OnImageEventListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onImageLoadError:(Ljava/lang/Exception;)V:GetOnImageLoadError_Ljava_lang_Exception_Handler:PDFViewAndroid.SubscaleView.SubsamplingScaleImageView/IOnImageEventListenerInvoker, Xamarin.Bindings.PDFView-Android\n" +
			"n_onImageLoaded:()V:GetOnImageLoadedHandler:PDFViewAndroid.SubscaleView.SubsamplingScaleImageView/IOnImageEventListenerInvoker, Xamarin.Bindings.PDFView-Android\n" +
			"n_onPreviewLoadError:(Ljava/lang/Exception;)V:GetOnPreviewLoadError_Ljava_lang_Exception_Handler:PDFViewAndroid.SubscaleView.SubsamplingScaleImageView/IOnImageEventListenerInvoker, Xamarin.Bindings.PDFView-Android\n" +
			"n_onPreviewReleased:()V:GetOnPreviewReleasedHandler:PDFViewAndroid.SubscaleView.SubsamplingScaleImageView/IOnImageEventListenerInvoker, Xamarin.Bindings.PDFView-Android\n" +
			"n_onReady:()V:GetOnReadyHandler:PDFViewAndroid.SubscaleView.SubsamplingScaleImageView/IOnImageEventListenerInvoker, Xamarin.Bindings.PDFView-Android\n" +
			"n_onTileLoadError:(Ljava/lang/Exception;)V:GetOnTileLoadError_Ljava_lang_Exception_Handler:PDFViewAndroid.SubscaleView.SubsamplingScaleImageView/IOnImageEventListenerInvoker, Xamarin.Bindings.PDFView-Android\n" +
			"";
		mono.android.Runtime.register ("PDFViewAndroid.SubscaleView.SubsamplingScaleImageView+IOnImageEventListenerImplementor, Xamarin.Bindings.PDFView-Android", SubsamplingScaleImageView_OnImageEventListenerImplementor.class, __md_methods);
	}


	public SubsamplingScaleImageView_OnImageEventListenerImplementor ()
	{
		super ();
		if (getClass () == SubsamplingScaleImageView_OnImageEventListenerImplementor.class)
			mono.android.TypeManager.Activate ("PDFViewAndroid.SubscaleView.SubsamplingScaleImageView+IOnImageEventListenerImplementor, Xamarin.Bindings.PDFView-Android", "", this, new java.lang.Object[] {  });
	}


	public void onImageLoadError (java.lang.Exception p0)
	{
		n_onImageLoadError (p0);
	}

	private native void n_onImageLoadError (java.lang.Exception p0);


	public void onImageLoaded ()
	{
		n_onImageLoaded ();
	}

	private native void n_onImageLoaded ();


	public void onPreviewLoadError (java.lang.Exception p0)
	{
		n_onPreviewLoadError (p0);
	}

	private native void n_onPreviewLoadError (java.lang.Exception p0);


	public void onPreviewReleased ()
	{
		n_onPreviewReleased ();
	}

	private native void n_onPreviewReleased ();


	public void onReady ()
	{
		n_onReady ();
	}

	private native void n_onReady ();


	public void onTileLoadError (java.lang.Exception p0)
	{
		n_onTileLoadError (p0);
	}

	private native void n_onTileLoadError (java.lang.Exception p0);

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
