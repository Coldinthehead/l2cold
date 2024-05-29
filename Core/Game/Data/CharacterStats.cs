using Core.Game.Data.Static;
using Core.Game.Repository;

namespace Core.Game.Data;

[Serializable]
public struct CharacterStats
{
    public int STR { get; set; }
    public int DEX { get; set; }
    public int CON { get; set; }
    public int INT { get; set; }
    public int WIT { get; set; }
    public int MEN { get; set; }

    public int Patk { get; set; }
    public int PatkSpd { get; set; }
    public int Pdef { get; set; }
    public int Evasion { get; set; }
    public int Accuracy { get; set; }
    public int Crit { get; set; }
    public int Matk { get; set; } 
    public int MatkSpd { get; set; }
    public int Mdef { get; set; }

    public int RunSpd { get; set; }
    public int WalkSpd { get; set; }

    public static CharacterStats BuildMockCharacterStats()
    {
        var stats = new CharacterStats();
        stats.STR = 10;
        stats.DEX = 10;
        stats.CON = 10;
        stats.INT = 10;
        stats.WIT = 10;
        stats.MEN = 10;

        stats.Patk = 10;
        stats.PatkSpd = 10;
        stats.Pdef = 10;
        stats.Evasion = 10;
        stats.Accuracy = 10;
        stats.Crit = 10;
        stats.Matk = 10;
        stats.MatkSpd = 10;
        stats.Mdef = 10;

        stats.RunSpd = 250;
        stats.WalkSpd = 100;

        return stats;
    }

 
    public void SetSpeed(int value)
    {
        RunSpd = value;
    }
}



