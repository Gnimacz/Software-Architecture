using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOITower : MonoBehaviour
{
    List<IDamagable> m_List = new List<IDamagable>();
    bool canAttack = true;
    float damage = 3f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(attack());
    }
    IEnumerator attack()
    {
        while (canAttack)
        {
            if (m_List.Count > 0)
            {
                for (int i = 0; i < m_List.Count; i++)
                {
                    IDamagable enemy = m_List[i];
                    enemy.TakeDamage(damage);
                    if (enemy.health <= 0)
                    {
                        RemoveEnemyFromList(enemy);
                    }
                }
            }
            yield return new WaitForSeconds(2);
        }
    }


    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Enemy") return;

        IDamagable damagable = other.GetComponent<IDamagable>();
        if (damagable != null && !m_List.Contains(damagable))
        {
            m_List.Add(damagable);
        }


    }

    private void OnTriggerExit(Collider other)
    {
        IDamagable damagable = other.GetComponent<IDamagable>();
        RemoveEnemyFromList(damagable);
    }

    void RemoveEnemyFromList(IDamagable enemy)
    {
        m_List.Remove(enemy);
    }
}
