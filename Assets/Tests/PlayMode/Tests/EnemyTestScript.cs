using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class EnemyTestScript
{
    [OneTimeSetUp]
    public void LoadScene()
    {
        SceneManager.LoadScene("UnitTestScene");
    }

    
    [UnityTest]
    public IEnumerator EnemyDamageEventTest()
    {
        WaveManager.Instance.timeBetweenWaves = 3;
        yield return new WaitForSeconds(5);

        var enemy = WaveManager.Instance.Enemies[0];

        float damage = 3;
        enemy.TakeDamage(damage, IDamagable.DamageType.Physical);

        Assert.AreEqual(enemy.MaxHealth - damage, enemy.Health);
    }

    [UnityTest]
    public IEnumerator EnemyInstantDeathTest()
    {
        yield return new WaitForSeconds(2);

        var enemy = WaveManager.Instance.Enemies[0];

        float damage = 200;
        enemy.TakeDamage(damage, IDamagable.DamageType.Physical);
        Assert.AreEqual(enemy.Health, 0);
    }
}
