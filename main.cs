//kljuc bude mesto u broju pi
//prvo slovo a -> d ako je ta cifra 3
//Math.Pi
//76 -> 86 + duzina kljuca 96 ->106 + duzina kljuca

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
            //u kodu ispod: ascii kod od slova pretvaramo u broj njihovog mesta u alfabetu, a stavljam da cifre pomere za jacinu cifre a ne njihov ascii kod
            if (char.IsLetter(operacija.kljuc[i]))
            {
                kljuc[i] = char.ToUpper(operacija.kljuc[i]) - 64;//A ima ascii kod od 65
            }
            else if (char.IsDigit(operacija.kljuc[i]))
            {
                kljuc[i] = operacija.kljuc[i] - 48;//0 ima ascii kod od 48
                                                   //isti kljuc je abc i 123
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

    static bool Sifra2(Operacija operacija)
    {
        // ako ne postoji pi fajl onda greska
        // sabira se kljuc i krece se sifrovanje od cifre pi koja je jednaka zbiru kljuca.
        // kada se sifruje broj karaktera ekvivalentat duzinu kljuca, preskace se na cifru pi za jos jednu duzinu kljuca pa se tako ponavlja.
        // duzina kljuca je maks 10
        // kljuc su slova i brojevi (za slova se sabira ascii kod)             
        // datoteka cifara pi, u string, pa u char array, pa po potrebi parsiras
        StreamReader sr; StreamWriter sw;
        if (File.Exists(operacija.datoteka))
            sr = new StreamReader(operacija.datoteka);
        else
            return false; // greska

        int[] kljuc = new int[operacija.kljuc.Length];
        // kljuc u nizu
      if(kljuc.Length > 10)
        return false; // duzina kljuca ne treba biti veca od 10
        for (int i = 0; i < operacija.kljuc.Length; i++)
        {
            //kljuc moze da bude samo slovo ili broj
            //u kodu ispod: ascii kod ostaje isti, a stavljamo da cifre pomere za jacinu cifre a ne njihov ascii kod
            if (char.IsLetter(operacija.kljuc[i]))
                kljuc[i] = char.ToUpper(operacija.kljuc[i]);
            else if (char.IsDigit(operacija.kljuc[i]))
                kljuc[i] = operacija.kljuc[i] - 48;//0 ima ascii kod od 48
            else
                return false;
        }

        int kljucZbir = 0; // ova promenljiva sluzi za odredjivanje decimale broja pi od koje sifrovanje krece
        foreach (int i in kljuc)
            kljucZbir += i;
        // Datoteka sa brojem pi
        StreamReader pi;
        if (File.Exists("Cifre.Pi.txt"))
            pi = new StreamReader("Cifre.Pi.txt");
        else
            return true;
        char[] piNiz = pi.ReadLine().ToCharArray(); // niz cifara broja pi
        int brojacPi; //broji u odnosu na kljucZbir
        //Sifrovanje
        if(operacija.sifrovanje)
        {
            brojacPi = 0;
            while (!sr.EndOfStream)
            {
                sw = new StreamWriter("sifrovana_" + operacija.datoteka);
                char[] temp = sr.ReadLine().ToCharArray();
                for (int i = 0; i < temp.Length; i++) // sabira se kljuc i krece se sifrovanje od cifre pi koja je jednaka zbiru kljuca.
                {
                    if (brojacPi == kljuc.Length - 1)
                    {
                        brojacPi = 0; // kada se sifruje broj karaktera ekvivalentat duzinu kljuca, preskace se na cifru pi za jos jednu duzinu kljuca
                        kljucZbir += kljuc.Length;
                    }
                    temp[i] += Convert.ToChar(piNiz[kljucZbir + brojacPi]);
                }
                sw.WriteLine(temp);
                Console.WriteLine("Uspesno sifrovano.");
                sw.Close();
                sr.Close();
            }
        }
        //Desifrovanje
        else
        {
            brojacPi = 0;
            sw = new StreamWriter("desifrovana_" + operacija.datoteka);
            while (!sr.EndOfStream)
            {
                char[] temp = sr.ReadLine().ToCharArray();
                for (int i = 0; i < temp.Length; i++) // sabira se kljuc i krece se desifrovanje od cifre pi koja je jednaka zbiru kljuca.
                {
                    if (brojacPi == kljuc.Length - 1)
                    {
                        brojacPi = 0; // kada se desifruje broj karaktera ekvivalentat duzinu kljuca, preskace se na cifru pi za jos jednu duzinu kljuca
                        kljucZbir += kljuc.Length;
                    }
                    temp[i] -= Convert.ToChar(piNiz[kljucZbir + brojacPi]);
                }
                sw.WriteLine(temp);
                Console.WriteLine("Uspesno desifrovano");
                sw.Close();
                sr.Close();
            }
        }
        return true;
    }

    static void IspisRadaPrograma(string greska)//Po pozivu uneti koja se greska desila
    {
        Console.WriteLine("program radi tako sto:");
        Console.WriteLine("bla bla bla");//ovo moze i sa datotekom
        Console.WriteLine(greska);
    }

    static void Main(string[] args)
    {
        Operacija[] operacije = new Operacija[2];//umesto 2 treba da stoji args.Length/4 ali jos nemam argumente, i ovo treba tek kasnije da se radi kada se argumenti provere
    }
}