using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frink.Models
{
    class ColorScheme
    {
        #region CLASS PARAMETERS


        public float[] color1 { get; set; }
        public float[] color2 { get; set; }
        public float[] color3 { get; set; }


        #endregion
        #region CLASS CONSTRUCT


        public ColorScheme() { }


        public ColorScheme(float[] _color1, float[] _color2, float[] _color3)
        {
            this.color1 = _color1;
            this.color2 = _color2;
            this.color3 = _color3;
        }


        #endregion

    }
}
