using UnityEngine;
using System.Collections.Generic;

public enum PowerUpType {SizeIncrease, Blast}

/// <summary>
/// Base class for power ups management
//  Lets mini games 
// - gain a power up
// - know if a power up for their type is available
// - remove power up if used
//
// Power ups available:
// - Bullseye minigame: projectile becomes bigger (SizeIncrease)
// - BombsAway minigame: projectile explodes in a strong blast (Explosion)
/// </summary>
public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance { get; private set; }

    private Dictionary<PowerUpType, int> powerUps = new Dictionary<PowerUpType, int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }

    /// <summary>
    //  Grants a power-up of the specified type, keeping track of its count.
    /// </summary>
    public void EarnPowerUp(PowerUpType type)
    {
        if (!powerUps.ContainsKey(type))
            powerUps[type] = 0;
        
        powerUps[type]++;
        Debug.Log($"Power-up earned: {type}. Total: {powerUps[type]}");
    }

    public bool HasPowerUp(PowerUpType type)
    {
        return powerUps.ContainsKey(type) && powerUps[type] > 0;
    }

    public void UsePowerUp(PowerUpType type)
    {
        if (HasPowerUp(type)){
            powerUps[type]--;
        }
    }

    /// <summary>
    //  Prevent power ups from being used from one game to another.
    /// </summary>
    public void ResetPowerUps()
    {
        powerUps = new Dictionary<PowerUpType, int>();;
    }
}
