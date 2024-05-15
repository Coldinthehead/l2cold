﻿namespace Core.Game.Data;


public struct GameCharacter
{
    public string Title;
    public CharacterSlotInfo Info;
    public double x, y, z;
    public CharacterStats Stats;

    public static GameCharacter BuildMockCharacter()
    {
        var character = new GameCharacter();
        character.Title = "1234567";
        character.Info = CharacterSlotInfo.BuildMockCharacterSlot();
        character.Stats = CharacterStats.BuildMockCharacterStats();
        return character;
    }

}
