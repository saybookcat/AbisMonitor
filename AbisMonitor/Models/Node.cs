using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;

namespace AbisMonitor.UI.Models
{
    public class Node : ObservableObject
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                this.RaisePropertyChanged(() => Name);
            }
        }

        private NodeType _nodeType;

        public NodeType NodeType
        {
            get { return _nodeType; }
            set
            {
                _nodeType = value;
                this.RaisePropertyChanged(() => NodeType);
            }
        }

        private List<Node> _children;

        public List<Node> Children
        {
            get { return _children; }
            set
            {
                _children = value;
                this.RaisePropertyChanged(() => Children);
            }
        }
    }

    public enum NodeType
    {
        Root,//根节点
        Device,//设备
        Port,//环

        
    }
}
