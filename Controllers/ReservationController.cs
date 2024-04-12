﻿using dogsitting_backend.ApplicationServices;
using dogsitting_backend.Domain;
using dogsitting_backend.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Org.BouncyCastle.Utilities.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dogsitting_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservationController : ControllerBase
    {
        private ReservationService ReservationService;
        public ReservationController(ReservationService reservationService)
        {
            this.ReservationService = reservationService;
        }


        [HttpGet(Name = "GetReservation")]
        public async Task<ActionResult> Get()
        {
            Calendar test = await this.ReservationService.GetReservations(null);
            test.GetBusyEvents();
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            string json = JsonConvert.SerializeObject(test, settings);
            return Ok(json);
        }



        [HttpPost(Name = "PostReservation")]
        public async Task<ActionResult> Post(Reservation reservation)
        {
            return Ok();
        }
    }
}
