
using System.Collections.ObjectModel;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Corcav.SmartRecyclerViewAdapter;

namespace RecyclerViewer
{
	public class PhotoRecyclerAdapter : SmartRecyclerViewAdapter<Photo>
	{
		public PhotoRecyclerAdapter(RecyclerView recyclerView, ObservableCollection<Photo> items, int? layoutId = null) : base(recyclerView, items, layoutId)
		{
		}


		protected override void OnLookupViewItems(View layout, GenericViewHolder viewHolder)
		{
			ImageView image = layout.FindViewById<ImageView>(Resource.Id.imageView);
			TextView caption = layout.FindViewById<TextView>(Resource.Id.textView);

			viewHolder.AddView(image, "image");
			viewHolder.AddView(caption, "caption");
		}

		protected override void OnUpdateView(GenericViewHolder viewHolder, Photo item)
		{
			viewHolder.GetView<ImageView>("image").SetImageResource(item.PhotoID);
			viewHolder.GetView<TextView>("caption").Text = item.Caption;
		}
	}
}