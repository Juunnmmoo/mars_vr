using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SojaExiles
{

    public class TVOnOff_Sport : MonoBehaviour
    {

        public Animator Sport;
        public Transform Player;
        public bool Off;

        void Start()
        {
            Off = false;
        }

        void OnMouseOver()
        {
            {
                if (Player)
                {
                    float dist = Vector3.Distance(Player.position, transform.position);
                    if (dist < 5)
                    {
                        if (Off == false)
                        {
                            if (Input.GetMouseButtonDown(0))
                            {
                                Sport.Play("Off");
                                Off = true;
                                print("you turned off the TV");
                            }

                        }
                        else
                            if (Off == true)
                        {
                            if (Input.GetMouseButtonDown(0))
                            {
                                Sport.Play("Sport");
                                Off = false;
                            }
                        }
                    }
                }

            }

        }



    }

}