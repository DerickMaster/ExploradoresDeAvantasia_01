using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurtlesManager : MonoBehaviour
{
    private Dictionary<int, TurtleBehaviour[]> dictTurtles;

    private void Start()
    {
        TurtleBehaviour[] turtles = GetComponentsInChildren<TurtleBehaviour>();
        dictTurtles = new Dictionary<int, TurtleBehaviour[]>();

        int count = turtles.Length;
        int id = 0;
        while(count > 0)
        {
            dictTurtles.Add(id, turtles.Where(turtle => turtle.Id == id).ToArray());
            count -= dictTurtles[id].Length;
            id++;
        }

        TurtlesPathActivator[] activators = GetComponentsInChildren<TurtlesPathActivator>();
        foreach (TurtlesPathActivator activator in activators)
        {
            RegisterActivator(activator);
        }
    }

    public void RegisterActivator(TurtlesPathActivator activator)
    {
        activator.activateTurtlesEvent.AddListener(ActivateTurtles);
    }

    private void ActivateTurtles(int id = 0, bool active = true)
    {
        foreach (TurtleBehaviour turtle in dictTurtles[id])
        {
            turtle.SetActivated(active);
        }
    }
}
