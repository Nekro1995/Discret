using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class DekstraAlgorim
{

    public Point[] points { get; private set; }
    public Rebro[] rebra { get; private set; }
    public Point BeginPoint { get; private set; }

    public DekstraAlgorim(Point[] pointsOfgrath, Rebro[] rebraOfgrath)
    {
        points = pointsOfgrath;
        rebra = rebraOfgrath;
    }
    public void AlgoritmRun(Point beginp)
    {
        if (this.points.Count() == 0 || this.rebra.Count() == 0)
        {
            throw new DekstraException("Массив вершин или ребер не задан!");
        }
        else
        {
            BeginPoint = beginp;
            OneStep(beginp);
            foreach (Point point in points)
            {
                Point anotherP = GetAnotherUncheckedPoint();
                if (anotherP != null)
                {
                    OneStep(anotherP);
                }
                else
                {
                    break;
                }

            }
        }

    }
    public void OneStep(Point beginpoint)
    {
        foreach (Point nextp in Pred(beginpoint))
        {
            if (nextp.IsChecked == false)//не отмечена
            {
                float newmetka = beginpoint.ValueMetka + GetMyRebro(nextp, beginpoint).Weight;
                if (nextp.ValueMetka > newmetka)
                {
                    nextp.ValueMetka = newmetka;
                    nextp.predPoint = beginpoint;
                }
                else
                {

                }
            }
        }
        beginpoint.IsChecked = true;//вычеркиваем
    }
    private IEnumerable<Point> Pred(Point currpoint)
    {
        IEnumerable<Point> firstpoints = from ff in rebra where ff.FirstPoint == currpoint select ff.SecondPoint;
        IEnumerable<Point> secondpoints = from sp in rebra where sp.SecondPoint == currpoint select sp.FirstPoint;
        IEnumerable<Point> totalpoints = firstpoints.Concat<Point>(secondpoints);
        return totalpoints;
    }
    private Rebro GetMyRebro(Point a, Point b)
    {//ищем ребро по 2 точкам
        IEnumerable<Rebro> myr = from reb in rebra where (reb.FirstPoint == a & reb.SecondPoint == b) || (reb.SecondPoint == a & reb.FirstPoint == b) select reb;
        if (myr.Count() > 1 || myr.Count() == 0)
        {
            throw new DekstraException("Не найдено ребро между соседями!");
        }
        else
        {
            return myr.First();
        }
    }
    private Point GetAnotherUncheckedPoint()
    {
        IEnumerable<Point> pointsuncheck = from p in points where p.IsChecked == false select p;
        if (pointsuncheck.Count() != 0)
        {
            float minVal = pointsuncheck.First().ValueMetka;
            Point minPoint = pointsuncheck.First();
            foreach (Point p in pointsuncheck)
            {
                if (p.ValueMetka < minVal)
                {
                    minVal = p.ValueMetka;
                    minPoint = p;
                }
            }
            return minPoint;
        }
        else
        {
            return null;
        }
    }

    public List<Point> MinPath1(Point end)
    {
        List<Point> listOfpoints = new List<Point>();
        Point tempp = new Point();
        tempp = end;
        while (tempp != this.BeginPoint)
        {
            listOfpoints.Add(tempp);
            tempp = tempp.predPoint;
        }

        return listOfpoints;
    }
}

class Rebro
{
    public Point FirstPoint { get; private set; }
    public Point SecondPoint { get; private set; }
    public float Weight { get; private set; }

    public Rebro(Point first, Point second, float valueOfWeight)
    {
        FirstPoint = first;
        SecondPoint = second;
        Weight = valueOfWeight;
    }

}
class Point
{
    public float ValueMetka { get; set; }
    public string Name { get; private set; }
    public bool IsChecked { get; set; }
    public Point predPoint { get; set; }
    public object SomeObj { get; set; }
    public Point(int value, bool ischecked)
    {
        ValueMetka = value;
        IsChecked = ischecked;
        predPoint = new Point();
    }
    public Point(int value, bool ischecked, string name)
    {
        ValueMetka = value;
        IsChecked = ischecked;
        Name = name;
        predPoint = new Point();
    }
    public Point()
    {
    }
}
static class PrintGrath
{
    public static List<string> PrintAllPoints(DekstraAlgorim da)
    {
        List<string> retListOfPoints = new List<string>();
        foreach (Point p in da.points)
        {
            retListOfPoints.Add(string.Format("point name={0}, point value={1}, predok={2}", p.Name, p.ValueMetka, p.predPoint.Name ?? "нет предка"));
        }
        return retListOfPoints;
    }
    public static List<string> PrintAllMinPaths(DekstraAlgorim da)
    {
        List<string> retListOfPointsAndPaths = new List<string>();
        foreach (Point p in da.points)
        {

            if (p != da.BeginPoint)
            {
                string s = string.Empty;
                foreach (Point p1 in da.MinPath1(p))
                {
                    s += string.Format("{0} ", p1.Name);
                }
                retListOfPointsAndPaths.Add(string.Format("Point ={0},MinPath from {1} = {2}", p.Name, da.BeginPoint.Name, s));
            }

        }
        return retListOfPointsAndPaths;
    }
}

class DekstraException : ApplicationException
{
    public DekstraException(string message)
        : base(message)
    {
    }
}

class Program
{
    static void Main(string[] args)
    {
        Point[] v = new Point[6];
        v[0] = new Point(0, false, "F");
        v[1] = new Point(9999, false, "A");
        v[2] = new Point(9999, false, "B");
        v[3] = new Point(9999, false, "C");
        v[4] = new Point(9999, false, "D");
        v[5] = new Point(9999, false, "E");
        Rebro[] rebras = new Rebro[10];
        rebras[0] = new Rebro(v[0], v[2], 8);
        rebras[1] = new Rebro(v[0], v[3], 4);//FC
        rebras[2] = new Rebro(v[0], v[1], 9);//FA
        rebras[3] = new Rebro(v[2], v[3], 7);//bc
        rebras[4] = new Rebro(v[2], v[5], 5);//be
        rebras[5] = new Rebro(v[3], v[5], 5);//ce
        rebras[6] = new Rebro(v[1], v[5], 6);//ae
        rebras[7] = new Rebro(v[1], v[4], 5);//ad
        rebras[8] = new Rebro(v[3], v[4], 4);//cd
        rebras[9] = new Rebro(v[2], v[4], 7);//bd
        DekstraAlgorim da = new DekstraAlgorim(v, rebras);
        da.AlgoritmRun(v[0]);
        List<string> b = PrintGrath.PrintAllMinPaths(da);
        for (int i = 0; i < b.Count; i++)
            Console.WriteLine(b[i]);
        Console.ReadKey(true);
    }
}

