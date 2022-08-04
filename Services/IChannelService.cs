using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_XML.Services
{
    public interface IChannelService
    {
        Task<Channel> AsyncRead();
        Task<Items> ReadReg();

        //Экспорт
        Task<Items[]> toTxt();
        Task<Items[]> toDocx();
        Task<Items[]> toXls();

    }
}
