using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class RankTextController : MonoBehaviour
{
    public TMP_Text RankText;

    

    void Update()
    {
        RankText.text = GameManager.Instance.PlayerRant.ToString();
    }
}
