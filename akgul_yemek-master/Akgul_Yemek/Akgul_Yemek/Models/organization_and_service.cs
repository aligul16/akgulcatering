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
    
    public partial class organization_and_service
    {
        public int id { get; set; }
        public int organization_information_id { get; set; }
        public int organization_service_id { get; set; }
        public string information { get; set; }
        public int price { get; set; }
    
        public virtual organization_information organization_information { get; set; }
        public virtual organization_service organization_service { get; set; }
    }
}