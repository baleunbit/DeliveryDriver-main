using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Player
{
    public int health = 100;
    public static int PlayerCount = 0;

    public Player()
    {
        PlayerCount++;
    }

    public void TakeDamage(int damage)
    {
        health = health - damage;
    }

    public void Attack()
    {
        int damage = 10;
        Debug.Log("공격력: " + damage);
    }
    public void defend()
    {
        int damage = 5;
        Debug.Log("방어력: " +  damage);

    }
}