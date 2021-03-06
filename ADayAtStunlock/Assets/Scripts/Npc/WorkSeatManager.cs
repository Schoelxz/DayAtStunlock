﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DAS
{
    [System.Serializable]
    public class WorkSeat
    {
        public GameObject workSeatGameObject;
        public DAS.NPC NpcOwner;

        public WorkSeat(GameObject workseat, DAS.NPC npc)
        {
            this.workSeatGameObject = workseat;
            this.NpcOwner = npc;
            WorkSeatManager.myInstance.workSeats.Add(this);

            workSeatGameObject.name = "Work seat for " + NpcOwner.name;
        }
    }

    public class WorkSeatManager : MonoBehaviour
    {
        public static WorkSeatManager myInstance;

        [HideInInspector]
        public Dictionary<string, WorkseatOwner> gameobjectSeats = new Dictionary<string, WorkseatOwner>();
        public List<WorkSeat> workSeats = new List<WorkSeat>();

        private void Awake()
        {
            if (myInstance == null)
                myInstance = this;
            else
            {
                Debug.LogError("Singleton Error: WorkSeatManager, newest component destroyed!");
                Destroy(this);
            }

            foreach (var workseat in GameObject.FindObjectsOfType<WorkseatOwner>())
            {
                gameobjectSeats.Add(workseat.ownerName, workseat);
            }
            
        }
    }

}