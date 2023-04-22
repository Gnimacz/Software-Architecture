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
        SceneManager.LoadScene("Main");
    }

    
    [UnityTest]
    public IEnumerator EnemyDamageEventTest()
    {
        yield return new WaitForSeconds(4f);

        var enemy = EnemyManager.Instance.Enemies[0];

        float damage = 3;
        enemy.TakeDamage(damage, IDamagable.DamageType.Physical);

        Assert.AreEqual(enemy.MaxHealth - damage, enemy.Health);
    }

    [UnityTest]
    public IEnumerator EnemyInstantDeathTest()
    {
        yield return new WaitForSeconds(4f);

        var enemy = EnemyManager.Instance.Enemies[0];

        float damage = 200;
        enemy.TakeDamage(damage, IDamagable.DamageType.Physical);
        Assert.AreEqual(enemy.Health, 0);
    }
}
