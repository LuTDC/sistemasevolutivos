using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public float strength; //força
    public float magic_power; //poder mágico
    public float defense; //defesa
    public float playerHealth = 100f; //vida total do player

    public Player(float rnd1, float rnd2, float rnd3){
        strength = rnd1;
        magic_power = rnd2;
        defense = rnd3;
    }

    public float turn(){
        int choice = Random.Range(0, 2);
        if(choice == 0){
            return strength;
        }
        if(choice == 1){
            return magic_power;
        }
        return 0;
    }

    public void defend(float damage){
        float total = damage - defense;
        playerHealth = playerHealth - total;
    }

}
