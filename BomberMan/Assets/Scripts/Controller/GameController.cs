using System.Collections;
using UnityEngine;


public class GameController : MonoBehaviour
{
    public IEnumerator Level(System.Action action)
    {
        //statusPanel.SetActive(true);
        float introTimer = 2f;
        while (introTimer > 0)
        {
            introTimer -= Time.deltaTime;
            yield return null;
        }
        //statusPanel.SetActive(false);
        action?.Invoke();
    }
}
