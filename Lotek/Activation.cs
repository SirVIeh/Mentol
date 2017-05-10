using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lotek
{
    [Serializable]
    public class Activation
    {
        //jeżeli napis "davon" to działa jak inny to wyświetla ten napis z pliku i nie działa a jeżeli będzie zapłacone, to napis "egal" powoduje, że ściągnie ten plik i już wiecej nie będzie chciał się łączyć online
        public string Activated { get; set; }
    }
}
