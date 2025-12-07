using UnityEngine;
using UnityEngine.UI;  // Use the Slider


public class EnergySystem : MonoBehaviour
{
    [Header("能量设置")]
    public Slider energySlider;        // energybar
    public int energy = 0;             // Current energy (0 - 3)
    public int maxEnergy = 3;          // Maximum energy = 3 (pick up 3 pieces of trash)

    [Header("UI 与预制体")]
    public GameObject energyFullPanel; // UI prompt when energy is full
    public GameObject energyBallPrefab;// Energy ball prefabricated body
    public Transform player;           // Player Transform

    [Header("能量球发射参数")]
    public float shootForce = 7f;      // initial velocity
    public float upwardFactor = 0.05f;  // The proportion of looking up

    private void Start()
    {
        // Set the Slider to integer values ranging from 0 to 3
        if (energySlider != null)
        {
            energySlider.minValue = 0;
            energySlider.maxValue = maxEnergy;
            energySlider.wholeNumbers = true;
            energySlider.value = energy;
        }

        if (energyFullPanel != null)
            energyFullPanel.SetActive(false);

        Debug.Log("Initial Energy: " + energy);
    }

    // +1 energy for each piece of trash picked up
    public void AddEnergy(int amount)
    {
        // Don't add any more when it's full (keep three grids)
        if (energy >= maxEnergy)
            return;

        energy += amount;
        if (energy > maxEnergy)
            energy = maxEnergy;

        if (energySlider != null)
            energySlider.value = energy;

        Debug.Log("Current Energy: " + energy);

        // A prompt UI pops up when fully charged
        if (energy >= maxEnergy && energyFullPanel != null)
        {
            energyFullPanel.SetActive(true);
        }
    }

    private void Update()
    {
        //No longer reset energy to 0. As long as energy is greater than or equal to maxEnergy, you can press E to fire infinitely
        if (energy >= maxEnergy && Input.GetKeyDown(KeyCode.E))
        {
            FireEnergyBall();
        }
    }

    // Turn off the "Energy is full" prompt
    public void CloseEnergyFullPanel()
    {
        if (energyFullPanel != null)
            energyFullPanel.SetActive(false);
    }

    //Launch energy balls
    private void FireEnergyBall()
    {
        if (energyBallPrefab == null || player == null)
        {
            Debug.LogWarning("EnergyBallPrefab or player is not set!");
            return;
        }

        // Generate at a slightly elevated position a little ahead of the player
        Vector3 spawnPos = player.position + player.forward * 1.0f + Vector3.up * 0.5f;
        GameObject energyBall = Instantiate(energyBallPrefab, spawnPos, Quaternion.identity);

        Rigidbody rb = energyBall.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.useGravity = true;

            // Launch direction: Forward + slightly upward
            Vector3 dir = (player.forward + Vector3.up * upwardFactor).normalized;
            rb.AddForce(dir * shootForce, ForceMode.VelocityChange);
        }

        Debug.Log("Energy ball fired!");
    }
}