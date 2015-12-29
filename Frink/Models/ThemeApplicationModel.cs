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
        public string       name { get; set; }
        public string       description { get; set; }
        public int          id { get; set; }
        public string       createdAt { get; set; }
        public string       updatedAt { get; set; }
        public string       titleFont { get; set; }
        public string       bodyFont { get; set; }   



        #endregion
        #region CLASS CONSTRUCT



        public ThemeApplicationModel ()  { }


        public ThemeApplicationModel (ColorScheme _colorScheme, string _name, string _description, int _id, string _createdAt, string _updatedAt, string _titleFont, string _bodyFont)
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
