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
            StreamReader sr;
            int[] kljuc = new int[operacija.kljuc.Length];
            if (File.Exists(operacija.datoteka))
            {
                sr = new StreamReader(operacija.datoteka);
            }
            else
            {
                return false;
            }
            for (int i = 0; i < operacija.kljuc.Length; i++)
            {
                //kljuc moze da bude samo slovo ili broj za ovo sifrovanje
                //u kodu ispod: ascii kod od slova pretvaram u broj njihovog mesta u alfabetu, a stavljam da cifre pomere za jacinu cifre a ne njihov ascii kod
                if (char.IsLetter(operacija.kljuc[i]))
                {
                    kljuc[i] = char.ToUpper(operacija.kljuc[i])-64;//A ima ascii kod od 65
                }
                else if (char.IsDigit(operacija.kljuc[i]))
                {
                    kljuc[i] = operacija.kljuc[i]-48;//0 ima ascii kod od 48
                }
                else
                {
                    return false;
                }
            }
            //Sifrovanje
            if (operacija.sifrovanje)
            {
                sw = new StreamWriter("sifrovana_" + operacija.datoteka);
                while (!sr.EndOfStream)
                {
                    char[] temp = sr.ReadLine().ToCharArray();

                    for (int i = 0; i < temp.Length; i++)
                    {
                        temp[i] += Convert.ToChar(kljuc[i % kljuc.Length]);//za sifrovanje povecam ascii kod svakog karaktera za njegovo odgovarajuce mesto u kljucu
                    }
                    sw.WriteLine(temp);
                }
                sw.Close();
                sr.Close();
                Console.WriteLine("Uspesno sifrovano.");
            }
            //Desifrovanje
            else
            {
                sw = new StreamWriter("desifrovana_" + operacija.datoteka);
                while (!sr.EndOfStream)
                {
                    char[] temp = sr.ReadLine().ToCharArray();
                    for (int i = 0; i < temp.Length; i++)
                    {
                        temp[i] -= Convert.ToChar(kljuc[i % kljuc.Length]);//za sifrovanje smanjim ascii kod svakog karaktera za njegovo odgovarajuce mesto u kljucu
                    }
                    sw.WriteLine(temp);
                }
                sw.Close();
                sr.Close();
                Console.WriteLine("Uspesno desifrovano.");
            }
            return true;//ako sve prodje vraca tacno
        }
    static void IspisRadaPrograma(string greska)
    {
        Console.WriteLine("program radi tako sto:");
        Console.WriteLine("bla bla bla");
        Console.WriteLine(greska);
    }
    
    static void Main(string[] args)
    {
        Operacija[] operacije = new Operacija[2];//umesto 2 treba da stoji args.Length/4 ali jos nemam argumente, i ovo treba tek kasnije da se radi kada se argumenti provere
    }
}