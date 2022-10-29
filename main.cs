using System;
using System.Text;
using System.Threading;
using System.IO;

class Program
{
    struct Operacija
    {
        public string datoteka;
        public int brSifre;
        public bool sifrovanje; //glup naziv
        public string kljuc;
    }
    static bool sifra1(Operacija operacija)
    {
        StreamWriter sw;
        for (int i = 0; i < operacija.kljuc.Length; i++)
        {
            if (!char.IsLetterOrDigit(operacija.kljuc[i]))
            {
                return false;
            }
        }
        StreamReader sb = new StreamReader(operacija.datoteka);
        if (operacija.sifrovanje)
        {
            sw = new StreamWriter("sifrovana_" + operacija.datoteka);
            while (sb.EndOfStream)
            {
                char[] temp = sb.ReadLine().ToCharArray();

                for (int i = 0; i < temp.Length; i++)
                {
                    temp[i] += operacija.kljuc[i % operacija.kljuc.Length];
                }
                sw.WriteLine(temp);
            }
            sw.Close();
            sb.Close();
        }
        else
        {
            sw = new StreamWriter("desifrovana_" + operacija.datoteka);
            while (sb.EndOfStream)
            {
                char[] temp = sb.ReadLine().ToCharArray();
                for (int i = 0; i < temp.Length; i++)
                {
                    temp[i] -= operacija.kljuc[i % operacija.kljuc.Length];
                }
                sw.WriteLine(temp);
            }
            sw.Close();
            sb.Close();
        }
        return true;//ako sve prodje vraca tacno
    }
    static void IspisRadaPrograma(string greska)
    {
        Console.WriteLine("program radi tako sto:");
        Console.WriteLine("bla bla bla");
        Console.WriteLine(greska);
    }
    static void Loading(string operacija, int brzina)
    {
        Console.Write("{0}...", operacija);
        StringBuilder sb = new StringBuilder("..", 3);
        Console.SetCursorPosition(0, Console.CursorTop + 1);
        for (int i = 0; i < 2; i++)
        {
            Thread.Sleep(brzina);
            Console.SetCursorPosition(operacija.Length, Console.CursorTop - 1);
            Console.Write("{0,-3}", sb);
            Console.SetCursorPosition(0, Console.CursorTop + 1);
            if (sb.ToString() != ".") sb = sb.Remove(sb.Length - 1, sb.Length - 1);
            else sb = new StringBuilder("", 3);
        }
        for (int i = 0; i < 4; i++)
        {
            Thread.Sleep(brzina);
            Console.SetCursorPosition(operacija.Length, Console.CursorTop - 1);
            Console.Write("{0,-3}", sb);
            Console.SetCursorPosition(0, Console.CursorTop + 1);
            sb.Append('.');
        }
        Console.WriteLine("uspesno izvrseno {0} ", operacija);
    }
    static void Main(string[] args)
    {
        Operacija[] operacije = new Operacija[2];//umesto 2 treba da stoji args.Length/4 ali jos nemam argumente, i ovo treba tek kasnije da se radi kada se argumenti provere

        //pre ovoga treba da se urade sve operacije nad fajlovima i provere

        for (int i = 0; i < operacije.Length; i++)
        {
            if (operacije[i].sifrovanje) Loading("Sifrovanje datoteke " + operacije[i].datoteka, 250);
            else Loading("Desifrovanje datoteke " + operacije[i].datoteka, 250);
        }

    }
}