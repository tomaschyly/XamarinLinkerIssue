using System;
using System.Collections.Generic;

using Android;
using Android.App;
using Android.Content.PM;
using Android.Database;
using Android.OS;
using Android.Provider;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace XamarinLinkerIssue
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

			Button button = FindViewById<Button> (Resource.Id.button);
			button.Click += LoadContacts;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View) sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }

		/**
		 * Read contacts, if needed ask for permission first.
		 */
		public void LoadContacts (object sender, EventArgs eventArgs) {
			if (ContextCompat.CheckSelfPermission (this, Manifest.Permission.ReadContacts) == Permission.Granted) {
				List<Contact> list = new List<Contact> ();

				Android.Net.Uri uri = ContactsContract.Contacts.ContentUri;
				string [] projection = { ContactsContract.Contacts.InterfaceConsts.Id, ContactsContract.Contacts.InterfaceConsts.DisplayName };

				ICursor cursor = (ICursor) new CursorLoader (this, uri, projection, null, null, null).LoadInBackground ();

				if (cursor.MoveToFirst ()) {
					do {
						try {
							list.Add (new Contact (cursor.GetInt (cursor.GetColumnIndex (projection [0])), cursor.GetString (cursor.GetColumnIndex (projection [1]))));
						} catch (Exception e) {
							Log.Error ("TCH_error", "MainActivity - LoadContacts - contact error:\n" + e.Message);
						}
					} while (cursor.MoveToNext ());
				}

				cursor.Close ();

				Toast.MakeText (this, "Loaded " + list.Count + " contacts.", ToastLength.Long).Show ();
			} else {
				ActivityCompat.RequestPermissions (this, new string [] { Manifest.Permission.ReadContacts }, 1);
			}
		}

		/**
		 * Callback received when a permissions request has been completed.
		 */
		public override void OnRequestPermissionsResult (int requestCode, string [] permissions, Permission [] grantResults) {
			base.OnRequestPermissionsResult (requestCode, permissions, grantResults);

			if (requestCode == 1 && ContextCompat.CheckSelfPermission (this, Manifest.Permission.ReadContacts) == Permission.Granted) {
				LoadContacts (null, null);
			}
		}
	}
}
