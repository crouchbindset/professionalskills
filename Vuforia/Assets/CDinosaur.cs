using System;

[Serializable]
public class CDinosaur
{
    public CDinosaur(string name, int speed, int strength, int agility)
    {
        DinosaurName = name;
        DinosaurSpeed = speed;
        DinosaurStrength = strength;
        DinosaurAgility = agility;

    }
    public string DinosaurName { get; set; }
    public int DinosaurSpeed { get; set; }
    public int DinosaurStrength { get; set; }
    public int DinosaurAgility { get; set; }
}