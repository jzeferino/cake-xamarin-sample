using Android.App;
using Android.Widget;
using Android.OS;
using Cake.Xamarin.Sample.Shared;

namespace Cake.Xamarin.Sample.Android
{
    [Activity(Label = "Cake.Xamarin.Sample.Android", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Using shared code here from PCL.
            var button = FindViewById<Button>(Resource.Id.button);

            button.Click += (sender, e) =>
            {
                button.Text = new SharedClass().Add(1, 3).ToString();
            };
        }
    }
}

