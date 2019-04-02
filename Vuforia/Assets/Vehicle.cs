using System;

[Serializable]
public class Vehicle
{
    public Vehicle(string name, int size, int speed, int cost, int power)
    {
        VehicleName = name;
        VehicleSize = size;
        VehicleSpeed = speed;
        VehicleCost = cost;
        VehiclePower = power;

    }
    public string VehicleName { get; set; }
    public int VehicleSize { get; set; }
    public int VehicleSpeed { get; set; }
    public int VehicleCost { get; set; }
    public int VehiclePower { get; set; }
}