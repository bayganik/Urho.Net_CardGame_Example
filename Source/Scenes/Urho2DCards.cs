using System;
using Urho;
using System.Collections.Generic;
using System.Linq;
using Urho.Urho2D;
using Urho.Gui;


namespace CardGameExample
{
    /*
     * Example of using 2D spritesheet to create playing cards
     *      see Data/Cards/CardDeck.xml for detail (must have)
     *      
     * SceneComponent provides the camera/light/zone and viewport
     * you can use PageUp/PageDown to Zoom in/out
     */
    public class Urho2DCards : SceneComponent
    {
        CardDeckManager CDM;
        //
        // Card Stacks starting locations
        //
        Vector2 playStackPos = new Vector2(-4, 2);
        Vector2 dealStackPos = new Vector2(-4, 3.25f);          
        Vector2 drawStackPos = new Vector2(-3, 3.25f);          
        Vector2 aceStackPos = new Vector2(-1, 3.25f);           

        List<Node> PlayStacks;                              //7 stacks 
        List<Node> AceStacks;                               //face up on stack (4 stacks)
        Node DealStack;                                     //face down on stack
        Node DrawStack;                                     //face up on stack
        Node DragStack;                                     //face up on stack

        public PhysicsWorld2D Physics2D
        {
            get
            {
                return MyScene.GetComponent<PhysicsWorld2D>();
            }
        }

        [Preserve]
        public Urho2DCards() : base(new ApplicationOptions(assetsFolder: "Data;CoreData")) { }

        protected override void Start()
        {
            base.Start();
            CreateScene();

            Input.SetMouseVisible(true);
            ClearColor = Color.Gray;
            SetupViewport();

            //Engine.PostRenderUpdate += OnPostRender;
        }

        protected override void OnUpdate(float timeStep)
        {
            base.OnUpdate(timeStep);    //must do this so systems get executed
        }
        private void OnPostRender(PostRenderUpdateEventArgs obj)
        {

            // If draw debug mode is enabled, draw viewport debug geometry, which will show eg. drawable bounding boxes and skeleton
            // bones. Note that debug geometry has to be separately requested each frame. Disable depth test so that we can see the
            // bones properly
            //if (drawDebug)
            //MyScene.GetComponent<PhysicsWorld2D>().DrawDebugGeometry();
        }
        /// <summary>
        /// Get mouse 2D mouse position in 3D vector
        /// </summary>
        public Vector2 GetMousePositionXY(float mouseX, float mouseY)
        {
            Input input = Input;
            Graphics graphics = Graphics;
            Vector3 screenPoint = new Vector3(mouseX / graphics.Width, mouseY / graphics.Height, 0.0f);

            Vector3 worldPoint = Camera.ScreenToWorldPoint(screenPoint);
            return new Vector2(worldPoint.X, worldPoint.Y);
        }
        /// <summary>
        /// Get node sitting on mouse position 2D (screen pixel)
        /// </summary>
        public Node GetMousePositionNode(float mouseX, float mouseY)
        {
            Node nodeResult = null;
            IntVector2 pos = new IntVector2((int)mouseX, (int)mouseY);
            Ray cameraRay = Camera.GetScreenRay((float)pos.X / Graphics.Width, (float)pos.Y / Graphics.Height);
            //
            // Search thru Octree for the Ray to hit which node
            //
            var result = MyScene.GetComponent<Octree>().RaycastSingle(cameraRay, RayQueryLevel.Triangle, 250, DrawableFlags.Geometry, uint.MaxValue);
            if (result != null)
            {
                nodeResult = result.Value.Node;
            }
            //
            // returns null, if nothing is found
            //
            return nodeResult;
        }

