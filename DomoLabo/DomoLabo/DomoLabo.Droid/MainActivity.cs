﻿using System;
using System.Collections.Generic;
using Android;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using AndroidX.Core.App;
using Plugin.FirebasePushNotification;

namespace DomoLabo.Droid
{
    [Activity(Label = "DomoLabo", Theme = "@style/MainTheme", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        
        private const int REQUEST_PERMISSION_CODE = 1001;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            FirebasePushNotificationManager.ProcessIntent(this, Intent);
            LoadApplication(new App());
            initPermission();
        }
        
        private void initPermission()
        {
            List<String> mPermissionList = new List<string>();
            // When the Android version is 12 or greater, apply for new Bluetooth permissions
            if (Build.VERSION.SdkInt >= BuildVersionCodes.S)
            {
                mPermissionList.Add(Manifest.Permission.BluetoothScan);
                mPermissionList.Add(Manifest.Permission.BluetoothAdvertise);
                mPermissionList.Add(Manifest.Permission.BluetoothConnect);
                //Request for location permissions based on your actual needs
                mPermissionList.Add(Manifest.Permission.AccessCoarseLocation);
                mPermissionList.Add(Manifest.Permission.AccessFineLocation);
            }
            else
            {
                mPermissionList.Add(Manifest.Permission.AccessCoarseLocation);
                mPermissionList.Add(Manifest.Permission.AccessFineLocation);
            }

            ActivityCompat.RequestPermissions(this, mPermissionList.ToArray(), REQUEST_PERMISSION_CODE);
        }
    }
}