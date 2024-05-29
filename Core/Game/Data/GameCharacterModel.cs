using System.Text.Json.Serialization;

namespace Core.Game.Data;

[Serializable]
public class GameCharacterModel
{
    public string Title { get; set; }
    public string Name { get; set; }
    public int ObjectId { get; set; }
    public int ClanId { get; set; }
    public bool Female { get; set; }
    public int Race { get; set; }
    public int BaseClass { get; set; }
    public double CurrentHealth { get; set; }
    public double CurrentMana { get; set; }
    public int Sp { get; set; }
    public long Exp { get; set; }
    public int Level { get; set; }
    public int Karma { get; set; }
    public int HairStyle { get; set; }
    public int HairColor { get; set; }
    public int Face { get; set; }
    public double MaxHealth { get; set; }
    public double MaxMana { get; set; }
    public int CurrentClass { get; set; }
    public double x { get; set; }
    public double y { get; set; }
    public double z { get; set; }
    public float CollisionHeight { get; set; } 
    public float CollisionRadius { get; set; }
    public CharacterStats Stats { get; set; }
    public CharacterGear GearObjectId { get; set; }
    public CharacterGear GearItemId { get; set; }

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
}

