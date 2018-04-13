using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UdpBase
{
    public abstract  class UdpBaseClass
    {
        protected UdpClient Client;

        protected UdpBaseClass()
        {
            Client = new UdpClient();
        }

        
    }
}
