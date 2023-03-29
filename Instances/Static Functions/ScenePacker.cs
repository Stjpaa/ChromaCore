using Godot;


// used in the SaveSystem to convert current active Scenes into PackedScenes.
// PackedScenes retain some information about their Nodes, for example: Scene Structure (Node hierarchy)/ Node pos, rot, scale etc./ signals
// variables within Skripts are reset when Loading a PackedScene, for example: current state, animation, current gravity, ...
// Code Inspiration Source: https://www.youtube.com/watch?v=LWNjK28MMwU / https://github.com/nezvers/Godot_Public_Examples/tree/master/ScenePacking
public static class ScenePacker
{
    public static PackedScene CreatePackage(Node node)
    {
        SetOwner(node, node);   // ensures saving the current Scene Hierarchy

        PackedScene package = new PackedScene();

        Error error = package.Pack(node);
        if (error != Error.Ok)
        {
            GD.Print("Failed to pack scene: ", error);
            return null;
        }

        return package;
    }


    // when packing a Scene we need to insure that the owner of each child is set correctly.
    // without this a package could "lose" the current owner information, because a PackedScene doesnt save changes to the Owner which happened at runtime.
    // if we didnt save this Information before Packing, a Node that was moved would remember its old Owner, which would create Problems with signals and the scene structure.
    public static void SetOwner(Node node, Node owner)
    {
        foreach (Node child in node.GetChildren())  // needs to happen for all child nodes and their children (and so on) to ensure the same Scene Hierarchy on load
        {
            child.Owner = owner;
            SetOwner(child, owner);
        }
    }
}