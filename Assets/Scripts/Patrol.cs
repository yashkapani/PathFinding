using UnityEngine;
using System.Collections;
using System;

namespace AISandbox
{
    public class Patrol : StateMachine.State
    {
        public Pathfinding pathFinding;
        public GridNode[] destination;

        int currentDestination;
        public override string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public override void Enter()
        {
            pathFinding.setDestination(destination[0]);
            currentDestination = 0;
        }

        public override void Exit()
        {
            
        }

        public override void Update()
        {
            if (!pathFinding.pathFound)
            {
                pathFinding.doAstar();
            }
            else
            {
                if (!pathFinding.pathFollowing)
                {
                    if (pathFinding.PathNodes.Count > 0)
                    {
                        pathFinding.nodeToseek = pathFinding.PathNodes.Pop();
                        pathFinding.pathFollowing = true;
                    }
                }
                if (pathFinding.plotPath())
                {
                    pathFinding.setDestination(currentDestination == 0 ? destination[1] : destination[0]);
                    currentDestination = currentDestination == 0 ? 1 : 0;
                }
                else
                {

                }
            }
        }
    }
}