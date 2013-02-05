using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HuangcdLib.Parser
{
    public abstract class Token
    {
        public abstract char[] Chars {get; }
        public abstract String String { get; }
    }

    public enum TokenClass
    {
        Keyword,
        Identify,
        Operator,

    }
}
