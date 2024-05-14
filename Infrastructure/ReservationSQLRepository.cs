using dogsitting_backend.ApplicationServices;
using dogsitting_backend.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dogsitting_backend.Infrastructure
{
    public class ReservationSQLRepository
    {
        public DogsittingDBContext context { get; set; }

        public ReservationSQLRepository(DogsittingDBContext context)
        {
            this.context = context;
        }

        public async Task<Reservation> FindById(Guid Id)
        {
            return await this.context.Reservations.Where(reservation => reservation.Id == Id).FirstAsync();
        }

        public async Task<List<Reservation>> GetAllReservationsAsync()
        {
            return await this.context.Reservations.Include("Team").ToListAsync();
        }

        public async Task<List<Reservation>> GetReservationsByCalendarIdAsync(Guid calendarId)
        {
            return await this.context.Reservations.Where(e => e.CalendarId == calendarId).ToListAsync();
        }

        public async Task<List<Reservation>> GetReservationsByUserIdAsync(Guid userId)
        {
            return await this.context.Reservations.Include(reserv => reserv.Calendar).ThenInclude(calendar => calendar.Team).ThenInclude(team => team.Admins).Where(e => e.Client.Id == userId).ToListAsync();
        }

        public async Task<List<Reservation>> GetReservationsByTeamIdAsync(Guid teamId)
        {
            return await this.context.Reservations.Include(reserv => reserv.Calendar).ThenInclude(cal => cal.Team).ThenInclude(team => team.Admins).Where(e => e.Calendar.Team.Id == teamId).ToListAsync();
        }


        public async Task<Object> Create(Reservation reservation)
        {
            this.context.Reservations.Add(reservation);
            await this.context.SaveChangesAsync();
            return reservation;
        }

        public async Task<Object> Delete(Reservation reservation)
        {
            this.context.Reservations.Remove(reservation);
            await this.context.SaveChangesAsync();
            return reservation;
        }

    }
}
