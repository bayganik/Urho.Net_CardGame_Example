using System;
using System.Collections.Generic;
using System.Text;
using Urho;

namespace CardGameExample
{
    public class DragFromStack : Component
    {
        public Node NodeOrig;                   //Stack Entity cards came from

        public DragFromStack(Node _node)
        {
            NodeOrig = _node;
        }
        //public override void OnAttachedToNode(Node _node)
        //{
        //    NodeOrig = _node;
        //}
    }
}
