using System.Windows.Forms;
using Game_Test.Test_Data;
using Game_Test.Transition;

namespace Game_Test
{
    class GameStart
    {
        static void Main(string[] args)
        {
            new Game(new TestScene()); 
        }
    }
}
