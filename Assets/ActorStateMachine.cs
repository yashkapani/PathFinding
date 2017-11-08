using UnityEngine;
//using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace AISandbox
{
    public class ActorStateMachine : MonoBehaviour
    {
        [SerializeField]
        Grid grid;

        Color color;
        string Tag;

        StateMachine stateMachine;
        SeekTreasure captureFlag;
        Guard guard;
        Patrol patrol;
        CapturePlayer capturePlayer;

        bool alloutAttack, alloutdefense;

        public static bool gameover;

        int bordercol;

        public enum Intent
        {
            Capturing,
            Patrolling,
            Guarding,
            CapturingPlayer,
            Captured,
            Evading
        };

        Intent _intent;
        Intent _oldIntent;

        public Intent intent
        {
            get
            {
                return _intent;
            }
            set
            {
                _intent = value;
            }
        }

        public void MakeDecision()
        {
            bool capturing = false, patrolling = false, guarding = false;
            GameObject[] teammates = GameObject.FindGameObjectsWithTag(tag);
            foreach(GameObject teammate in teammates)
            {
                if(teammate != this.gameObject)
                {
                    switch(teammate.GetComponent<ActorStateMachine>().intent)
                    {
                        case Intent.Guarding:
                            guarding = true;
                            break;
                        case Intent.Patrolling:
                            patrolling = true;
                            break;
                        case Intent.Capturing:
                            capturing = true;
                            break;
                    }
                }
            }

            if (!guarding)
            {
                intent = Intent.Guarding;
                stateMachine.SetActiveState(guard);
            }
            else if (!patrolling)
            {
                intent = Intent.Patrolling;
                stateMachine.SetActiveState(patrol);
            }
            else if (!capturing)
            {
                intent = Intent.Capturing;
                stateMachine.SetActiveState(captureFlag);
            }
            GameObject.Find("CaptureTheFlag").GetComponent<CaptureTheFlag>().ShowEvent(gameObject.GetComponent<SpriteRenderer>().color, intent.ToString(), tag);
        }

        public void Destroy()
        {
            intent = Intent.Captured;
            GameObject.Find("CaptureTheFlag").GetComponent<CaptureTheFlag>().ShowEvent(color, intent.ToString(), Tag);
            GameObject[] teammates = GameObject.FindGameObjectsWithTag(Tag);
            foreach (GameObject teammate in teammates)
            {
                if (teammate != this.gameObject)
                {
                    teammate.GetComponent<ActorStateMachine>().Alert(intent);
                }
            }
        }

        public void Alert(Intent alertintent)
        {
            if (alloutAttack == false && alloutdefense == false)
            {
                switch (alertintent)
                {
                    case Intent.Captured:
                        int offordef = Random.Range(0, 100);
                        if (offordef % 2 == 0)
                        {
                            alloutAttack = true;
                        }
                        else
                        {
                            alloutdefense = true;
                        }
                        break;
                }
            }

            if(alloutAttack)
            {
                if (intent != Intent.CapturingPlayer)
                {
                    intent = Intent.Capturing;
                    GetComponent<OrientedActor>().Velocity = 3 * GetComponent<OrientedActor>().initialVelocity;
                    stateMachine.SetActiveState(captureFlag);
                    GameObject.Find("CaptureTheFlag").GetComponent<CaptureTheFlag>().ShowEvent(gameObject.GetComponent<SpriteRenderer>().color, intent.ToString(), tag);
                }
            }
            else if(alloutdefense)
            {
                if (intent != Intent.CapturingPlayer)
                {
                    intent = Intent.Patrolling;
                    GetComponent<OrientedActor>().Velocity = 3 * GetComponent<OrientedActor>().initialVelocity;
                    stateMachine.SetActiveState(patrol);
                    GameObject.Find("CaptureTheFlag").GetComponent<CaptureTheFlag>().ShowEvent(gameObject.GetComponent<SpriteRenderer>().color, intent.ToString(), tag);
                }
            }
        }

        // Use this for initialization
        void Start()
        {
            gameover = false;
            Tag = tag;
            bordercol = tag == "Red" ? 14 : 16;
            color = gameObject.GetComponent<SpriteRenderer>().color;
            stateMachine = new StateMachine();
            patrol = new Patrol();
            patrol.pathFinding = GetComponent<Pathfinding>();
            guard = new Guard();
            capturePlayer = new CapturePlayer();

            guard.pathFinding = GetComponent<Pathfinding>();
            capturePlayer.pathFinding = GetComponent<Pathfinding>();
            captureFlag = new SeekTreasure();
            captureFlag.pathFinding = GetComponent<Pathfinding>();
            patrol.destination = new GridNode[2];
            guard.destination = new GridNode[4];
            if (CompareTag("Red"))
            {
                captureFlag.destination = CaptureTheFlag.blueTreasureNode;

                patrol.destination[0] = grid.GetNode(5, 7);
                patrol.destination[1] = grid.GetNode(24, 7);

                guard.destination[0] = grid.GetNode(CaptureTheFlag.redTreasureNode.row, CaptureTheFlag.redTreasureNode.column + 1);
                guard.destination[1] = grid.GetNode(CaptureTheFlag.redTreasureNode.row, CaptureTheFlag.redTreasureNode.column - 1);
                guard.destination[2] = grid.GetNode(CaptureTheFlag.redTreasureNode.row + 1, CaptureTheFlag.redTreasureNode.column);
                guard.destination[3] = grid.GetNode(CaptureTheFlag.redTreasureNode.row - 1, CaptureTheFlag.redTreasureNode.column);
            }
            else
            {
                captureFlag.destination = CaptureTheFlag.redTreasureNode;

                patrol.destination[0] = grid.GetNode(5, 22);
                patrol.destination[1] = grid.GetNode(24, 22);

                guard.destination[0] = grid.GetNode(CaptureTheFlag.blueTreasureNode.row, CaptureTheFlag.blueTreasureNode.column + 1);
                guard.destination[1] = grid.GetNode(CaptureTheFlag.blueTreasureNode.row, CaptureTheFlag.blueTreasureNode.column - 1);
                guard.destination[2] = grid.GetNode(CaptureTheFlag.blueTreasureNode.row + 1, CaptureTheFlag.blueTreasureNode.column);
                guard.destination[3] = grid.GetNode(CaptureTheFlag.blueTreasureNode.row - 1, CaptureTheFlag.blueTreasureNode.column);
            }

            stateMachine.AddState(captureFlag);
            stateMachine.AddState(patrol);
            stateMachine.AddState(guard);
            stateMachine.AddState(capturePlayer);
            MakeDecision();
        }

        GameObject[] getEnemies()
        {
            return GameObject.FindGameObjectsWithTag(tag == "Red" ? "Blue" : "Red");
        }

        // Update is called once per frame
        void Update()
        {
            stateMachine.Update();
            if (stateMachine.processed)
                gameover = true;

            if (gameover)
            {
                GetComponent<OrientedActor>().Velocity = Vector2.zero;
                if (captureFlag.transition)
                    GameObject.Find("GameOver").GetComponent<Text>().text = tag + " Team Won! Press 'R' to restart.";
            }
            else
                switch (intent)
                {
                    case Intent.Patrolling:
                        GameObject[] enemies = getEnemies();
                        if(enemies.Length == 0)
                        {
                            intent = Intent.Capturing;
                            GameObject.Find("CaptureTheFlag").GetComponent<CaptureTheFlag>().ShowEvent(gameObject.GetComponent<SpriteRenderer>().color, intent.ToString(), tag);
                            stateMachine.SetActiveState(captureFlag);
                        }
                        foreach (GameObject enemy in enemies)
                        {
                            GridNode node = grid.GetNodeBasedOnPosition(enemy.transform.position);
                            if (node != null && ((node.column < bordercol && bordercol < 15) || (node.column > bordercol && bordercol > 15)))
                            {
                                Stack<GridNode> enemyPath = enemy.GetComponent<Pathfinding>().PathNodes;
                                GetComponent<OrientedActor>().Velocity = 2 * GetComponent<OrientedActor>().initialVelocity;
                                _oldIntent = intent;
                                intent = Intent.CapturingPlayer;
                                GameObject.Find("CaptureTheFlag").GetComponent<CaptureTheFlag>().ShowEvent(gameObject.GetComponent<SpriteRenderer>().color, intent.ToString(), tag);
                                capturePlayer.enemyPath = enemyPath;
                                capturePlayer.culprit = enemy;
                                capturePlayer.pursuer = gameObject;
                                stateMachine.SetActiveState(capturePlayer);
                                break;
                            }
                        }
                        if(intent != Intent.CapturingPlayer)
                        {
                            bool capturing = false;
                            GameObject[] teammates = GameObject.FindGameObjectsWithTag(tag);
                            foreach(GameObject teammate in teammates)
                            {
                                if (teammate != gameObject)
                                    if (teammate.GetComponent<ActorStateMachine>().intent == Intent.Capturing)
                                        capturing = true;
                            }

                            if (!capturing)
                            {
                                intent = Intent.Capturing;
                                stateMachine.SetActiveState(captureFlag);
                                GameObject.Find("CaptureTheFlag").GetComponent<CaptureTheFlag>().ShowEvent(gameObject.GetComponent<SpriteRenderer>().color, intent.ToString(), tag);
                            }
                        }
                        break;
                    case Intent.CapturingPlayer:
                        if (capturePlayer.culprit == null)
                        {
                            alloutdefense = false;
                            alloutAttack = false;
                            intent = Intent.Patrolling;
                            stateMachine.SetActiveState(patrol);
                            GameObject.Find("CaptureTheFlag").GetComponent<CaptureTheFlag>().ShowEvent(gameObject.GetComponent<SpriteRenderer>().color, intent.ToString(), tag);
                        }

                        break;
                }
        }
    }
}
