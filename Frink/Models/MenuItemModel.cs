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


        public string   name { get; set; }
        public string   icon { get; set; }
        public string   id { get; set; }
        public string   createdAt { get; set; }
        public string   updatedAt { get; set; }
        public string   layout { get; set; }
        public string   source { get; set; }
        public string   type { get; set; }


        #endregion
        #region CLASS CONSTRCUT



        public MenuItemModel() { }


        public MenuItemModel (string _name, string _icon, string _id, string _createdAt, string _updatedAt, string _layout, string _source, string _type)
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
