using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private float timer;
    private int timeToBeStill = 2;
    private bool isInTrigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isInTrigger = true;
        StartCoroutine(AddPlupp(collision));
    }

    private IEnumerator AddPlupp(Collider2D collision)
    {
        yield return new WaitForSeconds(2);

        if (collision.gameObject.GetComponent<Rigidbody2D>() != null && isInTrigger)
        {
            BoardGrid gridScript = transform.parent.parent.GetComponent<BoardGrid>();
            gridScript.SetCellTaken(gameObject, collision.gameObject);
            gameObject.SetActive(false);
            collision.GetComponent<Plupp>().DeactivateRb();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isInTrigger = false;
    }
}
