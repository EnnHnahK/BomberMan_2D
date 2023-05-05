using System.Collections;
using UnityEngine;


public class GameController : MonoBehaviour
{
    public IEnumerator Level(System.Action action)
    {
        //statusPanel.SetActive(true);
        float introTimer = 1f;
        while (introTimer > 0)
        {
            //gameStartCountDown.text = "STARTS IN:" + ((int)introTimer).ToString();
            //levelText.text = "LEVEL:" + level;
            introTimer -= Time.deltaTime;
            yield return null;
        }
        //statusPanel.SetActive(false);
        action?.Invoke();
    }
}
