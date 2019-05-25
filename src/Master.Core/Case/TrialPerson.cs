using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Case
{
    public class TrialPerson
    {
        public string Name { get; set; }
        public TrialRole TrialRole { get; set; }
    }
    public enum TrialRole
    {
        审判长,
        审判员,
        书记员
    }
}
