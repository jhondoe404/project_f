using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frink.Models
{
    class ContentItemModel
    {
        #region CLASS PARAMETERS



        public string   title { get; set; }
        public string   description { get; set; }
        public string   id { get; set; }
        public string   createdAt { get; set; }
        public string   updatedAt { get; set; }
        public int      total { get; set; }
        public ObservableCollection<ImageModel> images { get; set; }
        public string   text { get; set; }
        public string   author { get; set; }
        public string   picture { get; set; }


        #endregion
        #region CLASS CONSTRUCT



        public ContentItemModel(string title, string descrption, string id, string createdAt, string updatedAt, int total, ObservableCollection<ImageModel> images)
        {
            this.title = title;
            this.description = description;
            this.id = id;
            this.createdAt = createdAt;
            this.updatedAt = updatedAt;
            this.total = total;
            this.images = images;
        }



        #endregion
    }
}
