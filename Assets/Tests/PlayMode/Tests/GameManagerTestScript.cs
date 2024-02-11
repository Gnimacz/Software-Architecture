using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class GameManagerTestScript
{

    [OneTimeSetUp]
    public void LoadScene()
    {
        SceneManager.LoadScene("UnitTestScene");
    }

    [UnityTest]
    public IEnumerator GameOverPlayerWon()
    {
        yield return new WaitForSeconds(3f);

        var gameManager = GameManager.Instance;

        EventBus<GameOverEvent>.Raise(new GameOverEvent(true));

        Assert.IsTrue(gameManager.isGameOver && (gameManager.PlayerWonGame == true));

    }

    [UnityTest]
    public IEnumerator GameOverPlayerLost()
    {
        yield return new WaitForSeconds(3f);

        var gameManager = GameManager.Instance;

        EventBus<GameOverEvent>.Raise(new GameOverEvent(false));

        Assert.IsTrue(gameManager.isGameOver && (gameManager.PlayerWonGame == false));
    }

    [UnityTest]
    public IEnumerator IsGameManagerSingletonTest()
    {
        yield return new WaitForSeconds(1.2f);

        var gameManager = GameManager.Instance.gameObject;

        GameObject newGameManger = new GameObject();
        newGameManger.AddComponent<GameManager>();

        yield return new WaitForEndOfFrame();

        Assert.AreEqual(gameManager, GameManager.Instance.gameObject);

    }

}
