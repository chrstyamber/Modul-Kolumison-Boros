using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Purchase : MonoBehaviour, IPointerClickHandler
{
    public GameObject purchasePanel;
    public GameObject panelPosition;

    public void OnPointerClick(PointerEventData eventData)
    {

        GameObject instantiatedPurchase = Instantiate(purchasePanel, panelPosition.transform.position, panelPosition.transform.rotation) as GameObject;
        instantiatedPurchase.transform.SetParent(panelPosition.transform);
    }
}