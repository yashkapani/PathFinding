using UnityEngine;
using System.Collections.Generic;

namespace AISandbox {
    public class GridNode : MonoBehaviour {
        public Grid grid;
        public int column;
        public int row;

        private SpriteRenderer _renderer;
        public Color _orig_color;
        private Color _blocked_color;
        private Color _road_color;
        private Color _forest_color;
        private Color _swamp_color;
        private Color _showPathColor;

        [SerializeField]
        private bool _blocked = false;
        [SerializeField]
        private bool _road = false;
        [SerializeField]
        private bool _forest = false;
        [SerializeField]
        private bool _swamp = false;
        [SerializeField]
        private bool _showPath = false;

        public GridNode parent;
        public float cost;

        public bool blocked {
            get {
                return _blocked;
            }
            set {
                _blocked = value;
                if(_renderer)
                    _renderer.color = _blocked ? _blocked_color : _orig_color;
            }
        }



        public bool road
        {
            get
            {
                return _road;
            }
            set
            {
                _road = value;
                if (_renderer)
                    _renderer.color = _road ? _road_color : _orig_color;
            }
        }

        public bool forest
        {
            get
            {
                return _forest;
            }
            set
            {
                _forest = value;
                if (_renderer)
                    _renderer.color = _forest ? _forest_color : _orig_color;
            }
        }

        public bool swamp
        {
            get
            {
                return _swamp;
            }
            set
            {
                _swamp = value;
                if (_renderer)
                    _renderer.color = _swamp ? _swamp_color : _orig_color;
            }
        }

        public bool showPath
        {
            get
            {
                return _showPath;
            }
            set
            {
                _showPath = value;
                if (_renderer)
                {
                    if (_showPath)
                        _renderer.color = _showPathColor;
                    else
                    {
                        if (blocked)
                            blocked = true;
                        else if (road)
                            road = true;
                        else if (forest)
                            forest = true;
                        else if (swamp)
                            swamp = true;
                        else
                            _renderer.color = _orig_color;
                    }
                }
            }
        }

        private void Awake() {
            _renderer = GetComponent<SpriteRenderer>();
            _orig_color = _renderer.color;
            _blocked_color = new Color( _orig_color.r * 0.5f, _orig_color.g * 0.5f, _orig_color.b * 0.5f );
            _forest_color = new Color(0, 0.8f, 1);
            _swamp_color = new Color(0.2f, 0.6f, 0.2f);
            _road_color = new Color(0.6f, 0.2f, 0.2f);
            _showPathColor = new Color(0, 1.0f, 0.0f);
        }

        public void ResetColor()
        {
            _renderer.color = _orig_color;
        }

        public IList<GridNode> GetNeighbors( bool include_diagonal = false ) {
            return grid.GetNodeNeighbors( row, column, include_diagonal );
        }
    }
}