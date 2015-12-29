using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frink.Models
{
    class MenuModel
    {
        #region CLASS PARAMETERS



        public ObservableCollection<MenuItemModel> items { get; set; }
        public string image { get; set; }
        public string type { get; set; }
        public string id { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }

        public string name { get; set; }

        public string imageCaption { get; set; }



        #endregion
        #region CLASS CONSTRUCT



        public MenuModel() {}


        public MenuModel(ObservableCollection<MenuItemModel> _items, string _image, string _type, string _id, string _createdAt, string _updatedAt, string _name, string _imageCaption)
        {
            this.items = _items;
            this.image = _image;
            this.type = _type;
            this.id = _id;
            this.createdAt = _createdAt;
            this.updatedAt = _updatedAt;
            this.name = _name;
            this.imageCaption = _imageCaption;
        }



        #endregion
    }
}
