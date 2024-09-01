using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ltly.Lambda.Domain.Primitives;

namespace Ltly.Lambda.Domain.Urls
{
    internal class UrlErrors
    {
        public static readonly Error InvalidUrlError = new("Provided invalid URL");
    }
}
