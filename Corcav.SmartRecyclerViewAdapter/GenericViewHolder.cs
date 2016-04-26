
#region Using

using System;
using System.Collections.Generic;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;

#endregion

namespace Corcav.SmartRecyclerViewAdapter
{
	/// <summary>
	/// Generic view holder that stores views and lookup keys into a dictionary
	/// </summary>
	/// <seealso cref="Android.Support.V7.Widget.RecyclerView.ViewHolder" />
	public class GenericViewHolder : RecyclerView.ViewHolder
	{
		private readonly Dictionary<string, View> viewCollection = new Dictionary<string, View>();

		public GenericViewHolder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
		}

		public GenericViewHolder(View itemView) : base(itemView)
		{
		}

		public void AddView(View view, string id)
		{
			this.viewCollection.Add(id, view);
		}

		public T GetView<T>(string id) where T : View
		{
			return (T)this.viewCollection[id];
		}
	}
}