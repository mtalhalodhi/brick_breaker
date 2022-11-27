using Godot;
using System;

public partial class Ball : CharacterBody2D
{
    public enum BallState { AT_LAUNCH, BOUNCING }

    public event Action<String> OnHit;

    [Export] public BallState State = BallState.AT_LAUNCH;
    [Export] public Node2D Launcher;
    [Export] float speed;

    Vector2 move;

    public override void _Ready()
    {
        move = (Vector2.Up * speed);
    }

    public override void _Process(double delta)
    {
        if (State == BallState.AT_LAUNCH)
        {
            ProcessAtLaunchState();
        }
        else if (State == BallState.BOUNCING)
        {
            ProcessBouncingState((float)delta);
        }
    }

    private void ProcessAtLaunchState()
    {
        GlobalPosition = Launcher?.GlobalPosition ?? Vector2.Zero;

        if (Input.IsActionJustPressed("launch_ball")) State = BallState.BOUNCING;
    }

    private void ProcessBouncingState(float delta)
    {
        var col = MoveAndCollide(move * delta);
        if (col == null) return;

        var obj = col.GetCollider() as Node2D;

        if (obj is Paddle)
        {
            var factor = (GlobalPosition - obj.GlobalPosition).x / 20;
            var angle = Mathf.LerpAngle(0, Mathf.Pi / 2.5f, factor);
            move = (Vector2.Up * speed).Rotated(angle);
            OnHit?.Invoke("PADDLE");
        }
        else if (obj is Brick)
        {
            move = move.Bounce(col.GetNormal());
            ((Brick)obj).TakeHit();
            OnHit?.Invoke("BRICK");
        }
        else if (obj.IsInGroup("floor"))
        {
            move = (Vector2.Up * speed);
            State = BallState.AT_LAUNCH;
            OnHit?.Invoke("FLOOR");
        }
        else
        {
            move = move.Bounce(col.GetNormal());
            OnHit?.Invoke("WALL");
        }
    }
}
