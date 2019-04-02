using UnityEngine;
using System.Collections.Generic;
using Vuforia;
using System.Linq;

namespace blue
{

    public class BluePlayer : MonoBehaviour, ITrackableEventHandler
    {
        // Our target image object which represents the players physical card
        private TrackableBehaviour mTargetCard;

        private bool cardTracked = false;

        public List<Dinosaur> dinosaurList = new List<Dinosaur>(new Dinosaur[]
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

        public List<Vehicle> vehicleList = new List<Vehicle>(new Vehicle[]
    {
        //Create Vehicle objects with name, size, speed, cost, power attributes
        new Vehicle("Aeroplane",5,5,5,5),
        new Vehicle("Ambulance",3,3,3,3),
        new Vehicle("Bicycle",1,1,1,1),
        new Vehicle("Bus",4,2,3,4),
        new Vehicle("Car",2,3,2,3),
        new Vehicle("Fire Engine",4,3,4,4),
        new Vehicle("Helicopter",3,4,5,4),
        new Vehicle("Motorcycle",1,4,3,3),
        new Vehicle("Police Car",3,3,3,3),
        new Vehicle("Scooter",2,2,2,2),
        new Vehicle("Tractor",3,2,3,4),
        new Vehicle("Truck",3,3,2,3),
    });

        // Use this for initialization
        void Start()
        {
            mTargetCard = GetComponent<TrackableBehaviour>();

            if (mTargetCard)
            {
                mTargetCard.RegisterTrackableEventHandler(this);
            }
        }

        // Update is called once per frame
        void Update() { }

        public void OnTrackableStateChanged(
         TrackableBehaviour.Status previousStatus,
         TrackableBehaviour.Status newStatus)
        {
            if (newStatus == TrackableBehaviour.Status.DETECTED ||
             newStatus == TrackableBehaviour.Status.TRACKED)
            {
                cardTracked = true;
                OnTrackingFound();
            }
            else
            {
                cardTracked = false;
            }
        }
        private void OnTrackingFound()
        {
            //disable any pre-existing augmentation
            for (int i = 0; i < mTargetCard.transform.childCount; i++)
            {
                Transform child = mTargetCard.transform.GetChild(i);
                child.gameObject.SetActive(false);
            }

            dinosaurList = dinosaurList.OrderBy(x => Random.value).ToList();

            Transform myModelTrf = Instantiate(Resources.Load<Transform>("Dinosaurs/" + dinosaurList[0].DinosaurName)) as Transform;
            myModelTrf.parent = mTargetCard.transform;
            myModelTrf.localPosition = new Vector3(0f, 0f, 0f);
            myModelTrf.localRotation = Quaternion.identity;
            myModelTrf.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            if (
                    myModelTrf.gameObject != null)
            {
                myModelTrf.gameObject.SetActive(true);
            }
        }

        void DrawOutline(Rect r, string t, int strength, GUIStyle style)
        {
            GUI.color = Color.black;
            int i;
            for (i = -strength; i <= strength; i++)
            {
                GUI.Label(new Rect(r.x - strength, r.y + i, r.width, r.height), t, style);
                GUI.Label(new Rect(r.x + strength, r.y + i, r.width, r.height), t, style);
            }
            for (i = -strength + 1; i <= strength - 1; i++)
            {
                GUI.Label(new Rect(r.x + i, r.y - strength, r.width, r.height), t, style);
                GUI.Label(new Rect(r.x + i, r.y + strength, r.width, r.height), t, style);
            }
            GUI.color = Color.white;
            GUI.Label(new Rect(r.x, r.y, r.width, r.height), t, style);
        }

        void OnGUI()
        {
            GUIStyle myStyle = new GUIStyle();
            myStyle.fontSize = 20;
            myStyle.normal.textColor = Color.white;

            Texture starIcon = Instantiate(Resources.Load("Star")) as Texture;
            if (cardTracked == true)
            {
                DrawOutline(new Rect(10, 10, 100, 20), dinosaurList[0].DinosaurName, 2, myStyle);
                DrawOutline(new Rect(10, 50, 100, 20), "Size: " + dinosaurList[0].DinosaurSize, 2, myStyle);
                for (int i = 0; i < dinosaurList[0].DinosaurSize; i++)
                {
                    GUI.Label(new Rect((30 * (i + 1)), 70, 30, 30), starIcon);
                }
                DrawOutline(new Rect(10, 100, 100, 20), "Speed: " + dinosaurList[0].DinosaurSpeed, 2, myStyle);
                for (int i = 0; i < dinosaurList[0].DinosaurSpeed; i++)
                {
                    GUI.Label(new Rect((30 * (i + 1)), 120, 30, 30), starIcon);
                }
                DrawOutline(new Rect(10, 150, 100, 20), "Age: " + dinosaurList[0].DinosaurAge, 2, myStyle);
                for (int i = 0; i < dinosaurList[0].DinosaurAge; i++)
                {
                    GUI.Label(new Rect((30 * (i + 1)), 170, 30, 30), starIcon);
                }
                DrawOutline(new Rect(10, 200, 100, 20), "Strength: " + dinosaurList[0].DinosaurStrength, 2, myStyle);
                for (int i = 0; i < dinosaurList[0].DinosaurStrength; i++)
                {
                    GUI.Label(new Rect((30 * (i + 1)), 220, 30, 30), starIcon);
                }
            }
        }
    }
}