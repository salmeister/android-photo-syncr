using System;
using System.Collections.Generic;
using System.IO;
using SQLite.Net;

namespace android_photo_syncr.Tools
{
    class SQLDB
    {
        private SQLiteConnection _db { get; set; }
        public SQLDB(string dbFilePath)
        {
            bool exists = false;
            
            if (File.Exists(dbFilePath))
            {
                exists = true;
            }
            var platform = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();
            _db = new SQLiteConnection(platform, dbFilePath);
            if (!exists)
            {
                InitializeDB();
            }
        }

        private void InitializeDB()
        {
            _db.CreateTable<Picture>();
        }

        internal IEnumerable<Picture> GetExistingPictures()
        {
            return _db.Table<Picture>();
        }

        internal int InsertNewPicture(Picture pic)
        {
            return _db.Insert(pic);
        }
    }
}