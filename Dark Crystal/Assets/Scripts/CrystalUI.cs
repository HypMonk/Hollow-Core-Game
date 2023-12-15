using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CrystalUI : MonoBehaviour
{
    [SerializeField]
    DarkCrystal crystalStats;
    [SerializeField]
    CrystalController controller;
    [SerializeField]
    TMP_Text _coolDownTimer, _energyAmount, _healthAmount;
    [SerializeField]
    Slider _healthBar, _energyBar;
    [SerializeField]
    Image _inLightIndicator, _rechargingIndicator;

    // Update is called once per frame
    void Update()
    {
        _healthBar.value = crystalStats.Health;
        _energyBar.value = crystalStats.SpawnEnergy;
        _energyAmount.text = crystalStats.SpawnEnergy.ToString();
        _healthAmount.text = crystalStats.Health.ToString();

        if (controller.spawnCoolDownTimer > 0)
        {
            _coolDownTimer.text = System.TimeSpan.FromSeconds(controller.spawnCoolDownTimer).Seconds.ToString();
        } else
        {
            _coolDownTimer.text = "";
        }

        if (crystalStats.InLight)
        {
            _inLightIndicator.enabled = true;
        }
        else
        {
            _inLightIndicator.enabled = false;
        }

        if (crystalStats.IsRecharging)
        {
            _rechargingIndicator.enabled = true;
        } else
        {
            _rechargingIndicator.enabled = false;
        }
    }
}
