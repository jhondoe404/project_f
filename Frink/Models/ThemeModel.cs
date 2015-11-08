using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frink.Models
{
    class ThemeModel
    {
        #region CLASS PARAMETERS



        public ThemeApplicationModel    app { get; set; }
        public MenuModel                menu { get; set; }



        #endregion
        #region CLASS CONSTRUCT



        public ThemeModel() { }

        public ThemeModel(ThemeApplicationModel _app, MenuModel _menu)
        {
            this.app =  _app;
            this.menu = _menu;
        }



        #endregion
    }
}
