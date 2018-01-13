using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcApplication1
{
    class UserToken
    {
        public string userName { get; set; }
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
        public string issued { get; set; }
        public string expires { get; set; }
    }
}
