using System.Collections.Generic;
using System;

[Serializable]
public class CPlayer
{
    //Create a deck object, contains initial deck (list) setups
    readonly CDecks PlayerDecks = new CDecks();

    //Constructor, with player name parameter
    public CPlayer(string name)
    {
        PlayerName = name;
        PlayerScore = 0;

        //Create decks (lists) of card type objects
        PlayerDinosaurs = PlayerDecks.Dinosaurs;
        PlayerVehicles = PlayerDecks.Vehicles;

        //Shuffle decks (lists) into random order
        Shuffle<CDinosaur>(PlayerDinosaurs);
        Shuffle<CVehicle>(PlayerVehicles);
    }

    //Shuffle a list into a random order
    public static void Shuffle<T>(IList<T> list)
    {
        var count = list.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var range = UnityEngine.Random.Range(i, count);
            var temp = list[i];
            list[i] = list[range];
            list[range] = temp;
        }
    }

    //Players current score
    public int PlayerScore { get; set; }

    //Player name getter
    public string PlayerName { get; }

    //Dinosaur deck (list) getter
    public List<CDinosaur> PlayerDinosaurs { get; }

    //Vehicle deck (list) getter
    public List<CVehicle> PlayerVehicles { get; }
}