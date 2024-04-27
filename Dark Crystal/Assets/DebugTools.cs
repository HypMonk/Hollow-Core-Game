using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugTools : MonoBehaviour
{
    public bool enableCrystalVisualizer = false;
    public bool enableSpawnOverride = false;
    public Mob mob;
    public int mobDropdownChoice;
    public int mobSpawnAmount;
    public int mobSpawnTimer;

    public void ToggleCrystalVisualizer()
    {
        if (enableCrystalVisualizer) { enableCrystalVisualizer = false; } else { enableCrystalVisualizer = true; }

        GameObject[] spawnedCrystals;
        spawnedCrystals = GameObject.FindGameObjectsWithTag("DarkCrystal");

        foreach (var crystal in spawnedCrystals)
        {
            crystal.GetComponent<CrystalUI>().canvas.SetActive(enableCrystalVisualizer);
        }

        for (int i = 0; i < spawnedCrystals.Length; i++)
        {
            spawnedCrystals[i] = null;
        }
    }

    public void ToggleSpawnOverride()
    {
        if (enableSpawnOverride) { enableSpawnOverride = false; } else { enableSpawnOverride = true; }

        Debug.Log(enableSpawnOverride);

        GameObject[] spawnedCrystals;
        spawnedCrystals = GameObject.FindGameObjectsWithTag("DarkCrystal");

        foreach (var crystal in spawnedCrystals)
        {
            crystal.GetComponent<CrystalController>().overrideSpawning = enableSpawnOverride;
        }

        for (int i = 0; i < spawnedCrystals.Length; i++)
        {
            spawnedCrystals[i] = null;
        }
    }

    public void UpdateMobDropdown(TMP_Dropdown dropdown)
    {
        mobDropdownChoice = dropdown.value;

        GameObject[] spawnedCrystals;
        spawnedCrystals = GameObject.FindGameObjectsWithTag("DarkCrystal");

        foreach (var crystal in spawnedCrystals)
        {
            crystal.GetComponent<CrystalController>().mob = (Mob)mobDropdownChoice;
        }

        for (int i = 0; i < spawnedCrystals.Length; i++)
        {
            spawnedCrystals[i] = null;
        }
    }

    public void UpdateMobSpawnAmount(Slider slider)
    {
        mobSpawnAmount = (int)slider.value;

        GameObject[] spawnedCrystals;
        spawnedCrystals = GameObject.FindGameObjectsWithTag("DarkCrystal");

        foreach (var crystal in spawnedCrystals)
        {
            crystal.GetComponent<CrystalController>().overideSpawnAmount = mobSpawnAmount;
        }

        for (int i = 0; i < spawnedCrystals.Length; i++)
        {
            spawnedCrystals[i] = null;
        }
    }

    public void UpdateMobSpawnTimer(Slider slider)
    {
        mobSpawnTimer = (int)slider.value;

        GameObject[] spawnedCrystals;
        spawnedCrystals = GameObject.FindGameObjectsWithTag("DarkCrystal");

        foreach (var crystal in spawnedCrystals)
        {
            crystal.GetComponent<CrystalController>().overrideSpawnTimer = mobSpawnTimer;
        }

        for (int i = 0; i < spawnedCrystals.Length; i++)
        {
            spawnedCrystals[i] = null;
        }
    }
}
