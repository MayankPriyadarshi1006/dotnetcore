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
    
    public partial class SchemeType
    {
        public SchemeType()
        {
            this.SchemeDocuments = new HashSet<SchemeDocument>();
        }
    
        public int SchemeTypeId { get; set; }
        public string SchemeTypeCode { get; set; }
        public string SchemeTypeName { get; set; }
        public string AbSchemeTypeName { get; set; }
        public bool IsEnabled { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual ICollection<SchemeDocument> SchemeDocuments { get; set; }
    }
}
