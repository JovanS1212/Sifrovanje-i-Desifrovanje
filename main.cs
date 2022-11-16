

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
        sr = new StreamReader(operacija.datoteka);
        for (int i = 0; i < operacija.kljuc.Length; i++)
        {
            //kljuc moze da bude samo slovo ili broj za ovo sifrovanje
            //u kodu ispod: ascii kod od slova pretvaram u broj njihovog mesta u alfabetu, a stavljam da cifre pomere za jacinu cifre a ne njihov ascii kod
            if (char.IsLetter(operacija.kljuc[i]))
            {
                kljuc[i] = char.ToUpper(operacija.kljuc[i]) - 64;//A ima ascii kod od 65
            }
            else if (char.IsDigit(operacija.kljuc[i]))
            {
                kljuc[i] = operacija.kljuc[i] - 48;//0 ima ascii kod od 48
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
                    if (temp[i] + kljuc[i % kljuc.Length] < char.MaxValue) temp[i] = Convert.ToChar(temp[i] + kljuc[i % kljuc.Length]);//za sifrovanje povecam ascii kod svakog karaktera za njegovo odgovarajuce mesto u kljucu
                    else temp[i] = Convert.ToChar(temp[i] + kljuc[i % kljuc.Length] - char.MaxValue + 32);//na 32 mestu je prvi karakter koji nije neka "komanda"
                }
                sw.WriteLine(temp);
            }
            sw.Close();
            sr.Close();
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
                    if (temp[i] - kljuc[i % kljuc.Length] > 0) temp[i] = Convert.ToChar(temp[i] - kljuc[i % kljuc.Length]);//za desifrovanje smanjim ascii kod svakog karaktera za njegovo odgovarajuce mesto u kljucu
                    else temp[i] = Convert.ToChar(temp[i] - kljuc[i % kljuc.Length] + char.MaxValue - 32);
                }
                sw.WriteLine(temp);
            }
            sw.Close();
            sr.Close();
        }
        return true;//ako sve prodje vraca tacno
    }
    static bool Sifra2(Operacija operacija)
    {
        // sabira se kljuc i krece se sifrovanje od cifre pi koja je jednaka zbiru kljuca.  
        StreamReader sr; StreamWriter sw;
        sr = new StreamReader(operacija.datoteka);
        int[] kljuc = new int[operacija.kljuc.Length];
        // kljuc u nizu
        if (kljuc.Length > 50)
            return false; // duzina kljuca ne treba biti veca od 50
        for (int i = 0; i < operacija.kljuc.Length; i++)
        {
            //kljuc moze da bude samo slovo ili broj
            //ascii kod ostaje isti, a stavljamo da cifre pomere za jacinu cifre a ne njihov ascii kod
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
            return false;// greska
        char[] piNiz = pi.ReadLine().ToCharArray(); // niz cifara broja pi
        pi.Close();
        int brojacPi = 0; //broji u odnosu na kljucZbir
        //Sifrovanje
        if (operacija.sifrovanje)
        {
            sw = new StreamWriter("sifrovana_" + operacija.datoteka);
            while (!sr.EndOfStream)
            {
                char[] temp = sr.ReadLine().ToCharArray();
                for (int i = 0; i < temp.Length; i++) //sifruje se red po red
                {
                    if (kljucZbir + brojacPi == 100000)
                    {
                        kljucZbir = 0;
                        brojacPi = 0;
                    }
                    if (temp[i] + Convert.ToChar(piNiz[kljucZbir + brojacPi] - 48) > char.MaxValue)
                        temp[i] -= Convert.ToChar(char.MaxValue - 32); //ako karakter izadje iz opsega ascii koda(127) umanjice se za ukupan broj ascii koda i krenuce od prvog karaktera (33)   
                    temp[i] += Convert.ToChar(piNiz[kljucZbir + brojacPi] - 48);
                    brojacPi++;
                }
                sw.WriteLine(temp);
            }
            sw.Close();
            sr.Close();
        }
        //Desifrovanje
        else
        {
            sw = new StreamWriter("desifrovana_" + operacija.datoteka);
            while (!sr.EndOfStream)
            {
                char[] temp = sr.ReadLine().ToCharArray();
                for (int i = 0; i < temp.Length; i++) //desifruje se red po red
                {
                    if (kljucZbir + brojacPi == 100000)
                    {
                        kljucZbir = 0;
                        brojacPi = 0;
                    }
                    if (temp[i] + Convert.ToChar(piNiz[kljucZbir + brojacPi] - 48) < char.MaxValue)
                        temp[i] += Convert.ToChar(char.MaxValue - 32); //ako karakter izadje iz opsega ascii koda(127) uvecace se za ukupan broj ascii koda i krenuce od prvog karaktera (33) 
                    temp[i] -= Convert.ToChar(piNiz[kljucZbir + brojacPi] - 48);
                    brojacPi++;
                }
                sw.WriteLine(temp);
            }
            sw.Close();
            sr.Close();
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
        Operacija[] operacije;
        string[] uspesnostSifre;
        if (args.Length % 4 != 0 || args.Length == 0)
        {
            IspisRadaPrograma("GRESKA! Broj unetih argumenata nije dobar");
            return;
        }
        else
        {
            operacije = new Operacija[args.Length / 4];
            uspesnostSifre = new string[args.Length / 4];
        }
        for (int i = 0; i < args.Length / 4; i++)
        {
            //Provera za naziv datoteke
            if (!args[i].EndsWith(".txt") || !File.Exists(args[i]))
            {
                uspesnostSifre[i] = $"GRESKA! {i + 1}. Fajl nije dobro unet";
                Console.WriteLine(uspesnostSifre[i]);
                continue;
            }
            operacije[i].datoteka = args[i];
            //Provera za nacin sifrovanja
            if (!int.TryParse(args[i + 1], out operacije[i].brSifre) || operacije[i].brSifre > 3 || operacije[i].brSifre < 1)
            {
                uspesnostSifre[i] = $"GRESKA! {i + 1}. Broj sifre nije dobro unet";
                Console.WriteLine(uspesnostSifre[i]);
                continue;
            }
            //Provera za sifrovanje/desifrovanje
            if (args[i + 2] == "a") operacije[i].sifrovanje = true;//ovo mozemo i da stavimo da bude s kao skraceno od sifrovanje
            else if (args[i + 2] == "b") operacije[i].sifrovanje = false;//ovo mozemo i da stavimo da bude d kao skraceno od desifrovanje
            else
            {
                uspesnostSifre[i] = $"GRESKA! {i + 1}. Specifikacija za sifrovanje nije dobro uneta";
                Console.WriteLine(uspesnostSifre[i]);
                continue;
            }
            //provera za unet kljuc
            operacije[i].kljuc = args[i + 3];
            if (!sifra1(operacije[i]))
            {
                uspesnostSifre[i] = $"GRESKA! {i + 1}. Kljuc nije dobro unet";
                Console.WriteLine(uspesnostSifre[i]);
                continue;
            }
            else if (operacije[i].sifrovanje == true)
            {
                Console.WriteLine("Uspesno izvrseno {0}. sifrovanje", i + 1);
            }
            else if (operacije[i].sifrovanje == false)
            {
                Console.WriteLine("Uspesno izvrseno {0}. desifrovanje", i + 1);
            }
        }
        for (int i = 0; i < uspesnostSifre.Length; i++)
        {
            if (uspesnostSifre[i] != null)
            {
                IspisRadaPrograma("");
                return;
            }
        }
    }
}