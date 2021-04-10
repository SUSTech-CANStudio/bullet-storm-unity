namespace CANStudio.BulletStorm.Core
{
    public interface IBulletAction
    {
        void UpdateBullet(ref BulletParams @params, float deltaTime);
    }
}