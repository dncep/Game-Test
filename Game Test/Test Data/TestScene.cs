using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Test.Components;
using Game_Test.ComponentSystems;
using Game_Test.Entities;
using Game_Test.Scenes;
using Game_Test.Util;
using Game_Test.Visuals;

namespace Game_Test.Test_Data
{
    class TestScene : Scene
    {
        //TestEntity a;

        Box box;

        public TestScene() : base()
        {
            //Entities.Add(new TestEntity(0, 0));
            //Entities.Add(new TestEntity(10, 10));
            //Entities.Add(new TestEntity(137+10, 125));
            //CurrentViewport.X = 16;
            /*for(int i = 0; i < 100; i++)
            {
                Entities.Add(new TestEntity(0, i));
            }*/
            //CurrentViewport.Scale = 1f;
            CurrentViewport.Y = 64;

            Entities.Add(new TestEntity(0, 0));


            SpriteSheet sprites0 = new SpriteSheet(Game_Test.Properties.Resources.sprites0);
            sprites0.AddRegion("grass", 0, 0, 16, 16);
            sprites0.AddRegion("brick", 16, 0, 16, 16);
            sprites0.AddRegion("brick_slope_right", 16, 16, 16, 16);
            sprites0.AddRegion("brick_slope_left", 16, 32, 16, 16);
            sprites0.AddRegion("brick_slope_upside_down", 16, 48, 16, 16);
            Sprites.AddSpriteSheet(sprites0);

            SpriteSheet note = new SpriteSheet(Properties.Resources.notes);
            note.AddRegion("note", 0, 0, 32, 32);
            Sprites.AddSpriteSheet(note);

            for(int x = -1; x <= 2; x++)
            {
                Entities.Add(new TileEntity("brick", x, 0));
                Entities.Add(new TileEntity("brick", x, 8));
            }

            box = new Box(8f, 64f);
            
            //(box.Components["rigidbody"] as RigidBody).Velocity += new Vector2D(10, 0);
            Entities.Add(box);
            Entities.Add(new Slope(0, 1, true));
            Entities.Add(new Slope(1, 1, false));



            Systems.Add(new FramerateCounter());
            Systems.Add(new LevelRenderer());
            //Systems.Add(new ObjectRenderer());
            Systems.Add(new PhysicsSystem());
        }

        protected override void Tick()
        {
            var boxsp = (box.Components["spatial"] as Spatial);
            var boxrb = (box.Components["rigidbody"]) as RigidBody;
            if (boxsp.Y < -1000)
            {
                boxsp.Position = new Vector2D(8, 64);
                boxrb.Velocity = new Vector2D();
            }
            //CurrentViewport.Y = (float) (16d*Math.Sin(2*time));
            //Console.WriteLine("Ticking");

            //CurrentViewport.X += (float)DeltaTime * 32f;
            //CurrentViewport.Y += (float)DeltaTime * 5f;
            //CurrentViewport.Scale += (float)DeltaTime * change;
            //if (CurrentViewport.Scale >= 2) change = -0.1f;
            //else if (CurrentViewport.Scale <= 0.1) change = 0.1f;

            //(a.Components["physical"] as Physical).X += 10 * DeltaTime;
        }
    }
}
