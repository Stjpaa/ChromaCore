using Godot;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

public partial class Rope : Node
{
    [Export]
    public PackedScene ropePiece;

    public float pieceLength = 16f;
    public float ropeCloseTolerance = 4f;

    public RopePiece[] ropeParts;

    public RigidBody2D ropeStartPiece;
    public RigidBody2D ropeEndPiece;

    public override void _Ready()
    {
        ropeStartPiece = GetNode<RigidBody2D>("RopeStartPiece");
        ropeEndPiece = GetNode<RigidBody2D>("RopeEndPiece");
    }

    public void SpawnRope(Vector2 startPos, Vector2 endPos)
    {
        ropeStartPiece.GlobalPosition = startPos;
        ropeEndPiece.GlobalPosition = endPos;

        startPos = ropeStartPiece.GetNode<Joint2D>("CollisionShape2D/PinJoint2D").GlobalPosition;
        endPos = ropeEndPiece.GetNode<Joint2D>("CollisionShape2D/PinJoint2D").GlobalPosition;

        float distance = startPos.DistanceTo(endPos);
        int piecesAmount = Mathf.RoundToInt(distance / pieceLength); 
        float spawnAngle = (endPos - startPos).Angle() - (Mathf.Pi/2);

        CreateRope(piecesAmount, endPos, spawnAngle);        
    }

    private void CreateRope(int piecesAmount, Vector2 endPos, float spawnAngle)
    {
        ropeParts = new RopePiece[piecesAmount];
        RopePiece lastCreatedRopePiece = null;

        for (int i = 0; i < piecesAmount; i++)
        {
            if(lastCreatedRopePiece == null) 
            {
                lastCreatedRopePiece = AddPiece(ropeStartPiece, i, spawnAngle);
            }
            else
            {
                lastCreatedRopePiece = AddPiece(lastCreatedRopePiece, i, spawnAngle);
            }
            
            lastCreatedRopePiece.Name = "RopePiece_" + i;
            ropeParts[i] = lastCreatedRopePiece;

            var jointPos = lastCreatedRopePiece.GetNode<Joint2D>("CollisionShape2D/PinJoint2D").GlobalPosition;
            if(jointPos.DistanceTo(endPos) < ropeCloseTolerance)
            {
                break;
            }
        }

        ropeEndPiece.GetNode<PinJoint2D>("CollisionShape2D/PinJoint2D").NodeA = ropeEndPiece.GetPath();
        ropeEndPiece.GetNode<PinJoint2D>("CollisionShape2D/PinJoint2D").NodeB = lastCreatedRopePiece.GetPath();

        var ss = ropeEndPiece.GetNode<PinJoint2D>("CollisionShape2D/PinJoint2D");
    }

    private RopePiece AddPiece(Node2D parent, int id, float spawnAngle)
    {
        PinJoint2D joint = parent.GetNode<PinJoint2D>("CollisionShape2D/PinJoint2D");
        RopePiece piece = ropePiece.Instantiate() as RopePiece;

        piece.GlobalPosition = joint.GlobalPosition;
        piece.Rotation = spawnAngle;
        piece.parent = parent;
        piece.id = id;

        AddChild(piece);
        joint.NodeA = parent.GetPath();
        joint.NodeB = piece.GetPath();

        return piece;
    }
}
