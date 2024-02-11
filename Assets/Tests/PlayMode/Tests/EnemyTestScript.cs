using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

/// <summary>
/// This class contains tests for the Enemy class.
/// The tests are run in the Unity Test Runner.
/// It tests the damage, debuff and death of an enemy.
/// </summary>

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

    [UnityTest]
    public IEnumerator EnemyDebuffTest()
    {
        yield return new WaitForSeconds(2);

        var enemy = WaveManager.Instance.Enemies[1];

        var originalSpeed = enemy.GetComponent<NavMeshAgent>().speed;
        var debuff = new IDamagable.Debuff(IDamagable.DebuffType.Slow, 0.5f);
        enemy.AddDebuff(debuff);
        yield return new WaitForSeconds(1);

        Assert.AreEqual(originalSpeed * debuff.level, enemy.GetComponent<NavMeshAgent>().speed);
    }
}
