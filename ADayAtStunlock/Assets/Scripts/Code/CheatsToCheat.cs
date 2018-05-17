using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DAS
{
    namespace DBUG
    {
        public class CheatsToCheat : MonoBehaviour
        {
            private static bool cheatsEnabled = false;
            private float timer;

            private bool j, o, e, l;

            private void Start()
            {
                cheatsEnabled = false;
            }

            public static bool CheatsEnabled
            {
                get { return cheatsEnabled; }
            }

            // Update is called once per frame
            private void Update()
            {
                if (timer > 0)
                    timer -= Time.deltaTime;

                if (Input.anyKey)
                    CheckInput();
            }

            private void CheckInput()
            {
                if (timer <= 0)
                {
                    j = false;
                    o = false;
                    e = false;
                    l = false;
                }

                if (Input.GetKey(KeyCode.J))
                {
                    timer = 1;
                    j = true;
                }
                else if (Input.GetKey(KeyCode.O) && j)
                {
                    o = true;
                }
                else if (Input.GetKey(KeyCode.E) && j && o)
                {
                    e = true;
                }
                else if(Input.GetKey(KeyCode.L) && j && o && e)
                {
                    l = true;
                }
                else
                {
                    j = false;
                    o = false;
                    e = false;
                    l = false;
                }
                if(l && e && o && j)
                    cheatsEnabled = true;
            }

            private void OnGUI()
            {
#if UNITY_EDITOR
                GUI.Box(new Rect(Screen.width - 120, Screen.height - 25, 120, 25), "Cheats: " + CheatsEnabled);
#else
                if (CheatsEnabled)
                    GUI.Box(new Rect(Screen.width -120, Screen.height - 25, 120, 25), "Cheats: " + CheatsEnabled);
#endif

                if (!CheatsEnabled)
                    return;

                if(GUI.Button(new Rect(Screen.width - 120, Screen.height - 50, 120, 25), "More Money"))
                {
                    MoneyManager.currentMoney += 9999f;
                }
                if (GUI.Button(new Rect(Screen.width - 120, Screen.height - 75, 120, 25), "Less Money"))
                {
                    MoneyManager.currentMoney -= 9999f;
                }
                if (GUI.Button(new Rect(Screen.width - 120, Screen.height - 100, 120, 25), "Euphoria"))
                {
                    foreach (var npc in NPC.s_npcList)
                    {
                        npc.myFeelings.Happiness = 100;
                        npc.myFeelings.Motivation = 100;
                    }
                }
                if (GUI.Button(new Rect(Screen.width - 120, Screen.height - 125, 120, 25), "Dystopia"))
                {
                    foreach (var npc in NPC.s_npcList)
                    {
                        npc.myFeelings.Happiness = 0;
                        npc.myFeelings.Motivation = 0;
                    }
                }
            }
        }
    }
}