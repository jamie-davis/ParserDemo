using System.Collections.Generic;

namespace Parser.Parsing
{
    internal abstract class ExpressionNode
    {
        internal abstract string Describe();

        internal abstract IEnumerable<ExpressionNode> ContainedNodes();
      
        internal abstract decimal Compute();
    }
}