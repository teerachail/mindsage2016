using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Engines.Models
{
    public enum PaymentResult
    {
        Unknow,
        created,
        approved,
        failed,
        canceled,
        expired,
        pending
    }
}
