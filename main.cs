using System;
using System.Text;
using System.IO;

class Program
{
    struct Operacija
    {
        public string datoteka;
        public int brSifre;
        public bool sifrovanje; 
        public string kljuc;
    }
    static bool Sifra1(Operacija operacija)
    {
        StreamWriter sw;
        StreamReader sr;
        int[] kljuc = new int[operacija.kljuc.Length];
        if (kljuc.Length > 50)
            return false; // duzina kljuca ne treba biti veca od 50
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
                    else temp[i] = Convert.ToChar(temp[i] + kljuc[i % kljuc.Length] - 256);//na 32 mestu je prvi karakter koji nije neka "komanda"
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
                    else temp[i] = Convert.ToChar(temp[i] - kljuc[i % kljuc.Length] + 256);
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
        int[] kljuc = new int[operacija.kljuc.Length];
        // kljuc u nizu
        if (kljuc.Length > 50)
            return false; // duzina kljuca ne treba biti veca od 50
        sr = new StreamReader(operacija.datoteka);
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
                    if (kljucZbir + brojacPi == 99999)
                    {
                        kljucZbir = 0;
                        brojacPi = 0;
                    }
                    if (temp[i] + Convert.ToChar(piNiz[kljucZbir + brojacPi] - 48) > 255)
                        temp[i] -= Convert.ToChar(256); //ako karakter izadje iz opsega ascii koda(127) umanjice se za ukupan broj ascii koda i krenuce od prvog karaktera (33)   
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
                    if (kljucZbir + brojacPi == 99999)
                    {
                        kljucZbir = 0;
                        brojacPi = 0;
                    }
                    if (temp[i] - Convert.ToChar(piNiz[kljucZbir + brojacPi] - 48) < 0)
                        temp[i] += Convert.ToChar(256); //ako karakter izadje iz opsega ascii koda(127) uvecace se za ukupan broj ascii koda i krenuce od prvog karaktera (33) 
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
        if(!int.TryParse(operacija.kljuc, out kljuc) || kljuc>255)
          return false; // duzina kljuca ne treba biti veca od 50
        
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
                        sw.Write(Convert.ToChar(temp[i] + kljuc - 256));
                    }
                    else if (temp[i] + kljuc < 0)
                    {
                        sw.Write(Convert.ToChar(temp[i] + kljuc + 256));
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
                        sw.Write(Convert.ToChar(temp[i] - kljuc - 256));
                    }
                    else if (temp[i] - kljuc < 0)
                    {
                        sw.Write(Convert.ToChar(temp[i] - kljuc + 256));
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
  
  static void IspisRadaPrograma()
    {
        Console.WriteLine("{0,27}","RAD PROGRAMA");
        Console.WriteLine("-U komandnu liniju argumenata je neophodno\nuneti po 4 validna podatka o sifrovanju za svaku\ndatoteku razdvojena jednim blanko znakom.");
        Console.WriteLine("-1. argument je naziv datoteke.");
        Console.WriteLine("-2. argument je broj sifre koju zelite da koristite.\nUnesite broj '1', '2' ili '3'.");
        Console.WriteLine("-3. argument je izbor izmedju sifrovanja i desifrovanja.\nUnesite slovo 's' ili 'd'.");
        Console.WriteLine("-4. argument je kljuc za sifrovanje. Kljuc mora biti manji\nod 50 karaktera. U slucaju trece sifre kljuc mora biti samo\nbroj, a u slucaju prve i druge mogu biti i slova.");
    Console.WriteLine("NAPOMENA: Program moze da sifruje samo karaktere u opsegu\nextended ASCII. U suprotnom ce doci do greske.");
        Console.WriteLine("-Pokrenite program ponovo i ispravno unesite podatke.");
    }
    static void Main(string[] args)
        {
            Operacija[] operacije;
            bool[] uspesnostSifre;
            if (args.Length % 4 != 0 || args.Length == 0)
            {
                IspisRadaPrograma();
                return;
            }
            else
            {
                operacije = new Operacija[args.Length/4];
                uspesnostSifre = new bool[args.Length / 4];
            }
            int indeksArgs = 0;
            int indeksOperacije = 0;
            while (indeksArgs < args.Length)
            {
                //Provera za naziv datoteke
                if (!File.Exists(args[indeksArgs]))
                {
                    uspesnostSifre[indeksOperacije] = true;
                    Console.WriteLine($"GRESKA! {indeksOperacije + 1}. Fajl nije dobro unet");
                    indeksArgs += 4-(indeksArgs % 4);
                    indeksOperacije++;
                    continue;
                }
                operacije[indeksOperacije].datoteka = args[indeksArgs];
                //Provera za nacin sifrovanja
                indeksArgs ++;
                if (!int.TryParse(args[indeksArgs], out operacije[indeksOperacije].brSifre) || operacije[indeksOperacije].brSifre > 3 || operacije[indeksOperacije].brSifre < 1)
                {
                    uspesnostSifre[indeksOperacije] = true;
                    Console.WriteLine($"GRESKA! {indeksOperacije + 1}. Broj sifre nije dobro unet");
                    indeksArgs += 4 - (indeksArgs % 4);
                    indeksOperacije++;
                    continue;
                }
                //Provera za sifrovanje/desifrovanje
                indeksArgs ++;
                if (args[indeksArgs] == "s") operacije[indeksOperacije].sifrovanje = true;
                else if(args[indeksArgs] == "d") operacije[indeksOperacije].sifrovanje= false;
                else
                {
                    uspesnostSifre[indeksOperacije] = true;
                    Console.WriteLine($"GRESKA! {indeksOperacije + 1}. Specifikacija za sifrovanje nije dobro uneta");
                    indeksArgs += 4 - (indeksArgs % 4);
                    indeksOperacije++;
                    continue;
                }
                //Provera za broj sifre
                indeksArgs ++;
                operacije[indeksOperacije].kljuc = args[indeksArgs];

                if (operacije[indeksOperacije].brSifre == 1?!Sifra1(operacije[indeksOperacije]) :
                   operacije[indeksOperacije].brSifre == 2?!Sifra2(operacije[indeksOperacije]):
                   !Sifra3(operacije[indeksOperacije]))
                {
                    uspesnostSifre[indeksOperacije] = true;
                    Console.WriteLine($"GRESKA! {indeksOperacije + 1}. Kljuc nije dobro unet");
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
                if (uspesnostSifre[i])
                {
                    IspisRadaPrograma();
                    return;
                }
            }
        }
}