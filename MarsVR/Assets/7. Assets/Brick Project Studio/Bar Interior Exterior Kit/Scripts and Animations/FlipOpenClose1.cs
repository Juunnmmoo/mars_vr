using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SojaExiles
{

	public class FlipOpenClose1 : MonoBehaviour
	{

		public Animator Flip1;
		public bool open;
		public Transform Player;

		void Start()
		{
			open = false;
		}

		void OnMouseOver()
		{
			{
				if (Player)
				{
					float dist = Vector3.Distance(Player.position, transform.position);
					if (dist < 15)
					{
						if (open == false)
						{
							if (Input.GetMouseButtonDown(0))
							{
								StartCoroutine(opening());
							}
						}
						else
						{
							if (open == true)
							{
								if (Input.GetMouseButtonDown(0))
								{
									StartCoroutine(closing());
								}
							}

						}

					}
				}

			}

		}

		IEnumerator opening()
		{
			print("you are opening the door");
			Flip1.Play("FlipOpen1");
			open = true;
			yield return new WaitForSeconds(.5f);
		}

		IEnumerator closing()
		{
			print("you are closing the door");
			Flip1.Play("FlipClose1");
			open = false;
			yield return new WaitForSeconds(.5f);
		}


	}

}