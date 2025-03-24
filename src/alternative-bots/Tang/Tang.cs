using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class Tang : Bot
{   
    bool close = false;
    double closest;//jarak dengan bot musuh
    double t_x;//absis bot musuh yang terpilih
    double t_y;//ordinat bot musuh yang terpilih
    //bot yang menyerang bot musuh terdekat setiap turn nya
    static void Main(string[] args)
    {
        new Tang().Start();
    }

    Tang() : base(BotInfo.FromFile("Tang.json")) { }

    public override void Run()
    {
        AdjustRadarForGunTurn = true;
        BodyColor = Color.Gray;
        while (IsRunning)
        {
            close = false;
            closest = 10000000;//Mereset jarak dengan bot musuh
            Forward(200);
            TurnRight(90);
            SetTurnRadarRight(90);
            TurnLeft(90);
        }
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        double distance = DistanceTo(e.X, e.Y);//Jarak dengan bot musuh
        if (distance < closest){
            close = true;
            closest = distance;
            t_x = e.X;
            t_y = e.Y;            
        }
        else{
            close = false;
        }
        // double absoluteBearing = Math.Atan2(y - Y, x - X) * (180 / Math.PI);
        // double gunTurnAngle = NormalizeBearing(absoluteBearing - GunDirection);
        // double angle = GunBearingTo(x, y);

        if (close){//Menyerang jika Bot yang di scan adalah bot terdekat
            double firepower = (50/distance) * 10;
            TurnGunLeft(GunBearingTo(t_x, t_y));
            Fire(firepower);
        }

        if (distance < 50){//Menghindar dari bot musuh
            SetTurnRight(90);
            SetForward(200);
        }

        else{
            SetForward(200);
        }

    }

    public override void OnHitBot(HitBotEvent e)
    {
        if (e.IsRammed){
            SetTurnRight(90);
            SetBack(100);
        }
        else{
            SetTurnRight(90 + BearingTo(e.X, e.Y));
            SetBack(200);
            SetTurnRight(45);
            SetForward(200);

        }
    }

    public override void OnHitWall(HitWallEvent e)
    {
        TurnRight(90);
        SetForward(100);
    }


    public override void OnHitByBullet(HitByBulletEvent e)
    {
        //Menhindari bot musuh dari arah datangnya peluru.
        var bearing = CalcBearing(e.Bullet.Direction);
        TurnLeft(90 - bearing);
    }
   

}



