# Urho.Net CardGame Example
This is a simple solitaire card game to show usage of Urho.Net in creating a 2D game. 

https://github.com/Urho-Net/Urho.Net 

I also like the ECS framework for games. So, I took the "Node" in the scene to be an Entity and created a node processing system to manage the system part of ECS.

Using the "SceneComponent" from https://github.com/Urho-Net/Samples I added a node processing loop to manage the systems.  This allows me to use "AddNodeProcessing" method to separate updates of so many nodes.  This makes the original scene cleaner. So to repeat :-)

Entity = Urho Node
Component = Urho Component
System = My NodeProcessingSystem

## Sample Screen

![game image](CardGameScreen.png)
