﻿using TicketingAppBackEnd.Models;
using TicketingAppBackEnd.Sql.Interfaces;
using TicketingLib.Model;

namespace TicketingAppBackEnd.Sql
{
    public class BookingRepository : IBookingRepository
    {
        private readonly DevContext _context; 

        public BookingRepository(DevContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Booking booking)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int bookingId)
        {
            var booking =await _context.Bookings.FindAsync(bookingId);
            if (booking != null) _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
        }

        public List<Booking> GetAll()
        {
            return _context.Bookings.ToList();
        }

        public Booking GetById(int bookingId)
        {
            return _context.Bookings.Find(bookingId);
        }

        public async Task UpdateAsync(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }
    }
}
