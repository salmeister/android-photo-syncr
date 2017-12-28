
using SQLite.Net.Attributes;
using System.ComponentModel;

namespace android_photo_syncr.Tools
{
    class Picture
    {
        private string _fileName;
        [NotNull]
        public string fileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                this._fileName = value;
                OnPropertyChanged(nameof(fileName));
            }
        }
        private string _path;
        [NotNull]
        public string path
        {
            get
            {
                return _path;
            }
            set
            {
                this._path = value;
                OnPropertyChanged(nameof(path));
            }
        }

        private bool _uploaded;
        [NotNull]
        public bool uploaded
        {
            get
            {
                return _uploaded;
            }
            set
            {
                this._uploaded = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this,
              new PropertyChangedEventArgs(propertyName));
        }
    }
}