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
    
    public partial class organization_information
    {
        public organization_information()
        {
            this.organization_and_food = new HashSet<organization_and_food>();
            this.organization_and_service = new HashSet<organization_and_service>();
        }
    
        public int id { get; set; }
        public int organization_category_id { get; set; }
        public string organization_name { get; set; }
        public string name_surname { get; set; }
        public string email { get; set; }
        public string telephone { get; set; }
        public string telephone2 { get; set; }
        public System.DateTime date { get; set; }
        public string time { get; set; }
        public int people_count { get; set; }
        public string referance { get; set; }
        public int down_payment { get; set; }
        public string organizators_adress { get; set; }
        public string organization_adress { get; set; }
        public int organization_status { get; set; }
        public int total_price { get; set; }
        public int discount { get; set; }
        public int count_cook { get; set; }
        public int count_chef { get; set; }
        public int count_waiter { get; set; }
        public int count_other { get; set; }
    
        public virtual ICollection<organization_and_food> organization_and_food { get; set; }
        public virtual ICollection<organization_and_service> organization_and_service { get; set; }
        public virtual organization_type organization_type { get; set; }
    }
}
