using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HuangcdLib.Parser
{
    public interface TokenParser
    {
        Token Next { get; }
        StreamReader Stream { set; }
    }
}
