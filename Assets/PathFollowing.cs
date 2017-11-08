using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace AISandbox
{
    public class PathFollowing : MonoBehaviour
    {
        static int blockedCount = 10;
        static int roadCount = 10;
        static int forestCount = 10;
        static int swampCount = 10;

        List<GridNode> terrainNodes;

        //GridNode redDoornode, greenDoornode, blueDoornode;
        //GridNode redKeyNode, blueKeynode, greenKeyNode;
        //GridNode treasureNode;
        //StateMachine stateMachine;

        private const float SPAWN_RANGE = 10.0f;

        public Grid grid;
        private Queue<GameObject> _flock = new Queue<GameObject>();
        public GameObject _triangleActorPrefab;
        //public GameObject _doorPrefab;
        //public GameObject _keyPrefab;
        //public GameObject _treasurePrefab;
        //public GameObject _openPrefab;

        //void SetDoors()
        //{
        //    GridNode doornode = grid.GetNode(Random.Range(0, 30), Random.Range(0, 30));
        //        doornode = grid.GetNode(Random.Range(0, 30), Random.Range(0, 30));
        //    GameObject redDoor = Instantiate<GameObject>(_doorPrefab);
        //    redDoor.GetComponent<SpriteRenderer>().color = Color.red;
        //    redDoor.name = "RedDoor";
        //    redDoor.transform.position = new Vector3(doornode.transform.position.x, doornode.transform.position.y, -0.2f);
        //    doornode.blocked = true;
        //    redDoor.SetActive(true);
        //    redDoornode = doornode;

        //    doornode = grid.GetNode(Random.Range(0, 30), Random.Range(0, 30));
        //    redDoor = Instantiate<GameObject>(_doorPrefab);
        //    redDoor.GetComponent<SpriteRenderer>().color = Color.green;
        //    redDoor.name = "GreenDoor";
        //    redDoor.transform.position = new Vector3(doornode.transform.position.x, doornode.transform.position.y, -0.2f);
        //    doornode.blocked = true;
        //    redDoor.SetActive(true);
        //    greenDoornode = doornode;

        //    doornode = grid.GetNode(Random.Range(0, 30), Random.Range(0, 30));
        //    redDoor = Instantiate<GameObject>(_doorPrefab);
        //    redDoor.GetComponent<SpriteRenderer>().color = Color.blue;
        //    redDoor.name = "BlueDoor";
        //    redDoor.transform.position = new Vector3(doornode.transform.position.x, doornode.transform.position.y, -0.2f);
        //    doornode.blocked = true;
        //    redDoor.SetActive(true);
        //    blueDoornode = doornode;
        //}

        public GameObject GetActor()
        {
            return _flock.Peek();
        }

        //void AddStates()
        //{
        //    SeekKey redKeyState = new SeekKey();
        //    redKeyState.pathFinding = GetActor().GetComponent<Pathfinding>();
        //    redKeyState.destination = redKeyNode;
        //    redKeyState.Name = "Seeking Red Key";
        //    redKeyState.key = GameObject.Find("RedKey");
        //    redKeyState.door = redDoornode;

        //    stateMachine.AddState(redKeyState);

        //    SeekKey blueKeyState = new SeekKey();
        //    blueKeyState.pathFinding = GetActor().GetComponent<Pathfinding>();
        //    blueKeyState.destination = blueKeynode;
        //    blueKeyState.Name = "Seeking Blue Key";
        //    blueKeyState.key = GameObject.Find("BlueKey");
        //    redKeyState.nextState = blueKeyState;
        //    blueKeyState.door = blueDoornode;

        //    stateMachine.AddState(blueKeyState);

        //    SeekKey greenKeyState = new SeekKey();
        //    greenKeyState.pathFinding = GetActor().GetComponent<Pathfinding>();
        //    greenKeyState.destination = greenKeyNode;
        //    greenKeyState.Name = "Seeking Green Key";
        //    greenKeyState.key = GameObject.Find("GreenKey");
        //    blueKeyState.nextState = greenKeyState;
        //    greenKeyState.door = greenDoornode;

        //    stateMachine.AddState(redKeyState);

        //    SeekDoor redDoorState = new SeekDoor();
        //    redDoorState.pathFinding = GetActor().GetComponent<Pathfinding>();
        //    redDoorState.destination = redDoornode;
        //    redDoorState.Name = "Seeking Red Door";
        //    redDoorState.door = GameObject.Find("RedDoor");
        //    redDoorState.openDoor = _openPrefab;
        //    greenKeyState.nextState = redDoorState;

        //    stateMachine.AddState(redDoorState);

        //    SeekDoor blueDoorState = new SeekDoor();
        //    blueDoorState.pathFinding = GetActor().GetComponent<Pathfinding>();
        //    blueDoorState.destination = blueDoornode;
        //    blueDoorState.Name = "Seeking Blue Door";
        //    blueDoorState.door = GameObject.Find("BlueDoor");
        //    blueDoorState.openDoor = _openPrefab;
        //    redDoorState.nextState = blueDoorState;

        //    stateMachine.AddState(blueDoorState);

        //    SeekDoor greenDoorState = new SeekDoor();
        //    greenDoorState.pathFinding = GetActor().GetComponent<Pathfinding>();
        //    greenDoorState.destination = greenDoornode;
        //    greenDoorState.Name = "Seeking Green Door";
        //    greenDoorState.door = GameObject.Find("GreenDoor");
        //    greenDoorState.openDoor = _openPrefab;
        //    blueDoorState.nextState = greenDoorState;

        //    stateMachine.AddState(greenDoorState);

        //    SeekTreasure treasureState = new SeekTreasure();
        //    treasureState.pathFinding = GetActor().GetComponent<Pathfinding>();
        //    treasureState.destination = treasureNode;
        //    treasureState.Name = "Seeking Treasure";
        //    treasureState.treasure = GameObject.Find("Treasure");
        //    greenDoorState.nextState = treasureState;

        //    treasureState.nextState = null;

        //    stateMachine.SetActiveState(redKeyState);
        //}

        //void SetKeys()
        //{
        //    GridNode doornode = grid.GetNode(Random.Range(0, 30), Random.Range(0, 30));
        //    while (doornode.blocked)
        //        doornode = grid.GetNode(Random.Range(0, 30), Random.Range(0, 30));
        //    GameObject redDoor = Instantiate<GameObject>(_keyPrefab);
        //    redDoor.GetComponent<SpriteRenderer>().color = Color.red;
        //    redDoor.transform.position = new Vector3(doornode.transform.position.x, doornode.transform.position.y, -0.2f);
        //    redDoor.name = "RedKey";
        //    redDoor.SetActive(true);
        //    redKeyNode = doornode;

        //    doornode = grid.GetNode(Random.Range(0, 30), Random.Range(0, 30));
        //    while (doornode.blocked)
        //        doornode = grid.GetNode(Random.Range(0, 30), Random.Range(0, 30));
        //    redDoor = Instantiate<GameObject>(_keyPrefab);
        //    redDoor.GetComponent<SpriteRenderer>().color = Color.green;
        //    redDoor.transform.position = new Vector3(doornode.transform.position.x, doornode.transform.position.y, -0.2f);
        //    redDoor.name = "GreenKey";
        //    redDoor.SetActive(true);
        //    greenKeyNode = doornode;

        //    doornode = grid.GetNode(Random.Range(0, 30), Random.Range(0, 30));
        //    while (doornode.blocked)
        //        doornode = grid.GetNode(Random.Range(0, 30), Random.Range(0, 30));
        //    redDoor = Instantiate<GameObject>(_keyPrefab);
        //    redDoor.GetComponent<SpriteRenderer>().color = Color.blue;
        //    redDoor.transform.position = new Vector3(doornode.transform.position.x, doornode.transform.position.y, -0.2f);
        //    redDoor.name = "BlueKey";
        //    redDoor.SetActive(true);
        //    blueKeynode = doornode;
        //}

        //void SetTreasure()
        //{
        //    GridNode doornode = grid.GetNode(Random.Range(0, 30), Random.Range(0, 30));
        //    while (doornode.blocked)
        //        doornode = grid.GetNode(Random.Range(0, 30), Random.Range(0, 30));
        //    GameObject redDoor = Instantiate<GameObject>(_treasurePrefab);
        //    redDoor.GetComponent<SpriteRenderer>().color = Color.yellow;
        //    redDoor.name = "Treasure";
        //    redDoor.transform.position = new Vector3(doornode.transform.position.x, doornode.transform.position.y, -0.2f);
        //    redDoor.SetActive(true);
        //    doornode.blocked = true;
        //    treasureNode = doornode;
        //}

        private GameObject CreateActor()
        {
            GameObject newActor = Instantiate<GameObject>(_triangleActorPrefab);
            newActor.gameObject.name = "Actor";
            newActor.gameObject.tag = "Actor";
            newActor.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
            newActor.transform.position = new Vector3(Random.Range(-SPAWN_RANGE, SPAWN_RANGE), Random.Range(-SPAWN_RANGE, SPAWN_RANGE), -0.2f);
            newActor.GetComponent<OrientedActor>().initialVelocity = Random.onUnitSphere * Random.Range(0.0f, newActor.GetComponent<IActor>().MaxSpeed);
            return newActor;
        }

        // Use this for initialization
        void Start()
        {
            //stateMachine = new StateMachine();
            terrainNodes = new List<GridNode>();
            grid.Create(30, 30);
            Vector2 gridSize = grid.size;
            Vector2 gridPos = new Vector2(gridSize.x * -0.5f, gridSize.y * 0.5f);
            grid.transform.position = gridPos;


            while(terrainNodes.Count < 40)
            {
                GridNode node = grid.GetNode(Random.Range(0, 30), Random.Range(0, 30));
                int cost = Random.Range(0, 5);
                if (!terrainNodes.Contains(node))
                {
                    if(cost != 1)
                        terrainNodes.Add(node);
                    switch (cost)
                    {
                        case 0:
                            if (blockedCount > 0)
                            {
                                node.blocked = true;
                                blockedCount--;
                            }
                            break;
                        case 1:
                            node.cost = cost;
                            break;
                        case 2:
                            if (roadCount > 0)
                            {
                                node.road = true;
                                roadCount--;
                                node.cost = cost;
                            }
                            break;
                        case 3:
                            if (forestCount > 0)
                            {
                                node.forest = true;
                                forestCount--;
                                node.cost = cost;
                            }
                            break;
                        case 4:
                            if (swampCount > 0)
                            {
                                node.swamp = true;
                                swampCount--;
                            }
                            break;
                    }
                }
            }
            //SetDoors();
            //SetKeys();
            //SetTreasure();
            for (int i = 0; i < 10; i++)
            {
                GameObject newActor;
                newActor = CreateActor();
                GameObject canvas = GameObject.Find("Canvas");
                newActor.GetComponent<Pathfinding>().Initialize();
                newActor.GetComponent<Pathfinding>().showPathToggle = canvas.transform.GetChild(i).GetComponentInChildren<Toggle>();
                newActor.GetComponent<Pathfinding>().triangleImage = canvas.transform.GetChild(i).GetChild(1).GetComponent<Image>();
                newActor.GetComponent<Pathfinding>().triangleImage.color = newActor.GetComponent<SpriteRenderer>().color;

                newActor.GetComponent<Pathfinding>().setDestination(grid.GetNode(Random.Range(0, 30), Random.Range(0, 30)));
                newActor.SetActive(true);
                _flock.Enqueue(newActor);
            }
            //AddStates();
        }

        // Update is called once per frame
        void Update()
        {
            //if (!stateMachine.processed)
            //    stateMachine.Update();
            //else
            //{
            //    GetActor().GetComponent<IActor>().Velocity = Vector3.zero;
            //    if (Input.GetKeyDown(KeyCode.R))
            //        Application.LoadLevel(Application.loadedLevel);
            //}
            foreach (GameObject obj in _flock)
            {
                Pathfinding actorPathFinder = obj.GetComponent<Pathfinding>();
                if (!actorPathFinder.pathFound)
                {
                    actorPathFinder.doAstar();
                }
                else
                {
                    if (!actorPathFinder.pathFollowing)
                    {
                        actorPathFinder.nodeToseek = actorPathFinder.PathNodes.Pop();
                        actorPathFinder.pathFollowing = true;
                    }
                    if (actorPathFinder.plotPath())
                    {
                        actorPathFinder.setDestination(grid.GetNode(Random.Range(0, 30), Random.Range(0, 30)));
                    }
                }
            }
        }
    }
}
