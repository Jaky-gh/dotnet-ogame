using System;
using Ogame.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Ogame.Data
{
    public class PlanetRandomizer
    {
        private static readonly int primeNumber1 = 85571;
        private static readonly int primeNumber2 = 195997;
        private static readonly int primeNumber3 = 431621;
        private static readonly string[] firstNamePart =
        {
            "a", "e", "i", "o", "u",
            "ba", "be", "bi", "bo", "bu",
            "da", "de", "di", "do", "du",
            "fa", "fe", "fi", "fo", "fu",
            "ha", "he", "hi", "ho", "hu",
            "ia", "ie", "io", "iu",
            "ka", "ke", "ki", "ko", "ku",
            "la", "le", "li", "lo", "lu",
            "ma", "me", "mi", "mo", "mu",
            "na", "ne", "ni", "no", "nu",
            "pa", "pe", "pi", "po", "pu",
            "ra", "re", "ri", "ro", "ru",
            "sa", "se", "si", "so", "su",
            "ta", "te", "ti", "to", "tu",
            "va", "ve", "vi", "vo", "vu",
            "za", "ze", "zi", "zo", "zu",
        };
        private static readonly string[] lastnamePart =
        {
            "a", "e", "i", "o", "u",
            "ba", "be", "bi", "bo", "bu",
            "da", "de", "di", "do", "du",
            "fa", "fe", "fi", "fo", "fu",
            "ha", "he", "hi", "ho", "hu",
            "ia", "ie", "io", "iu",
            "ka", "ke", "ki", "ko", "ku",
            "la", "le", "li", "lo", "lu",
            "ma", "me", "mi", "mo", "mu",
            "na", "ne", "ni", "no", "nu",
            "pa", "pe", "pi", "po", "pu",
            "ra", "re", "ri", "ro", "ru",
            "sa", "se", "si", "so", "su",
            "ta", "te", "ti", "to", "tu",
            "va", "ve", "vi", "vo", "vu",
            "za", "ze", "zi", "zo", "zu",
            "ah", "eh", "ih", "oh", "uh",
            "bah", "beh", "bih", "boh", "buh",
            "dah", "deh", "dih", "doh", "duh",
            "fah", "feh", "fih", "foh", "fuh",
            "hah", "heh", "hih", "hoh", "huh",
            "iah", "ieh", "ioh", "iuh",
            "kah", "keh", "kih", "koh", "kuh",
            "lah", "leh", "lih", "loh", "luh",
            "mah", "meh", "mih", "moh", "muh",
            "nah", "neh", "nih", "noh", "nuh",
            "pah", "peh", "pih", "poh", "puh",
            "rah", "reh", "rih", "roh", "ruh",
            "sah", "seh", "sih", "soh", "suh",
            "tah", "teh", "tih", "toh", "tuh",
            "vah", "veh", "vih", "voh", "vuh",
            "zah", "zeh", "zih", "zoh", "zuh",
            "as", "es", "is", "os", "us",
            "bas", "bes", "bis", "bos", "bus",
            "das", "des", "dis", "dos", "dus",
            "fas", "fes", "fis", "fos", "fus",
            "has", "hes", "his", "hos", "hus",
            "ias", "ies", "ios", "ius",
            "kas", "kes", "kis", "kos", "kus",
            "las", "les", "lis", "los", "lus",
            "mas", "mes", "mis", "mos", "mus",
            "nas", "nes", "nis", "nos", "nus",
            "pas", "pes", "pis", "pos", "pus",
            "ras", "res", "ris", "ros", "rus",
            "sas", "ses", "sis", "sos", "sus",
            "tas", "tes", "tis", "tos", "tus",
            "vas", "ves", "vis", "vos", "vus",
            "zas", "zes", "zis", "zos", "zus",
            "ar", "er", "ir", "or", "ur",
            "bar", "ber", "bir", "bor", "bur",
            "dar", "der", "dir", "dor", "dur",
            "far", "fer", "fir", "for", "fur",
            "har", "her", "hir", "hor", "hur",
            "iar", "ier", "ior", "iur",
            "kar", "ker", "kir", "kor", "kur",
            "lar", "ler", "lir", "lor", "lur",
            "mar", "mer", "mir", "mor", "mur",
            "nar", "ner", "nir", "nor", "nur",
            "par", "per", "pir", "por", "pur",
            "rar", "rer", "rir", "ror", "rur",
            "sar", "ser", "sir", "sor", "sur",
            "tar", "ter", "tir", "tor", "tur",
            "var", "ver", "vir", "vor", "vur",
            "zar", "zer", "zir", "zor", "zur",
            "al", "el", "il", "ol", "ul",
            "bal", "bel", "bil", "bol", "bul",
            "dal", "del", "dil", "dol", "dul",
            "fal", "fel", "fil", "fol", "ful",
            "hal", "hel", "hil", "hol", "hul",
            "ial", "iel", "iol", "iul",
            "kal", "kel", "kil", "kol", "kul",
            "lal", "lel", "lil", "lol", "lul",
            "mal", "mel", "mil", "mol", "mul",
            "nal", "nel", "nil", "nol", "nul",
            "pal", "pel", "pil", "pol", "pul",
            "ral", "rel", "ril", "rol", "rul",
            "sal", "sel", "sil", "sol", "sul",
            "tal", "tel", "til", "tol", "tul",
            "val", "vel", "vil", "vol", "vul",
            "zal", "zel", "zil", "zol", "zul",
            "ab", "eb", "ib", "ob", "ub",
            "bab", "beb", "bib", "bob", "bub",
            "dab", "deb", "dib", "dob", "dub",
            "fab", "feb", "fib", "fob", "fub",
            "hab", "heb", "hib", "hob", "hub",
            "iab", "ieb", "iob", "iub",
            "kab", "keb", "kib", "kob", "kub",
            "lab", "leb", "lib", "lob", "lub",
            "mab", "meb", "mib", "mob", "mub",
            "nab", "neb", "nib", "nob", "nub",
            "pab", "peb", "pib", "pob", "pub",
            "rab", "reb", "rib", "rob", "rub",
            "sab", "seb", "sib", "sob", "sub",
            "tab", "teb", "tib", "tob", "tub",
            "vab", "veb", "vib", "vob", "vub",
            "zab", "zeb", "zib", "zob", "zub",
             "az", "ez", "iz", "oz", "uz",
            "baz", "bez", "biz", "boz", "buz",
            "daz", "dez", "diz", "doz", "duz",
            "faz", "fez", "fiz", "foz", "fuz",
            "haz", "hez", "hiz", "hoz", "huz",
            "iaz", "iez", "ioz", "iuz",
            "kaz", "kez", "kiz", "koz", "kuz",
            "laz", "lez", "liz", "loz", "luz",
            "maz", "mez", "miz", "moz", "muz",
            "naz", "nez", "niz", "noz", "nuz",
            "paz", "pez", "piz", "poz", "puz",
            "raz", "rez", "riz", "roz", "ruz",
            "saz", "sez", "siz", "soz", "suz",
            "taz", "tez", "tiz", "toz", "tuz",
            "vaz", "vez", "viz", "voz", "vuz",
            "zaz", "zez", "ziz", "zoz", "zuz"
        };

        public static async Task<Planet> GetExistingOrRandomPlanet(ApplicationDbContext context, int X, int Y)
        {
            var planet = context != null ?  await context.Planets.Include( m => m.Defenses).FirstOrDefaultAsync(p => p.X == X && p.Y == Y) : null;
            if (planet != null)
            {
                return planet;
            }
            return GetRandomPlanet(X, Y);
        }

        public static Planet GetRandomPlanet(int X, int Y)
        {
            Random random = new Random(primeNumber1 + X * primeNumber2 + Y * primeNumber3);
            int distToStar = random.Next(50, 10001);
            int percentage = random.Next(101);
            int numSylabe = percentage < 10 ? 1 : percentage < 50 ? 2 : percentage < 70 ? 3 : percentage < 90 ? 4 : 5;
            string name = "";
            for (int i = 0; i < numSylabe; i++)
            {
                if (i == numSylabe - 1)
                {
                    name += random.Next(4) > 0 ? firstNamePart[random.Next(firstNamePart.Length)] : lastnamePart[random.Next(lastnamePart.Length)];
                    if (random.Next(101) > 15)
                    {
                        name += '-' + random.Next(100).ToString();
                    }
                }
                else
                {
                    name += firstNamePart[random.Next(firstNamePart.Length)];
                }
            }
            var planet = new Planet { X = X, Y = Y, Cristal = 1000, Energy = 1000, Dist_to_star = distToStar, Deuterium = 100, Metal = 1000, Name = name };
            return planet;
        }
    }
}
