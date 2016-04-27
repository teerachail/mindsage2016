using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebManagementPortal.Repositories.Models
{
    public class UserProfile
    {
        #region Properties

        public string id { get; set; }
        public string Name { get; set; }
        public string SchoolName { get; set; }
        public string ImageProfileUrl { get; set; }
        public bool IsPrivateAccount { get; set; }
        public bool IsEnableNotification { get; set; }
        public ReminderFrequency CourseReminder { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        public IEnumerable<Subscription> Subscriptions { get; set; }

        #endregion Properties

        public enum ReminderFrequency
        {
            Once, Twice
        }

        public class Subscription
        {
            #region Properties

            public string id { get; set; }
            public AccountRole Role { get; set; }
            public string ClassRoomName { get; set; }
            public string CourseCatalogId { get; set; }
            public string ClassRoomId { get; set; }
            public string ClassCalendarId { get; set; }
            public string LicenseId { get; set; }
            public DateTime? LastActiveDate { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime? DeletedDate { get; set; }

            #endregion Properties
        }

        public enum AccountRole
        {
            Teacher, Student, SelfPurchaser
        }
    }
}
