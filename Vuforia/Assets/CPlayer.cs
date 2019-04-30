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
        DeckIndex = 0;
        TurnStatus = 0;
        HasChosen = false;
        CurrentTurn = false;

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

    //Is it the players turn?
    public bool CurrentTurn { get; set;  }

    //Has the player chosen an attribute to play?
    public bool HasChosen { get; set;  }

    //Players turn status, 0 = none, 1 = won, 2 = lost, 3 = drew
    public int TurnStatus { get; set; }

    //Player deck index getter and setter
    public int DeckIndex { get; set; }

    //Players current score getter and setter
    public int PlayerScore { get; set; }

    //Player name getter
    public string PlayerName { get; }

    //Dinosaur deck (list) getter
    public List<CDinosaur> PlayerDinosaurs { get; }

    //Vehicle deck (list) getter
    public List<CVehicle> PlayerVehicles { get; }
}