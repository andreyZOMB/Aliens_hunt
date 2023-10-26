using System.Collections.Generic;

public class CollideLose : CollideScript
{
    public override void Collide(List<ObjectControl> other)
    {
        base.Collide(other);
        controller.Invoke(nameof(controller.Lose), animationTime);
    }
}
