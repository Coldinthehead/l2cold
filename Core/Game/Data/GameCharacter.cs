﻿namespace Core.Game.Data;


public struct GameCharacter
{
    public string Title;
    public CharacterInfo Info;
    public double x, y, z;
    public CharacterStats Stats;
    public CharacterGear GearObjectId;
    public CharacterGear GeartItemId;

    public static GameCharacter BuildMockCharacter()
    {
        var character = new GameCharacter();
        character.Title = "1234567";
        character.Info = CharacterInfo.BuildMockCharacterSlot();
        character.Stats = CharacterStats.BuildMockCharacterStats();
        character.GearObjectId = new CharacterGear();
        character.GeartItemId = new CharacterGear();
        character.x = 8000;
        character.y = 8000;
        character.z = -2800;
        return character;
    }

}

