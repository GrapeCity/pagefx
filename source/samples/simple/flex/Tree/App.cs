using mx.core;
using mx.controls;
using mx.controls.treeClasses;

class Node
{
    public Node(string label)
    {
        this.label = label;
    }

    public Avm.String label;
    public Avm.Array children = new Avm.Array();
    
    public void AddChild(Node kid)
    {
        children.push(kid);
    }
}

class App : Application
{
    Tree tree;

    public App()
    {
    	tree = new Tree();
    	tree.setStyle("left", 10);
    	tree.setStyle("right", 10);
    	tree.setStyle("top", 10);
    	tree.setStyle("bottom", 10);

        Node root = new Node("Root");
        root.AddChild(new Node("1"));
        root.AddChild(new Node("2"));
        root.AddChild(new Node("3"));

        //tree.dataDescriptor = new DefaultDataDescriptor();
        tree.dataProvider = root;

    	addChild(tree);
    }
}