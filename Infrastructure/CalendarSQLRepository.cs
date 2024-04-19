using dogsitting_backend.ApplicationServices;
using dogsitting_backend.Domain;
using dogsitting_backend.Domain.calendar;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dogsitting_backend.Infrastructure
{
    public class CalendarSQLRepository
    {
        public DogsittingDBContext context { get; set; }

        public CalendarSQLRepository(DogsittingDBContext context)
        {
            this.context = context;
        }

        public async Task<List<Calendar>> GetCalendarByTeam(string team)
        {
            return await this.context.Calendars.Include("Team").Where(calendar => calendar.Team.Name == team).ToListAsync();
        }

        public async Task<List<Reservation>> GetReservationsByCalendarIdAsync(Guid calendarId)
        {
            return await this.context.Reservations.Where(e => e.CalendarId == calendarId).ToListAsync();
        }

        public async Task<List<Reservation>> GetReservationsByUserIdAsync(Guid userId)
        {
            return await this.context.Reservations.Where(e => e.Client.Id == userId).ToListAsync();
        }


        public async Task<Object> Create(Reservation reservation)
        {
            this.context.Reservations.Add(reservation);
            await this.context.SaveChangesAsync();
            return reservation;
        }

    }
}
