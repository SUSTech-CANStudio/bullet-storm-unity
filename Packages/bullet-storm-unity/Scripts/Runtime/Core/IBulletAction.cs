namespace CANStudio.BulletStorm.Core
{
    public interface IBulletAction
    {
        void SetContext(BulletStormContext context);

        void UpdateBullet(ref BulletParams @params, float deltaTime);
    }
}