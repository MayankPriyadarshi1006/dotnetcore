//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Mashi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class StoreCategory
    {
        public StoreCategory()
        {
            this.Stores = new HashSet<Store>();
        }
    
        public int StoreCategoryId { get; set; }
        public string StoreCategoryCode { get; set; }
        public string StoreCategoryName { get; set; }
        public string AbStoreCategoryName { get; set; }
        public bool IsEnabled { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> ParentStoreCategoryId { get; set; }
    
        public virtual ICollection<Store> Stores { get; set; }
    }
}
