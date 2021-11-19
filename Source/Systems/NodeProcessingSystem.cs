using System;
using System.Collections.Generic;
using System.Text;
using Urho;

namespace CardGameExample
{
    public class NodeProcessingSystem
    {
        /*
         * The system part of ECS framework. Executed on every update frame
         * Allows for processing of Nodes outside of Scene component for separation of concerns
         *   
         * You can use the Tag property to group Nodes together or use Component to group them
         * See the Process() method
         */
        public SceneComponent Scene;
        public Node[] Nodes;
        public string Tag;
        public bool Recursive;
        public NodeProcessingSystem(SceneComponent _scene, string _tag, bool _recursive = false)
        {
            Tag = _tag;
            Scene = _scene;
            Recursive = _recursive;
        }
        public virtual void Process()
        {
            //Nodes = Scene.MyScene.GetChildrenWithComponent<CardInfo>();
            //Nodes = Scene.MyScene.GetChildrenWithTag(Tag, Recursive);
            //foreach(Node nd in Nodes)
            //{

            //}
        }
    }
}
