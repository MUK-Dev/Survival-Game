using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectChecker : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private List<ObjectFader> objFaders;

    private void Start()
    {
        objFaders = new List<ObjectFader>();
    }

    private void Update()
    {
        Vector3 rayTransform = new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z);
        Ray ray = new Ray(rayTransform, player.transform.position - transform.position);
        Debug.DrawRay(rayTransform, player.transform.position - transform.position);
        //? Shoot a ray from camera to player to check objects within camera and player
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            //? If object didn't hit anything the return
            if (hit.collider == null) return;

            if (hit.collider.gameObject == player)
            {
                //? If ray hit player then nothing is between camera and player
                if (objFaders.Count != 0)
                {
                    //? If there were objects faded before then remove them
                    objFaders.ForEach(obj =>
                    {
                        Debug.Log("Disable fading");
                        obj.DisableFading();

                    });
                    //? Then clear the list
                    objFaders.Clear();
                }
            }
            else
            {
                //? If something is in between player and cam fade it and add it to the list
                ObjectFader newObj = hit.collider.gameObject.GetComponent<ObjectFader>();
                if (newObj != null)
                {
                    newObj.EnableFading();
                    objFaders.Add(newObj);
                }
            }
        }
    }
}
