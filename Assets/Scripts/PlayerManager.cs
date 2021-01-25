using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public RoadGenerator rg;
    public Text coinsCounter;
    int coinsCount = 0;
    public SwipeManager sm;

    private void OnTriggerEnter(Collider other)
    {
      if (other.gameObject.tag == "Coin")
        {
            Destroy(other.gameObject);
            coinsCount += 1;
            string Counter = coinsCount.ToString();
            coinsCounter.text = Counter;
        }
        if (other.gameObject.tag == "obstacle")
        {
            rg.PauseLevel();
            Debug.Log("You Die");
            coinsCounter.text = "You Die";
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            sm.isGrounded = true;
        }
    }
}