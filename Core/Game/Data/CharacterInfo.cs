namespace Core.Game.Data;


public struct CharacterInfo
{
    public string Name;
    public int ObjectId;
    public int ClanId;
    public bool Female;
    public int Race;
    public int BaseClass;
    public double CurrentHealth;
    public double CurrentMana;
    public int Sp;
    public long Exp;
    public int Level;
    public int Karma;
    public int HairStyle;
    public int HairColor;
    public int Face;
    public double MaxHealth;
    public double MaxMana;
    public int CurrentClass;


    public static CharacterInfo BuildMockCharacterSlot()
    {
        var info = new CharacterInfo();
        info.Name = "Hello world";
        info.ObjectId = 0;
        info.ClanId = 0;
        info.Female = true;
        info.Race = 1;
        info.BaseClass = 1;
        info.CurrentHealth = 500;
        info.CurrentMana = 250;
        info.Sp = 777;
        info.Exp = 1;
        info.Karma = 1;
        info.Level = 1;
        info.MaxHealth = 9955;
        info.MaxMana = 1313;
        info.CurrentClass = 1;

        return info;
    }
}

