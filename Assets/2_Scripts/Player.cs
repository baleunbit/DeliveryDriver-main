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
        Debug.Log("���ݷ�: " + damage);
    }
    public void defend()
    {
        int damage = 5;
        Debug.Log("����: " +  damage);

    }
}