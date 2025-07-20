using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Zoom
{
    public interface IZoomRepository
    {
        Task<Response<SessionAuthorizeResponse>> StartZoomCall();
    }
}
