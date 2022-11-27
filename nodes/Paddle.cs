using Godot;

public partial class Paddle : CharacterBody2D
{
    [Export] float speed;
    [Export] float acceleration;
    [Export] float bounce;

    public override void _Process(double delta)
    {
        var axis = Input.GetAxis("move_left", "move_right");
		var speed_vector = new Vector2(axis * speed, 0);
		
        Velocity = Velocity.Lerp(speed_vector, (1 / acceleration) * (float)delta);

        HandleCollision(MoveAndCollide(Velocity));
    }

    private void HandleCollision(KinematicCollision2D col)
    {
        if (col == null) return;
        var obj = col.GetCollider() as Node2D;

        if (obj.IsInGroup("walls"))
        {
            Velocity = Velocity.Bounce(col.GetNormal()) * bounce;
        }
    }
}
