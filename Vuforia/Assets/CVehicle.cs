using System;

[Serializable]
public class CVehicle
{
    //Constructor
    public CVehicle(string name, int speed, int power, int agility)
    {
        //Vehicle object name
        VehicleName = name;

        //Vehicle object speed attribute
        VehicleSpeed = speed;

        //Vehicle object power attribute
        VehiclePower = power;

        //Vehicle object agility attribute
        VehicleAgility = agility;
    }

    //Name getter and setter
    public string VehicleName { get; set; }

    //Speed getter and setter
    public int VehicleSpeed { get; set; }

    //Power getter and setter
    public int VehiclePower { get; set; }

    //Agility gett and setter
    public int VehicleAgility { get; set; }
}