        public Node DealNormalCard(bool _faceUp = true)
        {
            CardInfo card = CDM.DealCard();
            if (card == null)
                return null;            //end of card deck

            Node spriteNode = MyScene.CreateChild("cardFace");
            spriteNode.Name = "card " + card.Index.ToString();
            spriteNode.AddTag("card");
            //
            // Sprite layer changes depending how the cards fan out
            //
            StaticSprite2D tmp = spriteNode.CreateComponent<StaticSprite2D>();
            tmp.Sprite = card.FaceImage.Sprite;
            spriteNode.AddComponent(card);    //cardInfo

            return spriteNode;
        }
        public Node DealOtherCard(bool _faceUp, string _type)
        {
            CardInfo card = CDM.DealOtherCard(_type);
            if (card == null)
                return null;            //end of card deck

            Node spriteNode = MyScene.CreateChild(_type);
            spriteNode.Name = "card " + _type;
            spriteNode.AddTag("card");
            //
            // Sprite layer changes depending how the cards fan out
            //
            StaticSprite2D tmp = spriteNode.CreateComponent<StaticSprite2D>();
            tmp.Sprite = card.FaceImage.Sprite;

            spriteNode.AddComponent(card);    //cardInfo

            return spriteNode;
        }
        void CreateScene()
        {
            //
            // Create 2D physics world component, so we can use RigidBody2D for 
            // picking cards with mouse
            //
            Global.PhysicsWorld = MyScene.CreateComponent<PhysicsWorld2D>();
            Global.PhysicsWorld.DrawJoint = true;
            Global.PhysicsWorld.Gravity = new Vector2(0f, 0f);

            //
            // Setup the camera
            //
            CameraNode.Position = (new Vector3(0.0f, 0.0f, -10.0f));
            Camera.Orthographic = true;

            Camera.OrthoSize = (float)Graphics.Height * Application.PixelSize;
            //
            // CardStack area
            //
            PlayStacks = new List<Node>();
            AceStacks = new List<Node>();
            DealStack = new Node();
            DrawStack = new Node();
            DragStack = new Node();

            PlayStacks.Add(CreatePlayStack("1", playStackPos));
            playStackPos.X += 1;
            PlayStacks.Add(CreatePlayStack("2", playStackPos));
            playStackPos.X += 1;
            PlayStacks.Add(CreatePlayStack("3", playStackPos));
            playStackPos.X += 1;
            PlayStacks.Add(CreatePlayStack("4", playStackPos));
            playStackPos.X += 1;
            PlayStacks.Add(CreatePlayStack("5", playStackPos));
            playStackPos.X += 1;
            PlayStacks.Add(CreatePlayStack("6", playStackPos));
            playStackPos.X += 1;
            PlayStacks.Add(CreatePlayStack("7", playStackPos));

            AceStacks.Add(CreateEmptyStack("acestack", "30", aceStackPos));
            aceStackPos.X += 1;
            AceStacks.Add(CreateEmptyStack("acestack", "40", aceStackPos));
            aceStackPos.X += 1;
            AceStacks.Add(CreateEmptyStack("acestack", "50", aceStackPos));
            aceStackPos.X += 1;
            AceStacks.Add(CreateEmptyStack("acestack", "60", aceStackPos));
            aceStackPos.X += 1;

            DrawStack = CreateEmptyStack("drawstack", "80", drawStackPos);      //all face up
            DealStack = CreateEmptyStack("dealstack", "90", dealStackPos);      //all face down
            //
            // DragStack is for disp of cards being moved (no collisions needed)
            //
            DragStack = CreateEmptyStack("dragstack", "100", Vector2.Zero);     //all face up
            DragStack.RemoveAllComponents();
            DragStack.CreateComponent<CardStack>();
            DragFromStack ds = new DragFromStack(null);
            DragStack.AddComponent(ds);
            //
            // Setup the card management/draw
            //
            CDM = new CardDeckManager();
            Deal_Init_Stacks();
            //
            // Add systems for action/update
            //
            AddNodeProcessing(new DispAceStackSystem(this, "acestack"));
            AddNodeProcessing(new DispPlayStackSystem(this, "playstack"));
            AddNodeProcessing(new DispDealStackSystem(this, "dealstack"));
            AddNodeProcessing(new DispDrawStackSystem(this, "drawstack"));
            AddNodeProcessing(new DispDragStackSystem(this, "dragstack"));
            AddNodeProcessing(new MouseClickSystem(this, "mouse"));
            //
            // teaching system for moving camera (not needed for game)
            //
            AddNodeProcessing(new Camera2DSimpleMoveSystem(this, "camera"));
        }
        public GameState GameState_Check()
        {
            int stackWith13 = 0;
            foreach(Node aceStack in AceStacks)
            {
                CardStack scPlay = aceStack.GetComponent<CardStack>();
                if (scPlay.CardsInStack.Count != 13)
                    break;
                //
                // we have 13 cards in this stack
                //
                stackWith13 += 1;
            }
            //
            // if all ace stacks have 13 cards, then game is over
            //
            if (stackWith13 == 4)
            {
                var helloText = new Text()
                {
                    Value = "Game is Over",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                helloText.SetColor(new Color(0f, 1f, 0f));
                helloText.SetFont(font: Global.ResourceCache.GetFont("Fonts/Anonymous Pro.ttf"), size: 30);

                UI.Root.AddChild(helloText);
                return GameState.GS_END;
            }
            return GameState.GS_PLAY;
        }
        //####################################################
        //            Initial deal of cards
        void Deal_Init_Stacks()
        {
          
            Node drawnCard;
            CardStack sc;
            //
            // 7 play stacks
            //
            for (int i = 0; i < PlayStacks.Count; i++)
            {
                sc = PlayStacks[i].GetComponent<CardStack>();
                sc.CardsInStack.Clear();
                for (int j = 0; i >= j; j++)
                {
                    //
                    // Create a card Enitty
                    //
                    drawnCard = DealNormalCard(true);
                    drawnCard.Enabled = false;
                    CardInfo ci = drawnCard.GetComponent<CardInfo>();
                    if (i == j)
                        ci.IsFaceUp = true;             //last card on stack
                    else
                        ci.IsFaceUp = false;

                    ci.HoldingStack = sc;
                    sc.CardsInStack.Add(drawnCard);
                }
            }
            //
            // Deal stack
            //
            sc = DealStack.GetComponent<CardStack>();
            sc.CardsInStack.Clear();
            for (int i = 0; i < 52; i++)
            {
                //
                // Create a card Enitty
                //
                drawnCard = DealNormalCard(true);
                if (drawnCard == null)
                    break;      //end of deck

                CardInfo ci = drawnCard.GetComponent<CardInfo>();
                ci.IsFaceUp = false;
                ci.HoldingStack = sc;
                sc.CardsInStack.Add(drawnCard);
            }
        }
        //####################################################
        //       Return cards to their original stacks
        public void ReturnCardFromDrag2Stack()
        {
            DragFromStack scDragComp = DragStack.GetComponent<DragFromStack>();
            if (scDragComp == null)
                return;

            Node fromEntity = scDragComp.NodeOrig;
            if (fromEntity == null)
                return;

            CardStack scDrag = DragStack.GetComponent<CardStack>();            //cards being dragged
            CardStack scFrom = fromEntity.GetComponent<CardStack>();            //cards to give back
            if (scFrom == null)
                return;

            for (int i = 0; i < scDrag.CardsInStack.Count; i++)
            {
                scFrom.CardsInStack.Add(scDrag.CardsInStack[i]);
            }
            DragStackClear();

        }
        private void DragStackClear()
        {
            //
            // Clear out the DragStack
            //
            CardStack sc = DragStack.GetComponent<CardStack>();
            sc.CardsInStack = new List<Node>();
        }
        //####################################################
        //         Drop cards
        //
        public void DropCardFromDrag2AceStack(Node _aceStack)
        {
            CardStack scDrag = DragStack.GetComponent<CardStack>();            //cards being dragged

            if (scDrag.CardsInStack.Count != 1)
            {
                ReturnCardFromDrag2Stack();
                return;
            }
            DragFromStack scDragComp = DragStack.GetComponent<DragFromStack>();
            if (scDragComp.NodeOrig == _aceStack)
            {
                ReturnCardFromDrag2Stack();
                return;
            }

            CardStack scPlay = _aceStack.GetComponent<CardStack>();
            //
            // first card of drag needs to match last card of drop
            //
            Node firstCardonStack = scDrag.CardsInStack[0];               //get first card of drag
            CardInfo firstCard = firstCardonStack.GetComponent<CardInfo>();
            //
            // Make sure this drop stack is not empty
            //
            if (scPlay.CardsInStack.Count == 0)
            {
                if (firstCard.FaceNumber != 1)          //only an ACE will sit on empty stack
                {
                    ReturnCardFromDrag2Stack();
                    return;
                }
                //
                //  first card of drap is ACE, drop all of them
                //
                for (int i = 0; i < scDrag.CardsInStack.Count; i++)
                {
                    scPlay.CardsInStack.Add(scDrag.CardsInStack[i]);
                    //CardDeckManager.score += scDrag.GetCardFaceImageValue(i);       //card added, calc score
                }
                DragStackClear();
                return;
            }
            //
            // play stack is NOT empty, test cards
            //
            Node lastCardonStack = scPlay.CardsInStack.LastOrDefault();               //get last card
            CardInfo lastCard = lastCardonStack.GetComponent<CardInfo>();
            if (TestCardsForAceStack(firstCard, lastCard))
            {
                for (int i = 0; i < scDrag.CardsInStack.Count; i++)
                {
                    scPlay.CardsInStack.Add(scDrag.CardsInStack[i]);
                    //CardDeckManager.score += scDrag.GetCardFaceImageValue(i);       //card added, calc score
                }
                DragStackClear();
            }
            else
            {
                ReturnCardFromDrag2Stack();
            }
        }
        public void DropCardFromDrag2PlayStack(Node _playStack)
        {
            CardStack scDrag = DragStack.GetComponent<CardStack>();            //cards being dragged
            CardStack scPlay = _playStack.GetComponent<CardStack>();            //cards being dropped
            if (scDrag.CardsInStack.Count == 0)
                return;
            //
            // first card of drag needs to match last card of drop
            //
            Node firstCardonStack = scDrag.CardsInStack[0];               //get first card
            CardInfo firstCard = firstCardonStack.GetComponent<CardInfo>();
            //
            // Make sure this stack is not empty
            //
            if (scPlay.CardsInStack.Count == 0)
            {
                if (firstCard.FaceNumber != 13)          //only a king will sit on empty stack
                {
                    ReturnCardFromDrag2Stack();
                    return;
                }
                //
                //  first card of drap is king, drop all of them
                //
                for (int i = 0; i < scDrag.CardsInStack.Count; i++)
                {
                    scPlay.CardsInStack.Add(scDrag.CardsInStack[i]);
                }
                //
                // set the holding stack for this card pile
                //
                for (int i = 0; i < scPlay.CardsInStack.Count; i++)
                {
                    CardInfo _cc = scPlay.CardsInStack[i].GetComponent<CardInfo>();
                    _cc.HoldingStack = scPlay;
                }
                DragStackClear();
                return;
            }
            //
            // play stack is NOT empty, test cards
            //
            Node lastCardonStack = scPlay.CardsInStack.LastOrDefault();               //get last card
            CardInfo lastCard = lastCardonStack.GetComponent<CardInfo>();
            if (TestCardsForPlayStack(firstCard, lastCard))
            {
                for (int i = 0; i < scDrag.CardsInStack.Count; i++)
                {
                    scPlay.CardsInStack.Add(scDrag.CardsInStack[i]);
                }
                //
                // set the holding stack for this card pile
                //
                for (int i = 0; i < scPlay.CardsInStack.Count; i++)
                {
                    CardInfo _cc = scPlay.CardsInStack[i].GetComponent<CardInfo>();
                    _cc.HoldingStack = scPlay;
                }
                DragStackClear();
            }
            else
            {
                ReturnCardFromDrag2Stack();
            }
        }
        //####################################################
        //               Drag card
        //
        public void DealOneCard2Drag(Node _stack)
        {
            //
            // cards come from either AceStack or DrawStack
            //
            CardStack scDisp = DragStack.GetComponent<CardStack>();         //face up
            CardStack scTemp = _stack.GetComponent<CardStack>();         //face down
            if (scTemp == null)
                return;
            if (scTemp.CardsInStack.Count == 0)
                return;
            Node lastCardonStack = scTemp.CardsInStack.LastOrDefault();     //last card face Up
            scTemp.CardsInStack.Remove(lastCardonStack);                    //remove it from dealstack

            scDisp.CardsInStack.Add(lastCardonStack);
            //
            // This is used to return the cards to their original stack
            //
            DragFromStack dc = DragStack.GetComponent<DragFromStack>();
            dc.NodeOrig = _stack;

        }
        public void DealOneCard2DrawStack()
        {
            //
            // take last card from face down DealStack place it on DrawStack
            //
            CardStack scDisp = DrawStack.GetComponent<CardStack>();         //face up
            CardStack scTemp = DealStack.GetComponent<CardStack>();         //face down
            if (scTemp == null)
                return;
            //
            // if deal deck is empty, put all face up cards back in deal deck
            //
            if (scTemp.CardsInStack.Count <= 0)
            {
                for (int i = scDisp.CardsInStack.Count - 1; i >= 0; i--)
                {
                    CardInfo _card = scDisp.CardsInStack[i].GetComponent<CardInfo>();
                    _card.IsFaceUp = false;
                    scTemp.CardsInStack.Add(scDisp.CardsInStack[i]);
                }
                scDisp.CardsInStack.Clear();
                return;
            }
            Node lastCardonStack = scTemp.CardsInStack.LastOrDefault();     //last card face down
            scTemp.CardsInStack.Remove(lastCardonStack);                    //remove it from dealstack

            CardInfo ci = lastCardonStack.GetComponent<CardInfo>();
            ci.IsFaceUp = true;

            scDisp.CardsInStack.Add(lastCardonStack);

        }
        //####################################################
        //          Take cards from play stack
        //
        public void TakePlayStackCards2Drag(Node _cardNode)
        {
            int cind;
            //
            // is the card face up?
            //
            CardInfo ci = _cardNode.GetComponent<CardInfo>();
            CardStack scTemp = ci.HoldingStack;

            if (!ci.IsFaceUp)
            {
                //
                // Card we clicked on is not face up, if its the last card in stack
                //    Then turn it face up
                //

                cind = scTemp.CardsInStack.FindIndex(x => x.ID == _cardNode.ID);
                if (cind == scTemp.CardsInStack.Count - 1)
                {
                    //
                    // last card in stack NOT face up
                    //
                    ci.IsFaceUp = true;
                }
                return;
            }

            //
            // Take card(s) from stack
            //
            Node fromStack = scTemp.Node;
            CardStack scDrag = DragStack.GetComponent<CardStack>();
            //
            // index of card player clicked on (in the CardsInStack)
            //
            cind = scTemp.CardsInStack.FindIndex(x => x.ID == _cardNode.ID);
            if (cind < 0)
                return;
            //
            // get it and all the cards below it
            //
            for (int i = cind; i <= scTemp.CardsInStack.Count - 1; i++)
            {
                Node lastCard = scTemp.CardsInStack[i];
                CardInfo cc = lastCard.GetComponent<CardInfo>();
                scDrag.CardsInStack.Add(lastCard);
                
            }
            //
            // remove cards from original play stack using DragStack (clever?)
            //
            for (int i = 0; i < scDrag.CardsInStack.Count; i++)
            {
                Node lastCard = scDrag.CardsInStack[i];
                scTemp.CardsInStack.Remove(lastCard);
            }
            //
            // This is used to return the cards to their original stack
            //
            DragFromStack dc = DragStack.GetComponent<DragFromStack>();
            dc.NodeOrig = fromStack;
        }
        //####################################################
        //          Test cards
        //
        public bool TestCardsForPlayStack(CardInfo dragCard, CardInfo dropCard)
        {
            bool result = false;

            if ((dragCard == null) || (dropCard == null))
                return false;

            //
            // Test colors
            //
            if ((dropCard.IsRed) && (dragCard.IsRed))               //both red
                return false;
            if ((!dropCard.IsRed) && (!dragCard.IsRed))             //both black
                return false;
            //
            // Face value test
            //
            if (dropCard.FaceNumber == dragCard.FaceNumber + 1)
                return true;

            return result;
        }
        public bool TestCardsForAceStack(CardInfo dragCard, CardInfo dropCard)
        {
            bool result = false;
            if ((dragCard == null) || (dropCard == null))
                return result;


            if (dropCard.Suit != dragCard.Suit)
                return result;
            //
            // Face value test
            //
            if (dropCard.FaceNumber == dragCard.FaceNumber - 1)
                result = true;

            return result;
        }
        //####################################################
        //          Card stack create
        //
        private Node CreatePlayStack(string _number, Vector2 _pos)
        {
            //###############################################################
            // create an empty playstack node
            //
            Node stackNode = MyScene.CreateChild("tempCardStack");
            stackNode.Name = _number;
            stackNode.AddTag("playstack");
            stackNode.SetPosition2D(_pos);
            //stackNode.Scale = new Vector3(1f, 4.20f, 0);
            Sprite2D sprite = ResourceCache.GetSprite2D("Cards/EmptyHolder.png");
            if (sprite == null)
                throw new InvalidOperationException("sprite not found");
            StaticSprite2D staticSprite = stackNode.CreateComponent<StaticSprite2D>();
            // Set random color
            staticSprite.Color = (new Color(Global.NextRandom(1.0f), Global.NextRandom(1.0f), Global.NextRandom(1.0f), 1.0f));
            staticSprite.BlendMode = BlendMode.Alpha;
            staticSprite.Sprite = sprite;

            //
            // physics (MUST be static so the cards don't bounce off of eachother)
            //
            RigidBody2D boxBody = stackNode.CreateComponent<RigidBody2D>();
            
            boxBody.BodyType = BodyType2D.Static;
            boxBody.Mass = 0;
            boxBody.LinearDamping = 0.0f;
            boxBody.AngularDamping = 0.0f;

            CollisionBox2D shape = stackNode.CreateComponent<CollisionBox2D>(); // Create box shape
            shape.Center = new Vector2(0, -2.5f);   //change the center to lower Y-coord
            shape.Size = new Vector2(0.7f, 6.0f);    // Set size of collision box 
            shape.Density = 0.0f;                   // Set shape density (kilograms per meter squared)
            shape.Friction = 0.0f;                  // Set friction
            shape.Restitution = 0.0f;               // Set restitution (slight bounce)

            stackNode.CreateComponent<CardStack>();
            return stackNode;
        }
        private Node CreateEmptyStack(string _tag, string _number, Vector2 _pos)
        {
            //###############################################################
            // create an empty playstack node
            //
            Node stackNode = MyScene.CreateChild("tempCardStack");
            stackNode.Name = _number;
            stackNode.AddTag(_tag);
            stackNode.SetPosition2D(_pos);

            Sprite2D sprite = ResourceCache.GetSprite2D("Cards/EmptyHolder.png");
            if (sprite == null)
                throw new InvalidOperationException("sprite not found");
            StaticSprite2D staticSprite = stackNode.CreateComponent<StaticSprite2D>();
            // Set random color
            staticSprite.Color = (new Color(Global.NextRandom(1.0f), Global.NextRandom(1.0f), Global.NextRandom(1.0f), 1.0f));
            staticSprite.BlendMode = BlendMode.Alpha;
            staticSprite.Sprite = sprite;

            //
            // physics (MUST be static so the cards don't bounce off of eachother)
            //
            RigidBody2D boxBody = stackNode.CreateComponent<RigidBody2D>();

            boxBody.BodyType = BodyType2D.Static;
            boxBody.Mass = 0;
            boxBody.LinearDamping = 0.0f;
            boxBody.AngularDamping = 0.0f;

            CollisionBox2D shape = stackNode.CreateComponent<CollisionBox2D>(); // Create box shape
            //shape.Center = new Vector2(0, -2.5f);
            shape.Size = new Vector2(0.7f, 1.0f);    // Set size
            shape.Density = 0.0f;                   // Set shape density (kilograms per meter squared)
            shape.Friction = 0.0f;                  // Set friction
            shape.Restitution = 0.0f;               // Set restitution (slight bounce)

            stackNode.CreateComponent<CardStack>();
            return stackNode;
        }

    }
}