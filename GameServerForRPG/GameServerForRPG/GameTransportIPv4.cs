using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace GameServerForRPG
{
    public class GameTransportIPv4 : IGameTransport
    {
        //dichiarazione di un canale
        private Socket socket;

        //costruttore di canale IPV4
        public GameTransportIPv4()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //non autobloccante
            socket.Blocking = false;
        }

        //metodo per mettersi in ascolto su un indirizzo
        public void Bind(string address, int port)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(address), port);
            socket.Bind(endPoint);
        }

        //metodo per ritornare pacchetti(byte[]) di grandezza stabilita(buffer), da un client(endpoint) 
        public byte[] Recv(int bufferSize, ref EndPoint sender)
        {
            //preparo il buffer massimo per contenere il pacchetto
            byte[] data = new byte[bufferSize];
            //???
            EndPoint tmpSender = new IPEndPoint(0, 0);

            //grandezza del pacchetto
            int returnLength = -1;

            try
            {
                //controlla la grandezza del pacchetto
                returnLength = socket.ReceiveFrom(data, ref sender);
            }
            catch
            {
                //se è bloccante(senza pacchetti nel socket) ritorna null
                return null;
            }

            //se è vuot ritorna null
            if (returnLength <= 0)
                return null;

            //altrimenti prepara un array grande come il pacchetto ricevuto
            byte[] newData = new byte[returnLength];
            //ci copia i dati ricevuti
            Buffer.BlockCopy(data, 0, newData, 0, returnLength);
            //ritorna l'array di byte ricevuto
            return newData;
        }

        //metodo dell'interfaccia 
        public EndPoint CreateEndPoint()
        {
            return new IPEndPoint(0, 0);
        }

        //metodo che ritorna falso quando la coda di pacchetti è piena 
        public bool Send(byte[] data, EndPoint destination)
        {
            bool success = false;

            try
            {
                //controlla il numero di byte inviati al socket se corrisponde alla grandezza del pacchetto
                int rlen = socket.SendTo(data, destination);
                if (rlen == data.Length)
                    success = true;
            }
            catch
            {
                success = false;
            }

            return false;
        }
    }
}
