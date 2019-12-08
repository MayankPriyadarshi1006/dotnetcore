using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mashi.Areas.Store.Models.StoreCategory
{
    public class StoreCategoryModel
    {
        public int? storeCategoryId { get; set; }

        [Required(ErrorMessage = "Enter Store Category Code")]
        public string storeCategoryCode { get; set; }

        [Required(ErrorMessage = "Enter Store Category Name")]
        public string storeCategoryName { get; set; }

        [Required(ErrorMessage = "Enter Store Category Name")]
        public string storeCategoryNameAB { get; set; }

        public bool? IsEnabled { get; set; }
        public int? ParentStoreCategoryId { get; set; }
    }
}