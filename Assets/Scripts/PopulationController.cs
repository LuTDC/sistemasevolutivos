using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class PopulationController : MonoBehaviour
{
    List<Enemy> population = new List<Enemy>(); //população
    List<Enemy> print_enemies = new List<Enemy>();
    Player player; //jogador
    Enemy enemy_aux; //inimigo auxiliar
    public Enemy fittest; //melhor de todos
    public int maxGen;
    public int populationSize; //tamanho total da população
    int maxLifeTime = 0; //número de turnos máximo atingido por uma criatura
    public float cutoff = 0.3f;
	public float mutationRate = 0.01f; //taxa de mutação
    private float rnd1, rnd2, rnd3; //randoms auxiliares
    public Text player_txt;
    public Text enemy_txt;
    int cont = 1;
    int index = 0;

    private void Start()
    {
        player_txt.text = "";
        enemy_txt.text = "";
        InitPopulation(); 
    }

    //criação da população inicial
    void InitPopulation()
    {
        rnd1 = Random.Range(11f, 20f);
        rnd2 = Random.Range(11f, 20f);
        rnd3 = Random.Range(0f, 10f);

        player = new Player(rnd1, rnd2, rnd3);

        for(int i = 0; i < populationSize; i++)
        {
            rnd1 = Random.Range(11f, 20f);
            rnd2 = Random.Range(11f, 20f);
            rnd3 = Random.Range(0f, 10f);
            enemy_aux = new Enemy(rnd1, rnd2, rnd3);
            population.Add(enemy_aux);
            print_enemies.Add(enemy_aux);
        }
        
        for(int i = 0; i < populationSize; i++){
            player.playerHealth = 100f;
            index = i;
            fight();
            cont++;
        }
        
        NextGenerations();
    }

    //criação das próximas gerações
    void NextGenerations(){
        for(int i = 2; i < maxGen; i++){
            
            int survivorCut = Mathf.RoundToInt(populationSize * cutoff);
            List<Enemy> survivors = new List<Enemy>(); //sobreviventes da geração passada
            for(int j = 0; j < survivorCut; j++)
            {
                survivors.Add(GetFittest()); //melhores sobrevivem
            }

            //destruição daqueles que não foram adequados para sobreviver
            for(int j = 0; j < population.Count; j++)
            {
                population[j] = null;
            }
            population.Clear();

            //adiciona os sobreviventes à nova população
		    for(int j = 0; j < survivorCut; j++)
		    {
			    population.Add(survivors[j]);
            }
            //criação de novas criaturas
            while(population.Count < populationSize)
            {
                for(int j = 0; j < survivors.Count; j++)
                {
                    enemy_aux = new Enemy(survivors[j], survivors[Random.Range(0, survivors.Count - j)], mutationRate);
                    population.Add(enemy_aux);
                    print_enemies.Add(enemy_aux);
                    if(population.Count >= populationSize)
                    {
                        break;
                    }
                }
            }

            //destrói o restante da população anterior
            for(int j = 0; j < survivors.Count; j++)
            {
                survivors[j] = null;
            }

            for(int j = 0; j < population.Count; j++){
                player.playerHealth = 100f;
                index = j;
                fight();
                cont++;
            }
        }
       End();
    }
    
    //busca pelo melhor de todos
    Enemy GetFittest()
    {
        float maxFitness = float.MinValue;
        fittest = population[0];

        for(int i = 0; i < population.Count; i++)
        {
            if(population[i].getFitness(maxLifeTime) > maxFitness)
            {
                maxFitness = population[i].getFitness(maxLifeTime);
                fittest = population[i];
                maxLifeTime = fittest.number_of_turns;
            }
        }

        population.Remove(fittest);
        return fittest;
    }

    //sistema de batalha
    public void fight(){
        
        while(population[index].creatureHealth > 0 && player.playerHealth > 0){
            population[index].defend(player.turn());
            if(population[index].creatureHealth < 0) population[index].creatureHealth = 0;
            
            if(population[index].creatureHealth > 0){
                player.defend(population[index].turn());
                if(player.playerHealth < 0) player.playerHealth = 0;
                (population[index].number_of_turns)++;
            }
        }
    }

    public void End(){
        int aux = 0;
        player_txt.text = "\nForça: " + player.strength + "\nMagia: " + player.magic_power + "\nDefesa: " + player.defense;
        for(int i = 0; i < print_enemies.Count; i++){
            aux = i + 1;
            Debug.Log("Inimigo #" + aux + "\n\n" + "\nForça: " + print_enemies[i].strength + "\nMagia: " + print_enemies[i].magic_power + "\nDefesa: " + print_enemies[i].defense + "\nNúmero de turnos sobrevividos: " + print_enemies[i].number_of_turns + "\n\n");
        }

        enemy_txt.text = "Melhor inimigo encontrado:\n\n\n" + "Força: " + fittest.strength + "\nMagia: " + fittest.magic_power + "\nDefesa: " + fittest.defense + "\nNúmero de turnos sobrevividos: " + fittest.number_of_turns;
    }

}
