using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frink.Models
{
    class MenuItemModel
    {
        #region CLASS PARAMETERS


        public String   name { get; set; }
        public String   icon { get; set; }
        public String   id { get; set; }
        public String   createdAt { get; set; }
        public String   updatedAt { get; set; }
        public String   layout { get; set; }
        public String   source { get; set; }
        public String   type { get; set; }


        #endregion
        #region CLASS CONSTRCUT



        public MenuItemModel() { }


        public MenuItemModel (String _name, String _icon, String _id, String _createdAt, String _updatedAt, String _layout, String _source, String _type)
        {
            this.name =         _name;
            this.icon =         _icon;
            this.id =           _id;
            this.createdAt =    _createdAt;
            this.updatedAt =    _updatedAt;
            this.layout =       _layout;
            this.source =       _source;
            this.type =         _type;
        }



        #endregion
    }
}
