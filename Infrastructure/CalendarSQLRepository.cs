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

        public async Task<Object> Create(Reservation reservation)
        {
            this.context.Reservations.Add(reservation);
            await this.context.SaveChangesAsync();
            return reservation;
        }

        public async Task<List<Availability>> GetCalendarAvailabilities(Guid id)
        {
            return await this.context.Availabilities.Where(Availability => Availability.CalendarId == id).ToListAsync();
        }


        public async Task AddAvailabilities(List<Availability> availabilities)
        {
            await this.context.Availabilities.AddRangeAsync(availabilities);
            this.context.SaveChanges();
        }

        public async Task<List<Availability>> FindAvailabilities(Guid calendarId, List<DateTime> dates)
        {
            return await this.context.Availabilities.Where(availability => dates.Contains(availability.DateFrom) && availability.CalendarId == calendarId).ToListAsync();
        }

        public async Task DeleteAvailabilities(List<Availability> availabilities)
        {
            this.context.Availabilities.RemoveRange(availabilities);
            this.context.SaveChanges();
        }
    }
}
