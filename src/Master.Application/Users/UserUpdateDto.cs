using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Users
{
    public class UserUpdateDto
    {
        public int Id { get; set; }
        public string WorkLocation { get; set; }
        public int? AnYouId { get; set; }
    }
}
