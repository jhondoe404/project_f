using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frink.Models
{
    class ImageModel
    {
        #region CLASS PARAMETERS



        public string title { get; set; }
        public string caption { get; set; }
        public string picture { get; set; }



        #endregion
        #region CLASS CONSTRUCT



        public ImageModel (string title, string caption, string picture)
        {
            this.title = title;
            this.caption = caption;
            this.picture = picture;
        }



        #endregion
    }
}
