using System;
using System.Text;
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
    static bool Sifra1(Operacija operacija)
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
                    if (temp[i] + kljuc[i % kljuc.Length] < 255) temp[i] = Convert.ToChar(temp[i] + kljuc[i % kljuc.Length]);//za sifrovanje povecam ascii kod svakog karaktera za njegovo odgovarajuce mesto u kljucu
                    else temp[i] = Convert.ToChar(temp[i] + kljuc[i % kljuc.Length] - 255 + 32);//na 32 mestu je prvi karakter koji nije neka "komanda"
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
                    else temp[i] = Convert.ToChar(temp[i] - kljuc[i % kljuc.Length] + 255 - 32);
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
                    if (temp[i] + Convert.ToChar(piNiz[kljucZbir + brojacPi] - 48) > 255)
                        temp[i] -= Convert.ToChar(255 - 32); //ako karakter izadje iz opsega ascii koda(127) umanjice se za ukupan broj ascii koda i krenuce od prvog karaktera (33)   
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
                    if (temp[i] + Convert.ToChar(piNiz[kljucZbir + brojacPi] - 48) < 255)
                        temp[i] += Convert.ToChar(255 - 32); //ako karakter izadje iz opsega ascii koda(127) uvecace se za ukupan broj ascii koda i krenuce od prvog karaktera (33) 
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

    static bool Sifra3(Operacija operacija)
    {
        StreamReader sr;
        StreamWriter sw;
        sr = new StreamReader(operacija.datoteka);
        //kljuc
        int kljuc; 
        if(!int.TryParse(operacija.kljuc, out kljuc) || kljuc>255){
          return false;
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
                    if (temp[i] + kljuc > 255)
                    {
                        sw.Write(Convert.ToChar(temp[i] + kljuc - 255));
                    }
                    else if (temp[i] + kljuc < 0)
                    {
                        sw.Write(Convert.ToChar(temp[i] + kljuc + 255));
                    }
                    else { sw.Write(Convert.ToChar(temp[i] + kljuc)); }
                   
                }
                sw.WriteLine();
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
                    if (temp[i] - kljuc > 255)
                    {
                        sw.Write(Convert.ToChar(temp[i] - kljuc - 255));
                    }
                    else if (temp[i] - kljuc < 0)
                    {
                        sw.Write(Convert.ToChar(temp[i] - kljuc + 255));
                    }
                    else { sw.Write(Convert.ToChar(temp[i] - kljuc)); }

                }
                sw.WriteLine();
            }
            sw.Close();
            sr.Close();
        }
        return true;
    }
  
  static void IspisRadaPrograma(string greska)//Po pozivu uneti koja se greska desila
    {
        Console.WriteLine("program radi tako sto:");
        Console.WriteLine(" ");
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
                operacije = new Operacija[args.Length/4];
                uspesnostSifre = new string[args.Length / 4];
            }
            int indeksArgs = 0;
            int indeksOperacije = 0;
            while (indeksArgs < args.Length)
            {
                //Provera za naziv datoteke
                if (!args[indeksArgs].EndsWith(".txt") || !File.Exists(args[indeksArgs]))
                {
                    uspesnostSifre[indeksOperacije] = $"GRESKA! {indeksOperacije + 1}. Fajl nije dobro unet";
                    Console.WriteLine(uspesnostSifre[indeksOperacije]);
                    indeksArgs += 4-(indeksArgs % 4);
                    indeksOperacije++;
                    continue;
                }
                operacije[indeksOperacije].datoteka = args[indeksArgs];
                //Provera za nacin sifrovanja
                indeksArgs += 1;
                if (!int.TryParse(args[indeksArgs], out operacije[indeksOperacije].brSifre) || operacije[indeksOperacije].brSifre > 3 || operacije[indeksOperacije].brSifre < 1)
                {
                    uspesnostSifre[indeksOperacije] = $"GRESKA! {indeksOperacije + 1}. Broj sifre nije dobro unet";
                    Console.WriteLine(uspesnostSifre[indeksOperacije]);
                    indeksArgs += 4 - (indeksArgs % 4);
                    indeksOperacije++;
                    continue;
                }
                //Provera za sifrovanje/desifrovanje
                indeksArgs += 1;
                if (args[indeksArgs] == "a") operacije[indeksOperacije].sifrovanje = true;//ovo mozemo i da stavimo da bude s kao skraceno od sifrovanje
                else if(args[indeksArgs] == "b") operacije[indeksOperacije].sifrovanje= false;//ovo mozemo i da stavimo da bude d kao skraceno od desifrovanje
                else
                {
                    uspesnostSifre[indeksOperacije] = $"GRESKA! {indeksOperacije + 1}. Specifikacija za sifrovanje nije dobro uneta";
                    Console.WriteLine(uspesnostSifre[indeksOperacije]);
                    indeksArgs += 4 - (indeksArgs % 4);
                    indeksOperacije++;
                    continue;
                }
                //provera za unet kljuc
                indeksArgs += 1;
                operacije[indeksOperacije].kljuc = args[indeksArgs];
                if (operacije[indeksOperacije].kljuc == "1"?!Sifra1(operacije[indeksOperacije]) :
                   operacije[indeksOperacije].kljuc == "2"?!Sifra2(operacije[indeksOperacije]):
                   !Sifra3(operacije[indeksOperacije]))
                {
                    uspesnostSifre[indeksOperacije] = $"GRESKA! {indeksOperacije + 1}. Kljuc nije dobro unet";
                    Console.WriteLine(uspesnostSifre[indeksOperacije]);
                    indeksArgs += 4 - (indeksArgs % 4);
                    indeksOperacije++;
                    continue;
                }
                else if(operacije[indeksOperacije].sifrovanje == true)
                {
                    Console.WriteLine("Uspesno izvrseno {0}. sifrovanje", indeksOperacije + 1);
                }
                else if (operacije[indeksOperacije].sifrovanje == false)
                {
                    Console.WriteLine("Uspesno izvrseno {0}. desifrovanje", indeksOperacije + 1);
                }
                indeksArgs++;
                indeksOperacije++;
            }
            for (int i = 0; i < uspesnostSifre.Length; i++)
            {
                if (uspesnostSifre[i]!=null)
                {
                    IspisRadaPrograma("");
                    return;
                }
            }
        }
}