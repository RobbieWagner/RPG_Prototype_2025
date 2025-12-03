using System.Collections;
using RobbieWagnerGames.RPG;
using UnityEngine;

public class NewGameController : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(StartNewGame());
    }

    private IEnumerator StartNewGame()
    {
        yield return new WaitForSeconds(2f);
        RunManager.Instance.PrepRunScene();
    }
}
