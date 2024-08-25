using System.Collections.Generic;

namespace DotnetStarter.Logic.ParkingLotService
{
    public class ParkingLot
    {
        private readonly string _id;
        private readonly List<ParkingFloor> _floors;
        private HashSet<Ticket> _issuedTickets;

        public ParkingLot(string id, int floorCount, int slotsPerFloor)
        {
            this._id = id;
            this._floors = new List<ParkingFloor>();
            this._issuedTickets = new HashSet<Ticket>();
            for(int i = 0; i < floorCount; i++)
            {
                this._floors.Add(new ParkingFloor(this._id, i + 1, slotsPerFloor));
            }
        }

        public Ticket Park(Vehicle vehicle)
        {
            var slot = this.GetAvailableSlotForParking(vehicle);
            slot?.Park(vehicle);
            var ticket = Ticket.Create(this._id, slot?.FloorId, slot?.Id);
            this._issuedTickets.Add(ticket);
            return ticket;
        }

        private ParkingSlot GetAvailableSlotForParking(Vehicle vehicle)
        {
            foreach (var floor in this._floors)
            {
                var slot = floor.GetAvailableSlotForParking(vehicle);
                if (slot != null)
                {
                    return slot;
                }
            }
            return null;
        }

        internal class ParkingFloor
        {
            public string LotId { get; }
            public int Id { get; }
            private readonly List<ParkingSlot> _slots;

            public ParkingFloor(string lotId, int id, int slotsPerFloor)
            {
                this.LotId = lotId;
                this.Id = id;
                this._slots = new List<ParkingSlot>();
                for (int i = 0; i < slotsPerFloor; i++)
                {
                    this._slots.Add(new ParkingSlot(this.Id, i + 1));
                }
            }

            public ParkingSlot GetAvailableSlotForParking(Vehicle vehicle)
            {
                foreach (var slot in this._slots)
                {
                    if (slot.CanParkVehicle(vehicle))
                    {
                        return slot;
                    }
                }
                return null;
            }
        }

        internal class ParkingSlot
        {
            public int FloorId { get; }
            public int Id { get; }
            private Vehicle _vehicle;

            public ParkingSlot(int floorId, int id)
            {
                this.FloorId = floorId;
                this.Id = id;
            }

            public void Park(Vehicle vehicle)
            {
                this._vehicle = vehicle;
            }

            public bool IsOccupied()
            {
                return this._vehicle != null;
            }

            public bool CanParkVehicle(Vehicle vehicle)
            {
                if(this.IsOccupied())
                {
                    return false;
                }
                // get vehicle type based on slot id
                var compatibleVehicleType = this.Id == 1
                    ? VehicleType.Truck
                    : this.Id < 4
                        ? VehicleType.Bike
                        : VehicleType.Car;
                return vehicle.VehicleType == compatibleVehicleType;
            }
        }
    }

    public class Vehicle
    {
        public VehicleType VehicleType { get; }

        public Vehicle(string licensePlate, VehicleType vehicleType, string color)
        {
            this.VehicleType = vehicleType;
        }
    }

    public enum VehicleType {
        Bike,
        Car,
        Truck
    }

    public class Ticket
    {
        public string Id { get; }
        public static Ticket SlotFull => new Ticket(null, null, null);

        public static Ticket Create(string lotId, int? floorId, int? slotId)
        {
            return slotId == null ? Ticket.SlotFull : new Ticket(lotId, floorId, slotId);
        }

        private Ticket(string lotId, int? floorId, int? slotId)
        {
            this.Id = $"{lotId}_{floorId}_{slotId}";
        }

        public override string ToString()
        {
            return this.Id;
        }
    }
}
