using Core.Utils.Math;

namespace Core.Game.World.Actor
{
    public class FollowTargetComponent
    {
        public event Action<ICharacter> OnTargetReached;
        private readonly MovementComponent _movement;

        public ICharacter FollowTarget;
        private int _followDistance;

        private float _currentDistance;

        public FollowTargetComponent(MovementComponent movement)
        {
            _movement = movement;
        }

        public void Update(float dt)
        {
            var dist = Vec2.Distance(_movement.Position, FollowTarget.Origin);
            if (dist - _followDistance >= 0)
            {
                _movement.Move(FollowTarget.Origin, FollowTarget.OriginZ);
                _movement.Update(dt);
            }
            else
            {
              /*  var dir = Vec2.Direction(_movement.Position, FollowTarget.Origin);
                var compStep = dir * (_followDistance - 10);
                var finalPos = FollowTarget.Origin - compStep;
                _movement.Position = finalPos;
                _movement.Stop();
                OnTargetReached?.Invoke(FollowTarget);*/
            }
        }

        public void BeakState()
        {
            OnTargetReached?.Invoke(FollowTarget);
        }

        public void SetTarget(ICharacter target, int distance)
        {
            FollowTarget = target;
            _followDistance = distance;
            _currentDistance = Vec2.Distance(_movement.Position, target.Origin);
        }
    }
}
