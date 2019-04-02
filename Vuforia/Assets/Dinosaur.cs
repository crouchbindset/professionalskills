using System;

[Serializable]
public class Dinosaur
{
    public Dinosaur(string name, int size, int strength, int age, int speed)
    {
        DinosaurName = name;
        DinosaurSize = size;
        DinosaurSpeed = speed;
        DinosaurAge = age;
        DinosaurStrength = strength;

    }
    public string DinosaurName { get; set; }
    public int DinosaurSize { get; set; }
    public int DinosaurStrength { get; set; }
    public int DinosaurAge { get; set; }
    public int DinosaurSpeed { get; set; }
}