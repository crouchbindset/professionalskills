using System.Collections.Generic;

public class CDecks
{
    public CDecks()
    {
        //Dinosaur card deck (list)
        Dinosaurs = new List<CDinosaur>(new CDinosaur[]
        {
            //Create Dinosaur objects with name (string), speed (int), strength (int) and agility (int)
            new CDinosaur("Ankylosaurus",2,4,2),
            new CDinosaur("Dilophosaurus",3,2,5),
            new CDinosaur("Mammoth",3,4,1),
            new CDinosaur("Megalodon",3,4,4),
            new CDinosaur("Pterodactyl",5,3,4),
            new CDinosaur("Sabre Tooth",4,2,4),
            new CDinosaur("Sarcosuchus",2,3,2),
            new CDinosaur("Spinosaurus",3,5,1),
            new CDinosaur("Stegosaurus",3,4,3),
            new CDinosaur("T-Rex",3,5,1),
            new CDinosaur("Triceratops",3,3,2),
            new CDinosaur("Velociraptor",5,1,5)
        });

        //Vehicle card deck (list)
        Vehicles = new List<CVehicle>(new CVehicle[]
        {
            //Create Vehicle objects with name (string), speed (int), power (int) and agility (int)
            new CVehicle("Aeroplane",5,5,1),
            new CVehicle("Ambulance",1,1,1),
            new CVehicle("Bicycle",1,1,1),
            new CVehicle("Bus",1,1,1),
            new CVehicle("Car",1,1,1),
            new CVehicle("Fire Engine",1,1,1),
            new CVehicle("Helicopter",1,1,1),
            new CVehicle("Motorcycle",1,1,1),
            new CVehicle("Police Car",1,1,1),
            new CVehicle("Scooter",1,1,1),
            new CVehicle("Tractor",1,1,1),
            new CVehicle("Truck",1,1,1)
        });
    }

    //Dinosaur deck (list) getter
    public List<CDinosaur> Dinosaurs { get; }

    //Vehicle deck (list) getter
    public List<CVehicle> Vehicles { get; }
}

