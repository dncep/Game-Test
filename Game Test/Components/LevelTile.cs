namespace Game_Test.Components
{
    class LevelTile : Component
    {
        public string Texture { get; private set; }

        public LevelTile() => ComponentName = "level_tile";
    }
}
