using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Band.WebAPI.Controllers
{
    public class ButtonCountsController : ApiController
    {


        public static ButtonData _DATA = new ButtonData() { Blue = 5, Green = 4, Red = 3, Yellow = 2};

        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok(_DATA);
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody] string color)
        {
            switch (color.ToUpper())
            {
                case "RED":
                    _DATA.Red++;
                    break;
                case "GREEN":
                    _DATA.Green++;
                    break;
                case "BLUE":
                    _DATA.Blue++;
                    break;
                case "YELLOW":
                    _DATA.Yellow++;
                    break;
            }

            return Ok();
        }
    }


    public class ButtonData
    {
        public long Red { get; set; }
        public long Green { get; set; }
        public long Blue { get; set; }
        public long Yellow { get; set; }
    }
}
