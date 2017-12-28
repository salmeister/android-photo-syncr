using Android.App;
using Android.Widget;
using Android.OS;
using System.IO;
using android_photo_syncr.Tools;
using System.Net.Http;
using System;
using Android.Bluetooth;
using System.Linq;

namespace android_photo_syncr
{
    [Activity(Label = "Photo Syncr", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            string dbFile = "photoSyncr.db3";
            string dbFilePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), dbFile);
            SQLDB sqlConn = new SQLDB(dbFilePath);

            EditText txtSource = FindViewById<EditText>(Resource.Id.txtSource);
            Button btnSync = FindViewById<Button>(Resource.Id.btnSync);
            btnSync.RequestFocus();

            btnSync.Click += async (sender, e) =>
            {
                try
                {
                    string path = Path.Combine((string)Android.OS.Environment.ExternalStorageDirectory, txtSource.Text).TrimEnd('/');
                    if (Directory.Exists(path))
                    {
                        string clientSS = AESEncryption.Encrypt("someSharedSecertValue");
                        string url = "http://someURL:somePort";
                        RESTClient restAPI = new RESTClient();
                        if (restAPI.TestConnection(url))
                        {
                            var existingPics = sqlConn.GetExistingPictures().Where(p => p.path.ToLower() == path.ToLower() && p.uploaded == true);
                            BluetoothAdapter myDevice = BluetoothAdapter.DefaultAdapter;
                            string deviceName = myDevice.Name;
                            MultipartFormDataContent multiContent = new MultipartFormDataContent();
                            multiContent.Add(new StringContent(clientSS), "secret");

                            string[] pictures = Directory.GetFiles(path, "*.jpg", SearchOption.AllDirectories);
                            int counter = 0;
                            foreach (var pic in pictures)
                            {
                                FileInfo fi = new FileInfo(pic);
                                string deviceFileName = fi.Name;
                                if (!existingPics.Where(p => p.fileName.ToLower() == deviceFileName.ToLower()).Any())
                                {
                                    string ext = fi.Extension;
                                    string xferFileName = $"{ deviceFileName.Replace(ext, "")}_{deviceName}{ext}";
                                    byte[] fileBytes = System.IO.File.ReadAllBytes(pic);
                                    ByteArrayContent bytes = new ByteArrayContent(fileBytes);
                                    multiContent.Add(bytes, "file", xferFileName);
                                    counter++;
                                    sqlConn.InsertNewPicture(new Picture() { fileName = deviceFileName, path = path, uploaded = true });
                                }
                            }

                            if (counter > 0)
                            {
                                string plural = "";
                                if (counter > 1)
                                {
                                    plural = "s";
                                }
                                Toast.MakeText(this, $"Uploading {counter} new picture{plural}.", ToastLength.Long).Show();
                                await restAPI.AsyncMultipartPost("", url, multiContent);
                                Toast.MakeText(this, $"Finished uploading picture{plural}.", ToastLength.Long).Show();
                            }
                            else
                            {
                                Toast.MakeText(this, $"There are no new pictures to upload.", ToastLength.Long).Show();
                            }
                        }
                        else
                        {
                            Toast.MakeText(this, $"The server is not available.  Try again later.", ToastLength.Long).Show();
                        }
                    }
                    else
                    {
                        Toast.MakeText(this, "The source location does not exist.", ToastLength.Long).Show();
                    }
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                }
            };
        }
    }
}

