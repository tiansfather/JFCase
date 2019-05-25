using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Entity
{
    public interface IHaveAddress
    {
        string Country { get; set; }
        string Province { get; set; }
        string District { get; set; }
        string Address { get; set; }
    }
}
