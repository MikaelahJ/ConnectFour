using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private bool isInTrigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StopCoroutine(AddPlupp(collision));
        isInTrigger = true;
        StartCoroutine(AddPlupp(collision));
    }

    private IEnumerator AddPlupp(Collider2D collision)
    {
        yield return new WaitForSeconds(3);

        if (collision.gameObject.GetComponent<Rigidbody2D>() != null && isInTrigger)
        {
            float[] xPos = collision.GetComponent<Plupp>().xPositions.ToArray();
            float[] yPos = collision.GetComponent<Plupp>().yPositions.ToArray();

            BoardGrid gridScript = transform.parent.parent.GetComponent<BoardGrid>();
            gridScript.SetCellTaken(gameObject, collision.gameObject,xPos,yPos);

            gameObject.SetActive(false);
            collision.GetComponent<Plupp>().DeactivateRb();
            collision.transform.position = gameObject.GetComponent<BoxCollider2D>().bounds.center;
            collision.GetComponent<Plupp>().IsInCell = true;

            FirebaseManager.Instance.ChangeTurn();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isInTrigger = false;
        StopCoroutine(AddPlupp(collision));
    }
}
