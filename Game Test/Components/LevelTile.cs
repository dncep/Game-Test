namespace Game_Test.Components
{
    class LevelTile : IComponent
    {
        public string Texture { get; private set; }

        public LevelTile(string texture)
        {
            ComponentName = "level_tile";
            this.Texture = texture;
        }
    }
}
