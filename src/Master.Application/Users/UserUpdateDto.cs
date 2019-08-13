using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Users
{
    public class UserUpdateDto
    {
        public int Id { get; set; }
        public string WorkLocation { get; set; }
        public int[] AnYouIds { get; set; }
        public string Name { get; set; }
        public int WorkYear { get; set; }
        public string Introduction { get; set; }
        public string Avata { get; set; }
    }
}
