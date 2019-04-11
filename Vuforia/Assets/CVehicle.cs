using System;

[Serializable]
public class CVehicle
{
    public CVehicle(string name, int speed, int power, int agility)
    {
        VehicleName = name;
        VehicleSpeed = speed;
        VehiclePower = power;
        VehicleAgility = agility;
    }
    public string VehicleName { get; set; }
    public int VehicleSpeed { get; set; }
    public int VehiclePower { get; set; }
    public int VehicleAgility { get; set; }
}