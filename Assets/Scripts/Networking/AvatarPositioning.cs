using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarPositioning : MonoBehaviourPun
{

    void Update()
    {
        if (transform.parent.position != transform.position)
        {
            transform.parent.position = transform.position;
            transform.localPosition = Vector3.zero;
        }
        
    }
}
