using Godot;

public partial class LevelManager : Node
{
    [Export] TileMap tilemap;
    [Export] Ball ball;

	[Export] public int Lives;

    public override void _Ready()
    {
        ball.OnHit += OnBallHit;
    }

    public override void _Process(double delta)
    {
		
    }

    void OnBallHit(string objectHit)
    {
        if (objectHit == "FLOOR")
        {
            Lives--;
        }
    }
}
