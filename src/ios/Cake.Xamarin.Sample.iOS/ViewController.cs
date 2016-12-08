using System;
using Cake.Xamarin.Sample.Shared;
using UIKit;
using System.Runtime.Remoting.Channels;

namespace Cake.Xamarin.Sample.iOS
{
    public partial class ViewController : UIViewController
    {
        protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Using shared code here from PCL.
            Button.TouchUpInside += (sender, e) =>
            {
                Button.SetTitle(new SharedClass().Add(1, 3).ToString(), UIControlState.Normal);
            };
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}
