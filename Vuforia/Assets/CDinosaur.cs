using System;

[Serializable]
public class CDinosaur
{
    //Constructor
    public CDinosaur(string name, int speed, int strength, int agility)
    {
        //Dinosaur object name
        DinosaurName = name;

        //Dinosaur object speed attribute
        DinosaurSpeed = speed;

        //Dinosaur object strength attribute
        DinosaurStrength = strength;

        //Dinosaur object agility attribute
        DinosaurAgility = agility;

    }

    //Name getter and setter
    public string DinosaurName { get; set; }

    //Speed getter and setter
    public int DinosaurSpeed { get; set; }

    //Strength getter and setter
    public int DinosaurStrength { get; set; }

    //Agility getter and setter
    public int DinosaurAgility { get; set; }
}