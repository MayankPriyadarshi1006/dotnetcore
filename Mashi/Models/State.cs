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
    
    public partial class State
    {
        public State()
        {
            this.Cities = new HashSet<City>();
            this.Stores = new HashSet<Store>();
        }
    
        public short StateId { get; set; }
        public string StateName { get; set; }
        public string AbStateName { get; set; }
        public Nullable<short> CountryId { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    
        public virtual ICollection<City> Cities { get; set; }
        public virtual Country Country { get; set; }
        public virtual ICollection<Store> Stores { get; set; }
    }
}
