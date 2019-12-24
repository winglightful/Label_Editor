using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Bartector_Label_Editor
{
    [Serializable]
    internal abstract class PrintObject_Base: INotifyPropertyChanged
    {
        /// <summary>
        ///  Name can be search
        ///  Do not set same name
        /// </summary>
        [Description("Name")]
        public  string Name { get; set; }
    
        protected System.Windows.Point _location;
        /// <summary>
        ///  Location in graphic
        /// </summary>
        public System.Windows.Point Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
                OnpropertyChanged("Location");
            }
        }
        /// <summary>
        /// Data
        /// </summary>
        private string _data;
        public  string Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
                OnpropertyChanged("Data");
            }
        }

        /// <summary>
        /// Index 
        /// </summary>
        public int _Index;
        public int Index
        {
            get
            {
                return _Index;
            }
        }

        /// <summary>
        /// ImageSize of object
        /// </summary>
        protected System.Windows.Size _imagepixelSize;
        public  System.Windows.Size ImagepixelSize
        {
            get
            {
                return _imagepixelSize;
            }
        }


        protected EnumRotate _rotate;
        public EnumRotate Rotate
        {
            get
            {
                return _rotate;
            }
            set
            {
                _rotate = value;
                OnpropertyChanged("Rotate");
            }
        }

        /// <summary>
        /// Function for paint in graphic
        /// </summary>
        /// <returns></returns>
        public abstract  System.Drawing.Bitmap Paint();

        [field: NonSerialized()]
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnpropertyChanged(string propertyName)
        {
            if (propertyName != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public enum EnumFontStyle
        {
            Bold = 1,
            Italic = 2,
            Regular = 0,
            Strikeout = 8,
            Underline = 4
        }

        public enum EnumRotate
        {
            _90 = 90,
            _180 = 180,
            _270 = 270,
            _0 = 0
        }
    }
}
