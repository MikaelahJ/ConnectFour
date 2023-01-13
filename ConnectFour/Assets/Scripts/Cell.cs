using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private float timer;
    private int timeToBeStill = 2;

    private void OnTriggerStay2D(Collider2D collision)
    {
        timer += Time.deltaTime;
        if (collision.gameObject.GetComponent<Rigidbody2D>() != null && timer >= timeToBeStill)
        {
            BoardGrid gridScript = transform.parent.parent.GetComponent<BoardGrid>();
            gridScript.SetCellTaken(gameObject, collision.gameObject);
            gameObject.SetActive(false);
            collision.GetComponent<Plupp>().DeactivateRb();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        timer = 0;
    }
}
