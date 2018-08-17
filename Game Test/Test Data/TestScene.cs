using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Test.Components;
using Game_Test.ComponentSystems;
using Game_Test.Entities;
using Game_Test.Scenes;
using Game_Test.Visuals;

namespace Game_Test.Test_Data
{
    class TestScene : Scene
    {
        //TestEntity a;

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
            CurrentViewport.Scale = 8f;
            //CurrentViewport.Y = 500;
            //CurrentViewport.X = 125;

            Entities.Add(new TestEntity(0, 0));


            SpriteSheet sprites0 = new SpriteSheet(Game_Test.Properties.Resources.sprites0);
            sprites0.AddRegion("grass", 0, 0, 16, 16);
            sprites0.AddRegion("brick", 16, 0, 16, 16);
            Sprites.AddSpriteSheet(sprites0);

            SpriteSheet note = new SpriteSheet(Properties.Resources.notes);
            note.AddRegion("note", 0, 0, 32, 32);
            Sprites.AddSpriteSheet(note);

            Entities.Add(new TileEntity("brick", 0, 0));
            Entities.Add(new TileEntity("brick", 0, 1));
            Entities.Add(new TileEntity("brick", 1, 0));
            Entities.Add(new TileEntity("brick", 1, 1));


            Systems.Add(new LevelRenderer());
            Systems.Add(new Renderer());
            Systems.Add(new FramerateCounter());
            //Systems.Add(new TestSystem());
        }

        protected override void Tick()
        {

            //Console.WriteLine("Ticking");

            //CurrentViewport.X += (float)DeltaTime * 5f;
            //CurrentViewport.Y += (float)DeltaTime * 5f;
            CurrentViewport.Scale += (float)DeltaTime * 0.1f;

            //(a.Components["physical"] as Physical).X += 10 * DeltaTime;
        }
    }
}
