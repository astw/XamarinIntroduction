﻿using XamarinIntro;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamarinIntro.Droid;
using System.ComponentModel;
using XamarinIntro.Views;

[assembly: ExportRenderer(typeof(SketchView), typeof(SketchViewRenderer))]
namespace XamarinIntro.Droid
{
	class SketchViewRenderer : ViewRenderer<SketchView, PaintView>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<SketchView> e)
		{
			base.OnElementChanged(e);

			if (Control == null)
			{
				var paintView = new PaintView(Forms.Context);
				paintView.SetInkColor(Element.InkColor.ToAndroid());
				paintView.LineDrawn += PaintViewLineDrawn;
				SetNativeControl(paintView);

				MessagingCenter.Subscribe<SketchView>(this, "Clear", OnMessageClear);
			}
		}

		void OnMessageClear(SketchView sender)
		{
			if (sender == Element)
				Control.Clear();
		}

		private void PaintViewLineDrawn(object sender, System.EventArgs e)
		{
			((ISketchController)Element)?.SendSketchUpdated();
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == SketchView.InkColorProperty.PropertyName)
			{
				Control.SetInkColor(Element.InkColor.ToAndroid());
			}
		}

		protected override void Dispose(bool disposing)
		{
			MessagingCenter.Unsubscribe<SketchView>(this, "Clear");
			Control.LineDrawn -= PaintViewLineDrawn;
		}
	}
}