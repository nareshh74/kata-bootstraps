using System;
using System.Collections.Generic;
using System.Linq;

namespace DotnetStarter.Logic.ParkingLotService
{
    public class ParkingLotWithDisplay
    {
        private readonly ParkingLot _parkingLot;

        public ParkingLotWithDisplay(string id, int floors, int slotsPerFloor)
        {
            this._parkingLot = new ParkingLot(id, floors, slotsPerFloor);
            Console.WriteLine($"Created parking lot with {floors} floors and {slotsPerFloor} slots per floor");
        }

        public Ticket Park(Vehicle vehicle)
        {
            var ticket = this._parkingLot.Park(vehicle);
            Console.WriteLine($"{ticket}");
            return ticket;
        }

        public Vehicle Unpark(Ticket ticket)
        {
            var vehicle = this._parkingLot.Unpark(ticket);
            Console.WriteLine(vehicle == null ? "Invalid Ticket" : $"Unparked vehicle with Registration Number: {vehicle.RegistrationNumber} and Color: {vehicle.Color}");
            return vehicle;
        }

        public Dictionary<int, int> GetFreeSlotCount(VehicleType vehicleType)
        {
            var freeSlotCount = this._parkingLot.GetFreeSlotCount(vehicleType);
            foreach (var kvp in freeSlotCount)
            {
                Console.WriteLine($"No. of free slots for {vehicleType.ToString().ToUpper()} on Floor {kvp.Key}: {kvp.Value}");
            }
            return freeSlotCount;
        }

        public Dictionary<int, List<int>> GetFreeSlots(VehicleType vehicleType)
        {
            var freeSlots = this._parkingLot.GetFreeSlots(vehicleType);
            foreach (var kvp in freeSlots)
            {
                Console.WriteLine($"Free slots for {vehicleType.ToString().ToUpper()} on Floor {kvp.Key}: {string.Join(",", kvp.Value)}");
            }
            return freeSlots;
        }

        public Dictionary<int, List<int>> GetOccupiedSlots(VehicleType vehicleType)
        {
            var occupiedSlots = this._parkingLot.GetOccupiedSlots(vehicleType);
            foreach (var kvp in occupiedSlots)
            {
                Console.WriteLine($"Occupied slots for {vehicleType.ToString().ToUpper()} on Floor {kvp.Key}: {string.Join(",", kvp.Value)}");
            }
            return occupiedSlots;
        }
    }

    public class ParkingLot
    {
        private readonly string _id;
        private readonly List<ParkingFloor> _floors;
        private HashSet<Ticket> _issuedTickets;
        private static object _lockObject = new object();

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
            lock (ParkingLot._lockObject)
            {
                var slot = this.GetAvailableSlotForParking(vehicle);
                slot?.Park(vehicle);
                var ticket = Ticket.Create(this._id, slot?.FloorId, slot?.Id);
                this._issuedTickets.Add(ticket);
                return ticket;
            }
        }

        public Vehicle Unpark(Ticket ticket)
        {
            if (!this._issuedTickets.Contains(ticket))
            {
                return null;
            }
            var slotId = ticket.SlotId ?? 0;
            var floorId = ticket.FloorId ?? 0;
            var slot = this._floors[floorId - 1].GetSlot(slotId);
            var vehicle = slot.Unpark();
            this._issuedTickets.Remove(ticket);
            return vehicle;
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

            public ParkingSlot GetSlot(int slotId) => this._slots[slotId - 1];

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

            public List<int> GetSlots(VehicleType vehicleType, bool isOccupied)
            {
                return this._slots
                    .Where(slot => slot.IsOccupied() == isOccupied
                                   && slot.OfType(vehicleType))
                    .Select(slot => slot.Id)
                    .ToList();
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
                return this.OfType(vehicle.VehicleType);
            }

            public bool OfType(VehicleType vehicleType)
            {
                // get vehicle type based on slot id
                var compatibleVehicleType = this.Id == 1
                    ? VehicleType.Truck
                    : this.Id < 4
                        ? VehicleType.Bike
                        : VehicleType.Car;
                return compatibleVehicleType == vehicleType;
            }

            public Vehicle Unpark()
            {
                var vehicle = this._vehicle;
                this._vehicle = null;
                return vehicle;
            }
        }

        public Dictionary<int, int> GetFreeSlotCount(VehicleType vehicleType)
        {
            return this.GetSlots(vehicleType, false).ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Count);
        }

        public Dictionary<int, List<int>> GetFreeSlots(VehicleType vehicleType)
        {
            return this.GetSlots(vehicleType, false);
        }

        public Dictionary<int, List<int>> GetOccupiedSlots(VehicleType vehicleType)
        {
            return this.GetSlots(vehicleType, true);
        }

        private Dictionary<int, List<int>> GetSlots(VehicleType vehicleType, bool isOccupied)
        {
            return this._floors.ToDictionary(floor => floor.Id, floor => floor.GetSlots(vehicleType, isOccupied));
        }
    }

    public class Vehicle
    {
        public VehicleType VehicleType { get; }
        public string Color { get; }
        public string RegistrationNumber { get; }

        public Vehicle(string licensePlate, VehicleType vehicleType, string color)
        {
            this.VehicleType = vehicleType;
            this.Color = color;
            this.RegistrationNumber = licensePlate;
        }
    }

    public enum VehicleType {
        Bike,
        Car,
        Truck
    }

    public class Ticket
    {
        public static Ticket SlotFull => new Ticket(null, null, null);

        public static Ticket Create(string lotId, int? floorId, int? slotId)
        {
            return slotId == null ? Ticket.SlotFull : new Ticket(lotId, floorId, slotId);
        }

        private Ticket(string lotId, int? floorId, int? slotId)
        {
            this.Lotid = lotId;
            this.FloorId = floorId;
            this.SlotId = slotId;
        }

        public int? FloorId { get; }
        public int? SlotId { get; }

        public string Lotid { get; }

        public override string ToString()
        {
            return this.SlotId == null ? "Parking Lot Full" : $"Parked vehicle. Ticket ID: {this.Lotid}_{this.FloorId}_{this.SlotId}";
        }
    }
}
