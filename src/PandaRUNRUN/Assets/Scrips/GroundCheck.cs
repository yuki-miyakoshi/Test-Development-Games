using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private string groundTag = "Ground";
    private bool isGround = false; 
    private bool isGroundEnter, isGroundStay, isGroundExit;


    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision.tag == groundTag)
    //     {
    //         Debug.Log("何かが判定に入りました");
    //     }
    // }
    
    // private void OnTriggerStay2D(Collider2D collision)
    // {
    //      if (collision.tag == groundTag)
    //      {
    //          Debug.Log("何かが判定に入り続けています");
    //      }
    // }

    // private void OnTriggerExit2D(Collider2D collision)
    // {
    //      if (collision.tag == groundTag)
    //      {
    //          Debug.Log("何かが判定をでました");
    //      }
    // }
    public bool IsGround()
    {
       if(isGroundEnter || isGroundStay)
       {
          isGround = true;
       }
       else if(isGroundExit)
       {
          isGround = false;
       } 

       isGroundEnter = false;
       isGroundStay = false;
       isGroundExit = false;
       return isGround; 
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.tag == groundTag)
       {
          isGroundEnter = true;
       }
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
       if (collision.tag == groundTag)
       {
          isGroundStay = true;
       }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
       if (collision.tag == groundTag)
       {
          isGroundExit = true;
       }
    }
}


