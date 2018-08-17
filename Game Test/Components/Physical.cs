namespace Game_Test.Components
{
    class Physical : IComponent
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Physical() => ComponentName = "physical";

        public Physical(float X, float Y) : this()
        {
            this.X = X;
            this.Y = Y;
        }
    }
}
