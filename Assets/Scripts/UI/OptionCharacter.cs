//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class OptionCharacter : MonoBehaviour
//{

//    public Dropdown dropDownClassPlayer;

//    public Dropdown dropDownWeaponPlayer;
//    // Start is called before the first frame update
//    void Start()
//    {
//        PopulatePlayerClassList();
//        PopulatePlayerWeaponList();
//    }

//    // Update is called once per frame
//    void Update()
//    {
       
//    }

//    //populate dropdown list with player class
//    void PopulatePlayerClassList()
//    {
//        dropDownClassPlayer.ClearOptions();
//        string[] enumClass = Enum.GetNames(typeof(Player));
//        List<string> classNames = new List<string>(enumClass);
//        dropDownClassPlayer.AddOptions(classNames);
//    }


//    //populate dropdown list with Weapon 
//    void PopulatePlayerWeaponList()
//    {
//        dropDownWeaponPlayer.ClearOptions();
//        string[] enumWeapon = Enum.GetNames(typeof(Weapon));
//        List<string> weaponNames = new List<string>(enumWeapon);
//        dropDownWeaponPlayer.AddOptions(weaponNames);
//    }
//}
