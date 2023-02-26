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
            SetPlupp(collision.gameObject);
            FirebaseManager.Instance.ChangeTurn();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isInTrigger = false;
        StopCoroutine(AddPlupp(collision));
    }

    public void SetPlupp(GameObject plupp)
    {
        float[] xPos = plupp.GetComponent<Plupp>().xPositions.ToArray();
        float[] yPos = plupp.GetComponent<Plupp>().yPositions.ToArray();

        BoardGrid gridScript = transform.parent.parent.GetComponent<BoardGrid>();
        gridScript.SetCellTaken(gameObject, plupp.gameObject, xPos, yPos);

        gameObject.SetActive(false);
        plupp.GetComponent<Plupp>().DeactivateRb();
        plupp.transform.position = gameObject.GetComponent<BoxCollider2D>().bounds.center;
        plupp.GetComponent<Plupp>().IsInCell = true;
    }
}
