using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilation.bn.syntax.synnode
{
    public class ProgramNode : SyntaxNode
    {
        public List<SyntaxNode> funList = new List<SyntaxNode>();
        public List<SyntaxNode> arrayList = new List<SyntaxNode>();

        public void addFun(SyntaxNode fun)
        {
            funList.Add(fun);
        }

        public void addArray(SyntaxNode array)
        {
            arrayList.Add(array);
        }

        public string toString()
        {
            return " Program " + "\n";
        }

        public IEnumerable<SyntaxNode> getChild()
        {
            // 返回当前节点的所有子节点：先funList后arrayList
            foreach (var fun in funList)
            {
                yield return fun;
            }
            foreach (var array in arrayList)
            {
                yield return array;
            }
        }
    }
}
