using System.Collections.Generic;
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

        public class GetFreeSlotCountShould
        {
            public static IEnumerable<object[]> ReturnExpectedFreeSlotCountParameters =>
                new List<object[]>
                {
                    new object[] { VehicleType.Truck, new int[] { 1, 1 }, new int[] { 0, 1 } },
                    new object[] { VehicleType.Bike, new int[] { 2, 2 }, new int[] { 1, 2 } },
                    new object[] { VehicleType.Car, new int[] { 3, 3 }, new int[] { 2, 3 } }
                };

            [Theory, MemberData(nameof(GetFreeSlotCountShould.ReturnExpectedFreeSlotCountParameters))]
            public void Return_expected_free_slot_count(VehicleType vehicleType, int[] beforeParking, int[] afterParking)
            {
                // Arrange
                const int floorCount = 2;
                var lot = new ParkingLot("PR1234", floorCount, 6);
                var expectedBeforeParkingResult = new Dictionary<int, int>();
                for (var i = 0; i < floorCount; i++)
                {
                    expectedBeforeParkingResult.Add(i + 1, beforeParking[i]);
                }
                var expectedAfterParkingResult = new Dictionary<int, int>();
                for (var i = 0; i < floorCount; i++)
                {
                    expectedAfterParkingResult.Add(i + 1, afterParking[i]);
                }

                // Assert
                Assert.Equal(lot.GetFreeSlotCount(vehicleType), expectedBeforeParkingResult);

                // Arrange
                lot.Park(new Vehicle("ABC123", vehicleType, "Black"));

                // Act
                Assert.Equal(lot.GetFreeSlotCount(vehicleType), expectedAfterParkingResult);
            }
        }

        public class GetFreeSlotShould
        {
            public static IEnumerable<object[]> ReturnExpectedFreeSlotParameters =>
                new List<object[]>
                {
                    new object[]
                    {
                        VehicleType.Truck,
                        new Dictionary<int, List<int>>()
                        {
                            { 1, new List<int>(){ 1 } },
                            { 2, new List<int>() { 1 } }
                        },
                        new Dictionary<int, List<int>>()
                        {
                            { 1, new List<int>() },
                            { 2, new List<int>() { 1 } }
                        }
                    },
                    new object[]
                    {
                        VehicleType.Bike,
                        new Dictionary<int, List<int>>()
                        {
                            { 1, new List<int>(){ 2, 3 } },
                            { 2, new List<int>() { 2, 3 } }
                        },
                        new Dictionary<int, List<int>>()
                        {
                            { 1, new List<int>(){ 3 } },
                            { 2, new List<int>() { 2, 3 } }
                        },
                    },
                    new object[]
                    {
                        VehicleType.Car,
                        new Dictionary<int, List<int>>()
                        {
                            { 1, new List<int>(){ 4, 5, 6 } },
                            { 2, new List<int>() { 4, 5, 6 } }
                        },
                        new Dictionary<int, List<int>>()
                        {
                            { 1, new List<int>(){ 5, 6 } },
                            { 2, new List<int>() { 4, 5, 6 } }
                        }
                    }
                };
            [Theory, MemberData(nameof(GetFreeSlotShould.ReturnExpectedFreeSlotParameters))]
            public void Return_free_slot_as_expected(VehicleType vehicleType,
                Dictionary<int, List<int>> beforeParking,
                Dictionary<int, List<int>> afterParking)
            {
                // Arrange
                var parkingLot = new ParkingLot("PR1234", 2, 6);
                var vehicle = new Vehicle("ABC123", vehicleType, "Black");

                // Act and Assert
                Assert.Equal(parkingLot.GetFreeSlot(vehicleType), beforeParking);

                // Arrange
                parkingLot.Park(vehicle);

                // Act and Assert
                Assert.Equal(parkingLot.GetFreeSlot(vehicleType), afterParking);
            }
        }

        public class GetOccupiedSlotsShould
        {
            public static IEnumerable<object[]> ReturnExpectedOccupiedSlotsParameters =>
                new List<object[]>
                {
                    new object[]
                    {
                        VehicleType.Truck,
                        new Dictionary<int, List<int>>()
                        {
                            { 1, new List<int>() },
                            { 2, new List<int>() }
                        },
                        new Dictionary<int, List<int>>()
                        {
                            { 1, new List<int>(){ 1 } },
                            { 2, new List<int>() }
                        }
                    },
                    new object[]
                    {
                        VehicleType.Bike,
                        new Dictionary<int, List<int>>()
                        {
                            { 1, new List<int>() },
                            { 2, new List<int>()  }
                        },
                        new Dictionary<int, List<int>>()
                        {
                            { 1, new List<int>(){ 2 } },
                            { 2, new List<int>() }
                        }
                    },
                    new object[]
                    {
                        VehicleType.Car,
                        new Dictionary<int, List<int>>()
                        {
                            { 1, new List<int>() },
                            { 2, new List<int>() }
                        },
                        new Dictionary<int, List<int>>()
                        {
                            { 1, new List<int>(){ 4 } },
                            { 2, new List<int>() }
                        }
                    }
                };
            [Theory, MemberData(nameof(GetOccupiedSlotsShould.ReturnExpectedOccupiedSlotsParameters))]
            public void Return_occupied_slots_as_expected(VehicleType vehicleType, Dictionary<int, List<int>> beforeParking, Dictionary<int, List<int>> afterParking)
            {
                // Arrange
                var parkingLot = new ParkingLot("PR1234", 2, 6);

                // Act and Assert
                Assert.Equal(parkingLot.GetOccupiedSlots(), beforeParking);

                // Arrange
                parkingLot.Park(new Vehicle("ABC123", vehicleType, "Black"));

                // Act and Assert
                Assert.Equal(parkingLot.GetOccupiedSlots(), afterParking);
            }
        }
    }
}