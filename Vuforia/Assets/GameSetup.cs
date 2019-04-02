using UnityEngine;
using System.Collections.Generic;
using Vuforia;
using System.Linq;

public class GameSetup
{
    private List<Dinosaur> DinosaurList;

    public GameSetup()
    {
        DinosaurList = new List<Dinosaur>(new Dinosaur[]
    {
        //Create Dinosaur objects with name, size, strength, age, speed attributes
        new Dinosaur("Ankylosaurus",4,5,3,1),
        new Dinosaur("Dilophosaurus",2,3,5,4),
        new Dinosaur("Mammoth",4,4,1,2),
        new Dinosaur("Megalodon",5,4,2,3),
        new Dinosaur("Pterodactyl",3,2,5,5),
        new Dinosaur("Sabre Tooth",1,2,1,5),
        new Dinosaur("Sarcosuchus",3,3,4,1),
        new Dinosaur("Spinosaurus",5,5,4,2),
        new Dinosaur("Stegosaurus",3,4,4,1),
        new Dinosaur("T-Rex",4,5,3,2),
        new Dinosaur("Triceratops",3,4,3,3),
        new Dinosaur("Velociraptor",1,1,3,5),
    });
    }

    public List<Dinosaur> DinosaurDeck
    {
        set { DinosaurList = value; }
        get { return DinosaurList; }
    }
}

