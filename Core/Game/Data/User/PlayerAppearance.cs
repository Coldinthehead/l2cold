namespace Core.Game.Data.User
{

    public struct PlayerAppearance
    {
        public string Name;
        public int Race;
        public int Face;
        public int HairStyle;
        public int HairColor;
        public bool Female;


        public PlayerAppearance(string name, int race,int face, int hairStyle, int hairColor, bool female)
        {
            Name = name;
            Race = race;
            Face = face;
            HairStyle = hairStyle;
            HairColor = hairColor;
            Female = female;
        }
    }
}
