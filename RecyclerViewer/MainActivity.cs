
#region Using

using System.Collections.ObjectModel;
using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Widget;

#endregion

namespace RecyclerViewer
{
	[Activity(Label = "RecyclerViewer", MainLauncher = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.Material.Light.DarkActionBar")]
	public class MainActivity : Activity
	{
		RecyclerView recyclerView;
		private ObservableCollection<Photo> photoCollection;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			this.SetContentView(Resource.Layout.Main);

			// Get our RecyclerView layout:
			this.recyclerView = this.FindViewById<RecyclerView>(Resource.Id.recyclerView);

			// Layout Manager Setup
			var layoutManager = new LinearLayoutManager(this);
			this.recyclerView.SetLayoutManager(layoutManager);

			// Adapter Setup
			this.photoCollection = PhotoAlbum.GetPhotos();
			PhotoRecyclerAdapter adapter = new PhotoRecyclerAdapter(this.recyclerView, this.photoCollection, Resource.Layout.PhotoCardView);
			adapter.ItemSelected +=
				(s, e) =>
				{
					Toast.MakeText(this, "This is photo of " + e.Caption, ToastLength.Short).Show();
				};

			this.recyclerView.SetAdapter(adapter);

			// Clear+Add button
			Button randomPickBtn = this.FindViewById<Button>(Resource.Id.randPickButton);

			// Handler for the Random Pick Button:
			randomPickBtn.Click += delegate
			{
				var newPhoto = new Photo
				{
					mPhotoID = Resource.Drawable.big_ben_2,
					mCaption = "New Big Ben"
				};

				this.photoCollection.Clear();
				this.photoCollection.Add(newPhoto);
			};
		}
	}
}