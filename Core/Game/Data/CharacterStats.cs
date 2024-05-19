namespace Core.Game.Data;

public struct CharacterStats
{
    public int STR;
    public int DEX;
    public int CON;
    public int INT;
    public int WIT;
    public int MEN;

    public int Patk;
    public int PatkSpd;
    public int Pdef;
    public int Evasion;
    public int Accuracy;
    public int Crit;
    public int Matk;
    public int MatkSpd;
    public int Mdef;

    public int RunSpd;
    public int WalkSpd;

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

        stats.RunSpd = 200;
        stats.WalkSpd = 100;

        return stats;
    }
}



