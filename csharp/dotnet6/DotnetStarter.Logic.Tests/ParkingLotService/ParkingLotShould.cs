using DotnetStarter.Logic.ParkingLotService;
using Xunit;

namespace DotnetStarter.Logic.Tests.ParkingLotService
{
    public class ParkingLotShould
    {
        [Fact]
        public void Allow_to_park_car_and_return_ticket_as_expected()
        {
            // Arrange
            var parkingLot = new ParkingLot("PR1234", 1, 1);
            var vehicle = new Vehicle("ABC123", VehicleType.Truck, "Black");
            
            // Act
            var ticket = parkingLot.Park(vehicle);

            // Assert
            Assert.NotNull(ticket);
            Assert.Equal("PR1234_1_1", ticket.Id);
        }

        [Fact]
        public void Not_allow_to_park_car_when_no_slot_available()
        {
            // Arrange
            var parkingLot = new ParkingLot("PR1234", 1, 1);
            var vehicle = new Vehicle("XYZ123", VehicleType.Truck, "Black");
            parkingLot.Park(new Vehicle("ABC123", VehicleType.Truck, "Black"));

            // Act
            var ticket = parkingLot.Park(vehicle);

            // Assert
            Assert.Equal(ticket.ToString(), Ticket.SlotFull.ToString());
        }

        [Theory]
        [InlineData(VehicleType.Car)]
        [InlineData(VehicleType.Bike)]
        public void Not_allow_to_park_car_when_no_slot_available_for_vehicle_type(VehicleType vehicleType)
        {
            // Arrange
            var parkingLot = new ParkingLot("PR1234", 1, 1);
            var vehicle = new Vehicle("XYZ123", vehicleType, "Black");

            // Act
            var ticket = parkingLot.Park(vehicle);

            // Assert
            Assert.Equal(ticket.ToString(), Ticket.SlotFull.ToString());
        }
    }
}