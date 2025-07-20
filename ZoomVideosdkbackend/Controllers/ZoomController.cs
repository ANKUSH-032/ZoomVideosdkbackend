using Core.Zoom;
using Microsoft.AspNetCore.Mvc;

namespace ZoomVideosdkbackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZoomController : ControllerBase
    {
        private readonly IZoomRepository _zoomRepository;
        public ZoomController(IZoomRepository zoomRepository) { 
        _zoomRepository = zoomRepository;
        }

        [HttpPost("startcall")]
        public async Task<IActionResult> StartZoomCall()
        {
            var result = await _zoomRepository.StartZoomCall().ConfigureAwait(false);

            if (result.Status)
            {
                return Ok(result); // HTTP 200 with result body
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
        }
    }
}
