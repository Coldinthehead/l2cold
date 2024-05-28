using Core.Game.Data.Static;
using Core.Game.Data.User;

namespace Core.Game.Data;


public struct GameCharacterModel
{
    public string Title;
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
    public double x, y, z;
    public CharacterStats Stats;
    public CharacterGear GearObjectId;
    public CharacterGear GearItemId;

    public static GameCharacterModel BuildMockCharacter()
    {
        var model = new GameCharacterModel();
        model.Title = "1234567";
        model.Name = "Hello world";
        model.ObjectId = 0;
        model.ClanId = 0;
        model.Female = true;
        model.Race = 1;
        model.BaseClass = 1;
        model.CurrentHealth = 500;
        model.CurrentMana = 250;
        model.Sp = 777;
        model.Exp = 1;
        model.Karma = 1;
        model.Level = 1;
        model.MaxHealth = 9955;
        model.MaxMana = 1313;
        model.CurrentClass = 1;
        model.Stats = CharacterStats.BuildMockCharacterStats();
        model.GearObjectId = new CharacterGear();
        model.GearItemId = new CharacterGear();
        model.x = 8000;
        model.y = 8000;
        model.z = -2800;
        return model;
    }

    public static GameCharacterModel Build(string characterName, PlayerAppearance appearance)
    {
        var model = new GameCharacterModel();

        return model;
    }
}

