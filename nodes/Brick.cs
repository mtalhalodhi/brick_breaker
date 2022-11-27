using Godot;

public partial class Brick : Node2D
{
	[Export] AnimationPlayer AnimationPlayer;
	[Export] public int Lives = 1;

	public void TakeHit() {
		Lives--;
		AnimationPlayer.Play(Lives > 0 ? "TAKE_HIT" : "DIE");
	}
}
