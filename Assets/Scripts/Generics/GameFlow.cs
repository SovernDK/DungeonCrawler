using System.Collections;
using System.Collections.Generic;
using Crawler.Actors;
using Crawler.Commands;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlow : MonoBehaviour
{
    private Queue<ActorCommand> commands;
    private MainContext mainContext;
    private List<IActorContext> actors;

    [SerializeField]
    private float timeLimitForEnemies;
    private float timeLimitCurrent;
    
    public List<IActorContext> Actors { get => actors; set => actors = value; }
    public Queue<ActorCommand> Commands { get => commands; set => commands = value; }

    public void Initialize(MainContext context)
    {
        mainContext = context;
        commands = new Queue<ActorCommand>();
        actors = new List<IActorContext>();

        Player player = FindObjectOfType<Player>();
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        actors.Add(player);

        enemies.ForEach(enemy => 
        {
            enemy.Initialize(context);
            actors.Add(enemy);
        });

        StartCoroutine(Flow());
    }

    public void AddCommand(string name, Actor actor)
    {
        ActorCommand command = actor.Commands.Find(command => command.Name == name);
        if(command == null)
        {
            Debug.LogError("There is not command by name " + name);
        }
        
        commands.Enqueue(command);
    }

    public IEnumerator Flow()
    {
        while (true)
        {
            foreach (IActorContext currentActor in actors)
            {
                if(!currentActor.Alive) continue;
                StartCoroutine(currentActor.OnTurnStart());
                
                while(true) 
                {
                    timeLimitCurrent += Time.deltaTime;
                    if(!mainContext.player.Actor.Alive)
                    {
                        break;
                    }

                    StartCoroutine(currentActor.OnActionStart());
                    
                    while(currentActor.Busy)
                    {
                        yield return null;
                    }

                    yield return StartCoroutine(currentActor.Act());

                    while (commands.Count > 0)
                    {
                        ActorCommand command = commands.Dequeue();
                        yield return StartCoroutine(command.Execute());
                    }

                    StartCoroutine(currentActor.OnActionEnd());
                    if(timeLimitCurrent > timeLimitForEnemies && currentActor is not Player)
                    {
                        currentActor.Done = true;
                    }

                    if(currentActor.Done)
                    {
                        StartCoroutine(currentActor.OnTurnEnd());
                        break;
                    }

                   
                }
            }

            if(!mainContext.player.Actor.Alive)
            {
                break;
            }

            actors = actors.FindAll(actor => actor.Alive);
            yield return null;
        }

        Debug.Log("Game Over");
        Gamemaster.Instance.GameOver();
    }
}
