using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class Hytam : Bot
{
  static void Main(string[] args)
  {
    new Hytam().Start();
  }

  Hytam() : base(BotInfo.FromFile("Hytam.json")) { }

  public override void Run()
  {
    BodyColor = Color.FromArgb(0x00, 0x00, 0x00);   // Black
    TurretColor = Color.FromArgb(0x00, 0x00, 0x00); // Black
    RadarColor = Color.FromArgb(0x00, 0x00, 0x00);  // Black
    BulletColor = Color.FromArgb(0x00, 0x00, 0x00); // Black
    ScanColor = Color.FromArgb(0x00, 0x00, 0x00);   // Black

    while (IsRunning)
    {
      SetTurnLeft(10_000);
      MaxSpeed = 5;
      Forward(10_000);
    }
  }

  public override void OnScannedBot(ScannedBotEvent e)
  {
    base.OnScannedBot(e);
    double distance = CalculateDistanceToEnemy(e.X, e.Y);
    Fire(distance < 200 ? 3 : 2);

    SetTurnGunTowards(e.X, e.Y);
  }

  public override void OnHitBot(HitBotEvent botHitBotEvent)
  {
    base.OnHitBot(botHitBotEvent);
    SetTurnGunTowards(botHitBotEvent.X, botHitBotEvent.Y);

    Forward(50);
  }

  public override void OnHitWall(HitWallEvent e)
  {
    base.OnHitWall(e);

    TurnLeft(115);
  }

  private double CalculateDistanceToEnemy(double targetX, double targetY)
  {
    double deltaX = targetX - X;
    double deltaY = targetY - Y;
    return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
  }

  private void SetTurnGunTowards(double targetX, double targetY)
  {
    double angleToTarget = Math.Atan2(targetY - Y, targetX - X) * (180 / Math.PI);
    double gunTurnAngle = angleToTarget - GunDirection;
    SetTurnGunRight(NormalizeBearing(gunTurnAngle));
  }

  private double NormalizeBearing(double angle)
  {
    angle = (angle + 180) % 360 - 180;
    return angle;
  }
}
