using DotnetStarter.Logic.ParkingLotService;
using Xunit;

namespace DotnetStarter.Logic.Tests.ParkingLotService
{
    public class ParkingLotShould
    {
        public class ParkShould
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
                Assert.Equal("PR1234_1_1", ticket.ToString());
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

        public class UnParkShould
        {
            [Fact]
            public void Allow_to_unpark_car_for_valid_ticket()
            {
                // Arrange
                var parkingLot = new ParkingLot("PR1234", 1, 1);
                var vehicle = new Vehicle("ABC123", VehicleType.Truck, "Black");
                var ticket = parkingLot.Park(vehicle);

                // Act
                var unparkedVehicle = parkingLot.Unpark(ticket);

                // Assert
                Assert.NotNull(unparkedVehicle);
                Assert.Equal(vehicle, unparkedVehicle);
            }

            [Fact]
            public void Not_allow_to_unpark_car_when_ticket_is_invalid()
            {
                // Arrange
                var parkingLot = new ParkingLot("PR1234", 1, 1);

                // Act
                var unparkedVehicle = parkingLot.Unpark(Ticket.Create("PR1234", 1, 1));

                // Assert
                Assert.Null(unparkedVehicle);
            }
        }
    }
}