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
    
    public partial class FCMNotification
    {
        public long FCMNotificationId { get; set; }
        public Nullable<long> UserID { get; set; }
        public Nullable<int> NotificationCategoryId { get; set; }
        public string Title { get; set; }
        public string NotificationType { get; set; }
        public string NotificationTypePath { get; set; }
        public string NotificationTypeText { get; set; }
        public string NotificationCode { get; set; }
        public Nullable<bool> IsSent { get; set; }
        public Nullable<System.DateTime> SentDate { get; set; }
        public string TransactionId { get; set; }
        public string FCMResponse { get; set; }
        public Nullable<bool> IsRead { get; set; }
        public Nullable<System.DateTime> ReadDate { get; set; }
        public Nullable<int> RetryCount { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
    
        public virtual NotificationCategory NotificationCategory { get; set; }
    }
}
