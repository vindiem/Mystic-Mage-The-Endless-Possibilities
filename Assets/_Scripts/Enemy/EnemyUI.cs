using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    private EnemyAttackSystem enemyAttackSystem;
    private EnemyMovement enemyMovement;

    public Text elementText;

    // Health imagine
    public Image healthBackground;
    public Image healthImage;
    public Text healthText;

    // Relics
    private Animator relicPlate;

    private void Start()
    {
        enemyAttackSystem = GetComponent<EnemyAttackSystem>();
        enemyMovement = GetComponent<EnemyMovement>();

        // Relic A
        relicPlate = GameObject.FindGameObjectWithTag("RelicPlate").GetComponent<Animator>();

    }

    private void Update()
    {
        #region UI set

        if (enemyMovement.health < 0) enemyMovement.health = 0;

        Vector3 cameraPosition = Camera.main.transform.position;

        healthBackground.transform.LookAt(cameraPosition);
        healthImage.transform.LookAt(cameraPosition);
        elementText.transform.LookAt(cameraPosition);
        healthText.transform.LookAt(cameraPosition);

        healthImage.fillAmount = enemyMovement.health / enemyMovement.maxHealth;
        healthText.text = $"{(int)enemyMovement.health} / {(int)enemyMovement.maxHealth}";

        #endregion
    }

    // Relics
    public void RelicAchievement(int relicLevel, string nameOfRelic)
    {
        string MeteorBlock,
            TornadoBlock,
            FireBlock,
            WaveBlock,
            UltimateBlock;

        MeteorBlock = PlayerPrefs.GetString("MeteorBlock");
        TornadoBlock = PlayerPrefs.GetString("TornadoBlock");
        FireBlock = PlayerPrefs.GetString("FireBlock");
        WaveBlock = PlayerPrefs.GetString("WaveBlock");
        UltimateBlock = PlayerPrefs.GetString("UltimateBlock");

        switch (nameOfRelic)
        {
            case "Meteor":
                if (MeteorBlock == "false")
                {
                    float mmrp = PlayerPrefs.GetFloat("mMRP");

                    if (relicLevel > mmrp && relicLevel <= 1280)
                    {
                        Text relicPlateText = relicPlate.GetComponentInChildren<Text>();
                        relicPlateText.text = "New meteor relic reached";
                        relicPlate.SetTrigger("On");

                        int coinsCount = PlayerPrefs.GetInt("Coins");
                        coinsCount += (int)mmrp;
                        PlayerPrefs.SetInt("Coins", coinsCount);

                        mmrp *= 2;
                    }
                    else if (relicLevel > 1280)
                    {
                        mmrp = 1;
                    }

                    PlayerPrefs.SetFloat("mMRP", mmrp);
                }
                break;
            case "Tornado":
                if (TornadoBlock == "false")
                {
                    float tmrp = PlayerPrefs.GetFloat("mTRP");
                    if (relicLevel > tmrp && relicLevel <= 1280)
                    {
                        Text relicPlateText = relicPlate.GetComponentInChildren<Text>();
                        relicPlateText.text = "New tornado relic reached";
                        relicPlate.SetTrigger("On");

                        int coinsCount = PlayerPrefs.GetInt("Coins");
                        coinsCount += (int)tmrp;
                        PlayerPrefs.SetInt("Coins", coinsCount);

                        tmrp *= 2;
                    }
                    else if (relicLevel > 1280)
                    {
                        tmrp = 1;
                    }
                    PlayerPrefs.SetFloat("mTRP", tmrp);
                }
                break;
            case "Fire":
                if (FireBlock == "false")
                {
                    float fmrp = PlayerPrefs.GetFloat("mFRP");
                    if (relicLevel > fmrp && relicLevel <= 1280)
                    {
                        Text relicPlateText = relicPlate.GetComponentInChildren<Text>();
                        relicPlateText.text = "New fire relic reached";
                        relicPlate.SetTrigger("On");

                        int coinsCount = PlayerPrefs.GetInt("Coins");
                        coinsCount += (int)fmrp;
                        PlayerPrefs.SetInt("Coins", coinsCount);

                        fmrp *= 2;
                    }
                    else if (relicLevel > 1280)
                    {
                        fmrp = 1;
                    }
                    PlayerPrefs.SetFloat("mFRP", fmrp);
                }
                break;
            case "Wave":
                if (WaveBlock == "false")
                {
                    float wmrp = PlayerPrefs.GetFloat("mWRP");
                    if (relicLevel > wmrp && relicLevel <= 1280)
                    {
                        Text relicPlateText = relicPlate.GetComponentInChildren<Text>();
                        relicPlateText.text = "New wave relic reached";
                        relicPlate.SetTrigger("On");

                        int coinsCount = PlayerPrefs.GetInt("Coins");
                        coinsCount += (int)wmrp;
                        PlayerPrefs.SetInt("Coins", coinsCount);

                        wmrp *= 2;
                    }
                    else if (relicLevel > 1280)
                    {
                        wmrp = 1;
                    }
                    PlayerPrefs.SetFloat("mWRP", wmrp);
                }
                break;
            case "Ultimate":
                if (UltimateBlock == "false")
                {
                    float umrp = PlayerPrefs.GetFloat("mURP");
                    if (relicLevel > umrp && relicLevel <= 1280)
                    {
                        Text relicPlateText = relicPlate.GetComponentInChildren<Text>();
                        relicPlateText.text = "New ULTIMATE relic reached";
                        relicPlate.SetTrigger("On");

                        int coinsCount = PlayerPrefs.GetInt("Coins");
                        coinsCount += (int)umrp;
                        PlayerPrefs.SetInt("Coins", coinsCount);

                        umrp *= 2;
                    }
                    else if (relicLevel > 1280)
                    {
                        umrp = 1;
                    }
                    PlayerPrefs.SetFloat("mURP", umrp);
                }
                break;
        }
    }

    public void RelicKillUpdate(string skillName)
    {
        // If death check if relic has been reached
        if (enemyMovement.health <= 0 || transform.position.y <= -10f)
        {
            string MeteorBlock,
                TornadoBlock,
                FireBlock,
                WaveBlock,
                UltimateBlock;

            MeteorBlock = PlayerPrefs.GetString("MeteorBlock");
            TornadoBlock = PlayerPrefs.GetString("TornadoBlock");
            FireBlock = PlayerPrefs.GetString("FireBlock");
            WaveBlock = PlayerPrefs.GetString("WaveBlock");
            UltimateBlock = PlayerPrefs.GetString("UltimateBlock");

            switch (skillName)
            {
                case "Meteor":
                    if (enemyMovement.meteorDeath == false && MeteorBlock == "false")
                    {
                        enemyMovement.meteorDeath = true;
                        int MRP = PlayerPrefs.GetInt("Meteor relic progress");
                        MRP++;
                        RelicAchievement(MRP, "Meteor");
                        PlayerPrefs.SetInt("Meteor relic progress", MRP);
                    }
                    break;
                case "Tornado":
                    if (enemyMovement.tornadoDeath == false && TornadoBlock == "false")
                    {
                        enemyMovement.tornadoDeath = true;
                        int TRP = PlayerPrefs.GetInt("Tornado relic progress");
                        TRP++;
                        RelicAchievement(TRP, "Tornado");
                        PlayerPrefs.SetInt("Tornado relic progress", TRP);
                    }
                    break;
                case "Fire":
                    if (enemyMovement.fireDeath == false && FireBlock == "false")
                    {
                        enemyMovement.fireDeath = true;
                        int FRP = PlayerPrefs.GetInt("Fire relic progress");
                        FRP++;
                        RelicAchievement(FRP, "Fire");
                        PlayerPrefs.SetInt("Fire relic progress", FRP);
                    }
                    break;
                case "Wave":
                    if (enemyMovement.waveDeath == false && WaveBlock == "false")
                    {
                        enemyMovement.waveDeath = true;
                        int WRP = PlayerPrefs.GetInt("Wave relic progress");
                        WRP++;
                        RelicAchievement(WRP, "Wave");
                        PlayerPrefs.SetInt("Wave relic progress", WRP);
                    }
                    break;
                case "Ultimate":
                    if (enemyMovement.ultimateDeath == false && UltimateBlock == "false")
                    {
                        enemyMovement.ultimateDeath = true;
                        int URP = PlayerPrefs.GetInt("Ultimate relic progress");
                        URP++;
                        RelicAchievement(URP, "Ultimate");
                        PlayerPrefs.SetInt("Ultimate relic progress", URP);
                    }
                    break;
            }

        }
    }

}