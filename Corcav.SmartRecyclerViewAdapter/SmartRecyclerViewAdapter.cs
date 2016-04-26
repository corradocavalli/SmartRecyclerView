
#region Using

using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Android.Support.V7.Widget;
using Android.Views;
using Object = Java.Lang.Object;

#endregion

namespace Corcav.SmartRecyclerViewAdapter
{
	/// <summary>
	/// Smart implementation of a RecyclerView adapter that simplifies common operations
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <seealso cref="Android.Support.V7.Widget.RecyclerView.Adapter" />
	public abstract class SmartRecyclerViewAdapter<T> : RecyclerView.Adapter
	{
		private readonly ObservableCollection<T> items;
		private readonly int? layoutId;
		private readonly RecyclerView recyclerView;

		protected SmartRecyclerViewAdapter(RecyclerView recyclerView, ObservableCollection<T> items, int? layoutId = null)
		{
			this.items = items;
			items.CollectionChanged += this.OnCollectionChanged;
			this.layoutId = layoutId;
			this.recyclerView = recyclerView;
			if (recyclerView != null)
			{
				this.recyclerView.SetAdapter(this);
				this.recyclerView.AddOnChildAttachStateChangeListener(new AttachStateChangeListener(this));
			}
		}

		public override int ItemCount => this.items.Count;


		public event EventHandler<T> ItemSelected;

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
		{
			GenericViewHolder genericViewHolder = (GenericViewHolder)holder;
			this.OnUpdateView(genericViewHolder, this.items[position]);
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
		{
			int viewId = this.OnGetViewId(viewType);
			View layout = LayoutInflater.From(parent.Context).Inflate(viewId, parent, false);


			GenericViewHolder genericViewHolder = new GenericViewHolder(layout);

			this.OnLookupViewItems(layout, genericViewHolder);
			return genericViewHolder;
		}

		public override int GetItemViewType(int position)
		{
			return this.GetViewIdForType(this.items[position]);
		}

		protected virtual int OnGetViewId(int viewType)
		{
			if (this.layoutId == null)
			{
				throw new InvalidOperationException("No layoutId provided on adapter constructor, you need to override OnGetViewId and provide a valid resource is for this viewType");
			}

			return this.layoutId.Value;
		}

		protected abstract void OnLookupViewItems(View layout, GenericViewHolder viewHolder);

		protected abstract void OnUpdateView(GenericViewHolder viewHolder, T item);

		protected virtual int GetViewIdForType(T item)
		{
			return 0;
		}

		protected virtual void OnItemSelected(T e)
		{
			this.ItemSelected?.Invoke(this, e);
		}

		private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					this.NotifyItemInserted(e.NewStartingIndex);
					break;
				case NotifyCollectionChangedAction.Remove:
					this.NotifyItemRemoved(e.OldStartingIndex);
					break;
				case NotifyCollectionChangedAction.Replace:
					this.NotifyItemChanged(e.OldStartingIndex);
					this.NotifyItemChanged(e.NewStartingIndex);
					break;
				case NotifyCollectionChangedAction.Move:
					this.NotifyItemRemoved(e.OldStartingIndex);
					this.NotifyItemRemoved(e.NewStartingIndex);
					break;
				case NotifyCollectionChangedAction.Reset:
					this.NotifyDataSetChanged();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		/// <summary>
		/// Subscribes to view click so that we can have ItemSelected w/o any custom code
		/// </summary>
		/// <seealso cref="Android.Support.V7.Widget.RecyclerView.Adapter" />
		internal class AttachStateChangeListener : Object, RecyclerView.IOnChildAttachStateChangeListener
		{
			private readonly SmartRecyclerViewAdapter<T> parentAdapter;

			public AttachStateChangeListener(SmartRecyclerViewAdapter<T> parentAdapter) : base()
			{
				this.parentAdapter = parentAdapter;
			}

			public void OnChildViewAttachedToWindow(View view)
			{
				view.Click += this.View_Click;
			}

			public void OnChildViewDetachedFromWindow(View view)
			{
				view.Click -= this.View_Click;
			}

			private void View_Click(object sender, EventArgs e)
			{
				GenericViewHolder holder = (GenericViewHolder)this.parentAdapter.recyclerView.GetChildViewHolder(((View)sender));
				int clickedPosition = holder.AdapterPosition;
				this.parentAdapter.OnItemSelected(this.parentAdapter.items[clickedPosition]);
			}
		}
	}
}