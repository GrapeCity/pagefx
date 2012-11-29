using System;

namespace DataDynamics.PageFX.CLI.Translation.ControlFlow
{
    /// <summary>
    /// Represents edge in directed graph
    /// </summary>
    internal class Edge
    {
        public Edge(Node from, Node to)
        {
            _from = from;
            _to = to;
        }

        public Node From
        {
            get { return _from; }
            set
            {
                if (value != _from)
                {
                    if (_from != null)
                        _from.RemoveOut(this);
                    _from = value;
                    if (_from != null)
                        _from.AppendOut(this);
                }
            }
        }
        private Node _from;

        public Node To
        {
            get { return _to; }
            set
            {
                if (value != _to)
                {
                    if (_to != null)
                        _to.RemoveIn(this);
                    _to = value;
                    if (_to != null)
                        _to.AppendIn(this);
                }
            }
        }
        private Node _to;

        public Edge NextIn
        {
            get { return _nextIn; }
            set
            {
                if (value != _nextIn)
                {
                    _nextIn = value;
                    if (value != null)
                        value.PrevIn = this;
                }
            }
        }
        private Edge _nextIn;

        public Edge PrevIn
        {
            get { return _prevIn; }
            set
            {
                if (value != _prevIn)
                {
                    _prevIn = value;
                    if (value != null)
                        value.NextIn = this;
                }
            }
        }
        private Edge _prevIn;

        public Edge NextOut
        {
            get { return _nextOut; }
            set
            {
                if (value != _nextOut)
                {
                    _nextOut = value;
                    if (value != null)
                        value.PrevOut = this;
                }
            }
        }
        private Edge _nextOut;

        public Edge PrevOut
        {
            get { return _prevOut; }
            set
            {
                if (value != _prevOut)
                {
                    _prevOut = value;
                    if (value != null)
                        value.NextOut = this;
                }
            }
        }
        private Edge _prevOut;

        #region Flags
        public EdgeFlags Flags
        {
            get { return _flags; }
            set { _flags = value; }
        }
        private EdgeFlags _flags;

        public bool IsVisited
        {
            get { return (_flags & EdgeFlags.Visited) != 0; }
            set
            {
                if (value) _flags |= EdgeFlags.Visited;
                else _flags &= ~EdgeFlags.Visited;
            }
        }
        #endregion

        public EdgeType Type
        {
            get { return _type; }
            set
            {
                if (value != _type)
                {
                    if (_type == EdgeType.Back)
                    {
                        if (_from != null)
                            _from.BackOutCount--;
                        if (_to != null)
                            _to.BackInCount--;
                    }
                    _type = value;
                    if (_type == EdgeType.Back)
                    {
                        if (_from != null)
                            _from.BackOutCount++;
                        if (_to != null)
                            _to.BackInCount++;
                    }
                }
            }
        }
        private EdgeType _type;

        public bool IsBack
        {
            get { return _type == EdgeType.Back; }
        }

    	public int Label { get; set; }

    	public void Remove()
        {
            _from.RemoveOut(this);
            _to.RemoveIn(this);
        }

        public override string ToString()
        {
            return string.Format("{0}({1} -> {2})", _type, _from, _to);
        }
    }

    [Flags]
    internal enum EdgeFlags
    {
        None = 0,
        Visited = 1,
    }

    internal enum EdgeType
    {
        Unknown,
        Tree,
        Back,
        Forward,
        Cross,
        Goto
    }
}