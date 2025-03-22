using System;
using System.Drawing;
using System.Formats.Asn1;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class KuncirRamBOT : Bot
{
  static void Main(string[] args)
  {
    new KuncirRamBOT().Start();
  }
  int arahSenjata = 1;
  bool scanning = false;
  bool maju;
  bool kenceng;

  KuncirRamBOT() : base(BotInfo.FromFile("KuncirRamBOT.json")) { }

  public override void Run()
  {
    BodyColor = Color.White;
    TurretColor = Color.Black;
    RadarColor = Color.Red;
    BulletColor = Color.Black;
    ScanColor = Color.Red;

    while (IsRunning)
    {
      AdjustGunForBodyTurn = false;
      maju = true;
      kenceng = false;
      GunTurnRate = 15;
      if (Energy > 10)
      {
        MaxSpeed = 6;
        SetForward(4000);
        SetTurnRight(45);
        SetTurnLeft(45);
      }
      else
      {
        MaxSpeed = 8;
        SetTurnLeft(10_000);
        SetForward(10_000);
        kenceng = true;
      }
      SetTurnGunRight(360);
      Go();
    }
  }

  public override void OnScannedBot(ScannedBotEvent e)
  {
    base.OnScannedBot(e);
    scanning = true;
    arahSenjata = -arahSenjata;

    AdjustGunForBodyTurn = true;
    double distance = DistanceTo(e.X, e.Y);
    SetTurnGunTowards(e.X, e.Y);
    if (Energy > 10)
    {
      SetForward(4000);
      if (distance < 75)
      {
        Fire(3);
      }
      else if (distance < 100 && distance > 75)
      {
        Fire(2.8);
      }
      else if (distance < 150 && distance > 100)
      {
        SetTurnRight(45);
        Fire(2.5);
      }
      else if (distance < 250 && distance > 45)
      {
        SetTurnLeft(45);
        Fire(2.3);
      }
      else if (distance < 300 && distance > 250)
      {
        SetTurnRight(45);
        Fire(2);
      }
      else
      {
        double jarakbaru = distance % 3;
        if (jarakbaru == 0)
        {
          SetTurnRight(45);
          SetForward(4000);
        }
        else if (jarakbaru == 1)
        {
          SetTurnLeft(45);
          SetForward(4000);
        }
        else
        {
          SetBack(4000);
        }
        Fire(1.5);
      }
    }
    else
    {
      MaxSpeed = 8;
      SetTurnLeft(10_000);
      SetForward(10_000);
      kenceng = true;
    }
    SetTurnGunRight(360 * arahSenjata);
    Go();
  }

  public override void OnHitBot(HitBotEvent e)
  {
    base.OnHitBot(e);
    SetTurnGunTowards(e.X, e.Y);
    arahSenjata = -arahSenjata;
    SetTurnGunRight(360 * arahSenjata);
    if (e.IsRammed)
    {
      MaxSpeed = 8;
      SetForward(300);
      SetTurnLeft(115);
      SetFire(3);
      kenceng = true;
    }
    else
    {
      MaxSpeed = 8;
      SetBack(100);
      SetTurnRight(50);
      kenceng = true;
    }
    Go();
  }

  public override void OnHitWall(HitWallEvent e)
  {
    base.OnHitWall(e);
    if (maju)
    {
      SetBack(4000);
      maju = false;
    }
    else
    {
      SetForward(4000);
      maju = true;
    }
    SetTurnLeft(115);
    if (!scanning)
    {
      SetTurnGunRight(360);
    }
    Go();
  }

  private void SetTurnGunTowards(double targetX, double targetY)
  {
    double bearing = BearingTo(targetX, targetY);
    double absoluteBearing = Direction + bearing;
    double gunTurn = absoluteBearing - GunDirection;
    SetTurnGunRight(NormalizeBearing(gunTurn));
  }

  private double NormalizeBearing(double angle)
  {
    angle = (angle + 180) % 360 - 180;
    return angle;
  }
}
