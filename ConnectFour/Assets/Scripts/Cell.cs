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
            BoardGrid gridScript = transform.parent.parent.GetComponent<BoardGrid>();
            gridScript.SetCellTaken(gameObject, collision.gameObject);
            gameObject.SetActive(false);
            collision.GetComponent<Plupp>().DeactivateRb();
            collision.transform.position = gameObject.GetComponent<BoxCollider2D>().bounds.center;

            GameManager.Instance.ChangeTurn();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isInTrigger = false;
        StopCoroutine(AddPlupp(collision));
    }
}
