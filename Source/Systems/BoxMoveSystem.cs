    using System;
    using System.Collections.Generic;
    using System.Text;
    using Urho;
    using Urho.Actions;
    using Urho.Shapes;

namespace CardGameExample
{
    /*
     * Example of a system to get a collection of Nodes & perform actions on them
     */
    public class BoxMoveSystem : NodeProcessingSystem
    {
        const float duration = 1f;                  //2 second
        public BoxMoveSystem(SceneComponent _scene, string _tag, bool _recursive = false) : base(_scene, _tag, _recursive)
        {

        }
        public override void Process()
        {
            base.Process();
            //
            // Get a collection of nodes matching the "Tag" criteria
            //
            Nodes = Scene.MyScene.GetChildrenWithTag(Tag, Recursive);
            foreach (Node nd in Nodes)
            {
                FiniteTimeAction action = null;
                if (Scene.Input.GetKeyPress(Key.E))
                    action = new FadeIn(duration);

                if (Scene.Input.GetKeyPress(Key.Q))
                    action = new FadeOut(duration);

                if (Scene.Input.GetKeyPress(Key.R))
                    nd.SetScale(2.0f);

                if (Scene.Input.GetKeyPress(Key.G))
                    action = new TintTo(duration, Global.NextRandom(1), Global.NextRandom(1), Global.NextRandom(1));

                if (action != null)
                {
                    //
                    // perform the timed action
                    //
                    nd.RunActionsAsync(action);
                }
            }
        }
    }
}
