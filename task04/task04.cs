using System;
namespace task04{

public interface ISpaceship
{
    void MoveForward();      // Движение вперед
    void Rotate(int angle);  // Поворот на угол (градусы)
    void Fire();             // Выстрел ракетой
    int Speed { get; }       // Скорость корабля
    int FirePower { get; }   // Мощность выстрела
}

public class Cruiser: ISpaceship
{
    public int Speed => 50;
    public int FirePower => 100;
    int ang = 0;
    int x = 0;
    public void Fire()=>Console.WriteLine("Произведен выстрел!");
    public void Rotate(int angle)
        {
         ang += angle;
         Console.WriteLine($"Крейсер двигается под углом в {ang} градусов");   
        }
    public void MoveForward()
        {
            x+=Speed;
            Console.WriteLine($"Крейсер прилетел в точку {x}");
        }
}

public class Fighter: ISpaceship
{
    public int Speed => 100;
    public int FirePower => 50;
    int ang = 0;
    int x = 0;
    public void Fire()=>Console.WriteLine("Произведен выстрел!");
    public void Rotate(int angle)
        {
         ang += angle;
         Console.WriteLine($"Истребитель двигается под углом в {ang} градусов");   
        }
    public void MoveForward()
        {
            x+=Speed;
            Console.WriteLine($"Истребитель прилетел в точку {x}");
        }
}
}