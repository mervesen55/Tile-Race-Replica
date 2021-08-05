using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class RankTextController : MonoBehaviour
{
    public TMP_Text RankText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RankText.text = GameManager.instance.PlayerRant.ToString();
    }
}
