using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{
    public float strength; //força
    public float magic_power; //poder mágico
    public float defense; //defesa
	public float creatureHealth = 100f; //saúde inicial da criatura
    public float fitness;
    public int number_of_turns; //número de turnos que a criatura viveu
    
    public Enemy(float rnd1, float rnd2, float rnd3)
    {
        strength = rnd1;
        magic_power = rnd2;
        defense = rnd3;
        number_of_turns = 0;
    }
    
    public Enemy(Enemy parent, Enemy partner, float mutationRate = 0.01f)
    {
        number_of_turns = 0;
        
        float mutationChance = Random.Range(0.0f, 1.0f); //calcula a chance da criatura mutar
        
        if(mutationChance <= mutationRate) //a criatura muta se a condição for satisfeita...
        {
            strength = Random.Range(11f, 20f);
            magic_power = Random.Range(11f, 20f);
            defense = Random.Range(0f, 10f);
        }
        else //...senão ela recebe os genes dos pais
        {

            int chance = Random.Range(0, 2); //calcula qual dos pais passará seu gene
                
            //força
            if(chance == 0) //recebe o gene de parent
            {
                strength = parent.strength;
            }
            else //recebe o gene de partner
            {
                strength = partner.strength;
            }

            chance = Random.Range(0, 2);

            //poder mágico
            if(chance == 0)
            {
                magic_power = parent.magic_power;
            }
            else
            {
                magic_power = partner.magic_power;
            }
                
            chance = Random.Range(0, 2);

            //defesa
            if(chance == 0)
            {
                defense = parent.defense;
            }
            else
            {
                defense = partner.defense;
            }
                
        }
        
    }

    //cálculo do fitness da criatura
    public float getFitness(int max)
    {
        float dif = max - number_of_turns;

        if(dif < 0){
            dif = max - dif; 
        }
        
        if(dif == 0)
        {
            dif = 0.0001f;
        }

        fitness = dif;
        
        return fitness;
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
        creatureHealth = creatureHealth - total;
    }

}


