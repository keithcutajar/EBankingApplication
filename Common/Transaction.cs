//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Common
{
    using System;
    using System.Collections.Generic;
    
    public partial class Transaction
    {
        public int Id { get; set; }
        public long AccountNo_Fk { get; set; }
        public string Details { get; set; }
        public string Currency_Fk { get; set; }
        public decimal Amount { get; set; }
        public System.DateTime Transaction_Timestamp { get; set; }
    
        public virtual Currency Currency { get; set; }
    }
}
