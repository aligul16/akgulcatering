//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Akgul_Yemek.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class organization_type
    {
        public organization_type()
        {
            this.organization_information = new HashSet<organization_information>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
        public string color { get; set; }
    
        public virtual ICollection<organization_information> organization_information { get; set; }
    }
}
