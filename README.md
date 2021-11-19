# Urho.Net CardGame Example
This is a simple solitaire card game to show usage of Urho.Net in creating a 2D game. 

https://github.com/Urho-Net/Urho.Net 

I also like the ECS framework for games. So, I took the "Node" in the scene to be an Entity and created a node processing system to manage the system part of ECS.

Using the "SceneComponent" from https://github.com/Urho-Net/Samples I added a node processing loop to manage the systems.  This allows me to use "AddNodeProcessing" method to separate updates of so many nodes.  This makes the original scene cleaner. So to repeat :-)

Entity = Urho scene Node

Component = Urho scene Component

System = My NodeProcessingSystem

## SceneComponent
        protected override void OnUpdate(float timeStep)
        {
            //MoveCameraByTouches(timeStep);
            base.OnUpdate(timeStep);
            Global.DeltaTime = timeStep;
            //
            // Process node systems
            //
            if (NodeSystems.Count > 0)
            {
                foreach (NodeProcessingSystem ns in NodeSystems)
                    ns.Process();
            }
        }
        protected void AddNodeProcessing(NodeProcessingSystem _system)
        {
            NodeSystems.Add(_system);
        }

## Sample Screen

![game image](CardGameScreen.png)
