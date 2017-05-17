using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheBucketList.Models
{
    public class BucketItemsModel
    {
        public List<BucketItemModel> BucketItems { get; set; }

        public BucketItemsModel()
        {
            BucketItems = new List<BucketItemModel>();
        }
    }
}