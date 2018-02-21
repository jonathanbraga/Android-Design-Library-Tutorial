﻿using System;
using System.Collections.Generic;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using DesignLibrary.Helpers;
using SupportFragment = Android.Support.V4.App.Fragment;

namespace DesignLibrary.Fragments
{
    public class Fragment1 : SupportFragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            RecyclerView recyclerView = inflater.Inflate(Resource.Layout.Fragment1, container, false) as RecyclerView;

            SetUpRecyclerView(recyclerView);

            return recyclerView;
        }

        private void SetUpRecyclerView(RecyclerView recyclerView)
        {
            var values = GetRandomSubList(Cheeses.CheeseStrings, 30);

            recyclerView.SetLayoutManager(new LinearLayoutManager(recyclerView.Context));
            recyclerView.SetAdapter(new SimpleStringRecyclerViewAdapter(recyclerView.Context, values, Activity.Resources));
        }

        public List<string> GetRandomSubList(List<string> items, int amout)
        {
            List<string> list = new List<string>();
            Random random = new Random();

            while(list.Count < amout)
            {
                list.Add(items[random.Next(items.Count)]);
            }

            return list;
        }
    }

    public class SimpleStringRecyclerViewAdapter : RecyclerView.Adapter
    {
        private readonly TypedValue mTypedValue = new TypedValue();
        private int mBackground;
        private List<string> mValues;
        Resources mResource;
        private Dictionary<int, int> mCalculatedSizes;

        public SimpleStringRecyclerViewAdapter(Context context, List<string> items, Resources res)
        {
            context.Theme.ResolveAttribute(Resource.Attribute.selectableItemBackground, mTypedValue, true);
            mBackground = mTypedValue.ResourceId;
            mValues = items;
            mResource = res;

            mCalculatedSizes = new Dictionary<int, int>();
        }

        public override int ItemCount
        {
            get
            {
                return mValues.Count;
            }
        }

        public override async void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var simpleHolder = holder as SimpleViewHolder;

            simpleHolder.mBoundString = mValues[position];
            simpleHolder.mTxtView.Text = mValues[position];

            int drawableID = Cheeses.RandomCheeseDrawable;
            BitmapFactory.Options options = new BitmapFactory.Options();

            if (mCalculatedSizes.ContainsKey(drawableID))
            {
                options.InSampleSize = mCalculatedSizes[drawableID];
            }

            else
            {
                options.InJustDecodeBounds = true;

                BitmapFactory.DecodeResource(mResource, drawableID, options);

                options.InSampleSize = Cheeses.CalculateInSampleSize(options, 100, 100);
                options.InJustDecodeBounds = false;

                mCalculatedSizes.Add(drawableID, options.InSampleSize);
            }


            var bitMap = await BitmapFactory.DecodeResourceAsync(mResource, drawableID, options);

            simpleHolder.mImageView.SetImageBitmap(bitMap);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.List_item, parent, false);
            view.SetBackgroundResource(mBackground);

            return new SimpleViewHolder(view);
        }
    }

    public class SimpleViewHolder : RecyclerView.ViewHolder
    {
        public string mBoundString;
        public readonly View mView;
        public readonly ImageView mImageView;
        public readonly TextView mTxtView;

        public SimpleViewHolder(View view) : base(view)
        {
            mView = view;
            mImageView = view.FindViewById<ImageView>(Resource.Id.avatar);
            mTxtView = view.FindViewById<TextView>(Resource.Id.text1);
        }

        public override string ToString()
        {
            return base.ToString() + " '" + mTxtView.Text;
        }
    }
}