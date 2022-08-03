using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SojaExiles
{

    public class tapS : MonoBehaviour
    {

        public Animator TapPour;
        public Transform Player;
        public bool pouring;

        void Start()
        {
            pouring = false;
        }

        void OnMouseOver()
        {
            {
                if (Player)
                {
                    float dist = Vector3.Distance(Player.position, transform.position);
                    if (dist < 15)
                    {
                        if (pouring == false)
                        {
                            if (Input.GetMouseButtonDown(0))
                            {
                                TapPour.Play("Pour");
                                pouring = true;
                                print("you are pouring");
                            }

                        }
                    }
                }

            }

        }

        void OnMouseExit()
        {
            {
                if (pouring == true)
                {
                    TapPour.Play("Pour1");
                    pouring = false;
                }
            }
        }

    }
}