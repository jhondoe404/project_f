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
        public String image { get; set; }
        public String type { get; set; }
        public String id { get; set; }
        public String createdAt { get; set; }
        public String updatedAt { get; set; }

        public String name { get; set; }

        public String imageCaption { get; set; }



        #endregion
        #region CLASS CONSTRUCT



        public MenuModel() {}


        public MenuModel(ObservableCollection<MenuItemModel> _items, String _image, String _type, String _id, String _createdAt, String _updatedAt, String _name, String _imageCaption)
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
