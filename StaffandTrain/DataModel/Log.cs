//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StaffandTrain.DataModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Log
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public Nullable<int> RecordId { get; set; }
        public System.DateTime CreateAt { get; set; }
        public string Status { get; set; }
    }
}
