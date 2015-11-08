using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frink.Models
{
    class ThemeApplicationModel
    {
        #region CLASS PARAMETERS



        public ColorScheme  colorScheme { get; set; }
        public String       name { get; set; }
        public String       description { get; set; }
        public int          id { get; set; }
        public String       createdAt { get; set; }
        public String       updatedAt { get; set; }
        public String       titleFont { get; set; }
        public String       bodyFont { get; set; }   



        #endregion
        #region CLASS CONSTRUCT



        public ThemeApplicationModel ()  { }


        public ThemeApplicationModel (ColorScheme _colorScheme, String _name, String _description, int _id, String _createdAt, String _updatedAt, String _titleFont, String _bodyFont)
        {
            this.colorScheme =  _colorScheme;
            this.name =         _name;
            this.description =  _description;
            this.id =           _id;
            this.createdAt =    _createdAt;
            this.updatedAt =    _updatedAt;
            this.titleFont =    _titleFont;
            this.bodyFont =     _bodyFont;
        }



        #endregion
    }
}
