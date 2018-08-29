using System.Collections.Generic;

namespace ApiControllers.Models
{
    public class MemoryRepository : IRepository
    {
        private readonly Dictionary<int, Reservation> items;

        public MemoryRepository()
        {
            items = new Dictionary<int, Reservation>();
            new List<Reservation>
            {
                new Reservation {ClientName = "Alice", Location = "Board Room"},
                new Reservation {ClientName = "Bob", Location = "Lecture Hall"},
                new Reservation {ClientName = "Joe", Location = "Meeting Room 1"}
            }.ForEach(r => AddReservation(r));
        }

        public IEnumerable<Reservation> Reservations => items.Values;

        public Reservation this[int id] => items.ContainsKey(id) ? items[id] : null;

        public Reservation AddReservation(Reservation reservation)
        {
            if (reservation.ReservationId == 0)
            {
                var key = items.Count;
                while (items.ContainsKey(key)) { key++; }
                reservation.ReservationId = key;
            }

            items[reservation.ReservationId] = reservation;

            return reservation;
        }

        public Reservation UpdateReservation(Reservation reservation) => AddReservation(reservation);

        public void DeleteReservation(int id) => items.Remove(id);
    }
}