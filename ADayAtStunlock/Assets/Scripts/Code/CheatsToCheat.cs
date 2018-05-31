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

            private bool g,l,u,p,s,k,a;

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
                    g = false;
                    l = false;
                    u = false;
                    p = false;
                    s = false;
                    k = false;
                    a = false;
                }

                if (Input.GetKey(KeyCode.G))
                {
                    timer = 2;
                    g = true;
                }
                else if (Input.GetKey(KeyCode.L) && g)
                {
                    l = true;
                }
                else if (Input.GetKey(KeyCode.U) && g && l)
                {
                    u = true;
                }
                else if(Input.GetKey(KeyCode.P) && g && l && u)
                {
                    p = true;
                }
                else if (Input.GetKey(KeyCode.S) && g && l && u && p)
                {
                    s = true;
                }
                else if (Input.GetKey(KeyCode.K) && g && l && u && p && s)
                {
                    k = true;
                }
                else if (Input.GetKey(KeyCode.A) && g && l && u && p && s && k)
                {
                    a = true;
                }
                else
                {
                    g = false;
                    l = false;
                    u = false;
                    p = false;
                    s = false;
                    k = false;
                    a = false;
                }
                if(a && k && s && p && u && l && g)
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
                    MoneyManager.CurrentMoney += 9999f;
                }
                if (GUI.Button(new Rect(Screen.width - 120, Screen.height - 75, 120, 25), "Less Money"))
                {
                    MoneyManager.CurrentMoney -= 9999f;
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