using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Health up", menuName ="Powerup/HealthUps")]
public class Healthup : ScriptableObject
{
    [SerializeField] int amount;
    //sets heal amount and appplies it 
    public  void applyHeal(playerController player)
    {
        player.Heal(amount);
    }
}
