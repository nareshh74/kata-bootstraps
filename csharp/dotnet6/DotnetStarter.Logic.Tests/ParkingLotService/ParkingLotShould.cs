using System;
using System.Collections.Generic;
using System.IO;
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
                Assert.Equal("Parked vehicle. Ticket ID: PR1234_1_1", ticket.ToString());
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
                Assert.Equal(parkingLot.GetFreeSlots(vehicleType), beforeParking);

                // Arrange
                parkingLot.Park(vehicle);

                // Act and Assert
                Assert.Equal(parkingLot.GetFreeSlots(vehicleType), afterParking);
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
                Assert.Equal(parkingLot.GetOccupiedSlots(vehicleType), beforeParking);

                // Arrange
                parkingLot.Park(new Vehicle("ABC123", vehicleType, "Black"));

                // Act and Assert
                Assert.Equal(parkingLot.GetOccupiedSlots(vehicleType), afterParking);
            }
        }
    }

    // running all tests in this class at once, will fail because of the Console.WriteLine statements
    public class ParkingLotWithDisplayShould
    {
        public class ParkShould
        {
            [Fact]
            public void Print_ticket_as_expected_when_parked()
            {
                // Arrange
                var parkingLot = new ParkingLot("PR1234", 1, 1);
                var parkingLotWithDisplay = new ParkingLotWithDisplay(parkingLot);
                var vehicle = new Vehicle("ABC123", VehicleType.Truck, "Black");
                var expected = "Parked vehicle. Ticket ID: PR1234_1_1";

                using StringWriter sw = new();
                Console.SetOut(sw);

                // Act
                parkingLotWithDisplay.Park(vehicle);

                // Assert
                var result = sw.ToString();
                Assert.True(expected == result.Trim(), $"Expected: {expected}, Actual: {result}");
            }

            [Fact]
            public void Print_ticket_as_expected_when_slot_full()
            {
                // Arrange
                var parkingLot = new ParkingLot("PR1234", 1, 1);
                var parkingLotWithDisplay = new ParkingLotWithDisplay(parkingLot);
                var vehicle = new Vehicle("XYZ123", VehicleType.Truck, "Black");
                var expected = "Parking Lot Full";
                parkingLotWithDisplay.Park(new Vehicle("ABC123", VehicleType.Truck, "Black"));

                using StringWriter sw = new();
                Console.SetOut(sw);

                // Act
                parkingLotWithDisplay.Park(vehicle);

                // Assert
                var result = sw.ToString();
                Assert.True(expected == result.Trim(), $"Expected: {expected}, Actual: {result}");
            }

            [Theory]
            [InlineData(VehicleType.Car)]
            [InlineData(VehicleType.Bike)]
            public void Print_ticket_as_expected_when_vehicletype_not_compatible(VehicleType vehicleType)
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
            public void Print_vehicle_when_unparked()
            {
                // Arrange
                var parkingLot = new ParkingLot("PR1234", 1, 1);
                var parkingLotWithDisplay = new ParkingLotWithDisplay(parkingLot);
                var vehicle = new Vehicle("ABC123", VehicleType.Truck, "Black");
                var ticket = parkingLotWithDisplay.Park(vehicle);
                using StringWriter sw = new();
                Console.SetOut(sw);
                var expected = $"Unparked vehicle with Registration Number: {vehicle.RegistrationNumber} and Color: {vehicle.Color}";

                // Act
                parkingLotWithDisplay.Unpark(ticket);

                // Assert
                var result = sw.ToString().Trim();
                Assert.True(expected == result, $"Expected: {expected}, Actual: {result}");
            }

            [Fact]
            public void Print_as_expected_when_ticket_invalid()
            {
                // Arrange
                var parkingLot = new ParkingLot("PR1234", 1, 1);
                var parkingLotWithDisplay = new ParkingLotWithDisplay(parkingLot);
                using StringWriter sw = new();
                Console.SetOut(sw);
                const string expected = "Invalid Ticket";

                // Act
                parkingLotWithDisplay.Unpark(Ticket.Create("PR1234", 1, 1));

                // Assert
                var result = sw.ToString().Trim();
                Assert.True(expected == result, $"Expected: {expected}, Actual: {result}");
            }
        }

        public class GetFreeSlotCountShould
        {
            public static IEnumerable<object[]> ReturnExpectedFreeSlotCountParameters =>
                new List<object[]>
                {
                    new object[] { VehicleType.Truck, @"No. of free slots for TRUCK on Floor 1: 1
No. of free slots for TRUCK on Floor 2: 1", @"No. of free slots for TRUCK on Floor 1: 0
No. of free slots for TRUCK on Floor 2: 1" },
                    new object[] { VehicleType.Bike, @"No. of free slots for BIKE on Floor 1: 2
No. of free slots for BIKE on Floor 2: 2", @"No. of free slots for BIKE on Floor 1: 1
No. of free slots for BIKE on Floor 2: 2" },
                    new object[] { VehicleType.Car, @"No. of free slots for CAR on Floor 1: 3
No. of free slots for CAR on Floor 2: 3", @"No. of free slots for CAR on Floor 1: 2
No. of free slots for CAR on Floor 2: 3" }
                };

            [Theory, MemberData(nameof(GetFreeSlotCountShould.ReturnExpectedFreeSlotCountParameters))]
            public void Print_expected_free_slot_count(VehicleType vehicleType, string beforeParking, string afterParking)
            {
                // Arrange
                const int floorCount = 2;
                var lot = new ParkingLot("PR1234", floorCount, 6);
                var lotWithDisplay = new ParkingLotWithDisplay(lot);
                using (StringWriter sw = new())
                {
                    Console.SetOut(sw);

                    // Assert
                    lotWithDisplay.GetFreeSlotCount(vehicleType);
                    var result = sw.ToString().Trim();
                    Assert.True(beforeParking == result, $"Expected: {beforeParking}, Actual: {result}");

                    // Arrange
                    lotWithDisplay.Park(new Vehicle("ABC123", vehicleType, "Black"));
                }

                using (StringWriter sw = new())
                {
                    Console.SetOut(sw);

                    // Act
                    lotWithDisplay.GetFreeSlotCount(vehicleType);

                    // Assert
                    var result = sw.ToString().Trim();
                    Assert.True(afterParking == result, $"Expected: {afterParking}, Actual: {result}");
                }
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
                        @"Free slots for TRUCK on Floor 1: 1
Free slots for TRUCK on Floor 2: 1",
                        @"Free slots for TRUCK on Floor 1: 
Free slots for TRUCK on Floor 2: 1"
                    },
                    new object[]
                    {
                        VehicleType.Car,
                        @"Free slots for CAR on Floor 1: 4,5,6
Free slots for CAR on Floor 2: 4,5,6",
                        @"Free slots for CAR on Floor 1: 5,6
Free slots for CAR on Floor 2: 4,5,6"
                    },
                    new object[]
                    {
                        VehicleType.Bike,
                        @"Free slots for BIKE on Floor 1: 2,3
Free slots for BIKE on Floor 2: 2,3",
                        @"Free slots for BIKE on Floor 1: 3
Free slots for BIKE on Floor 2: 2,3"
                    }
                };
            [Theory, MemberData(nameof(GetFreeSlotShould.ReturnExpectedFreeSlotParameters))]
            public void Return_free_slot_as_expected(VehicleType vehicleType,
                string beforeParking,
                string afterParking)
            {
                // Arrange
                var lot = new ParkingLot("PR1234", 2, 6);
                var lotWithDisplay = new ParkingLotWithDisplay(lot);
                var vehicle = new Vehicle("ABC123", vehicleType, "Black");

                using (StringWriter sw = new())
                {
                    Console.SetOut(sw);

                    // Act
                    lotWithDisplay.GetFreeSlots(vehicleType);
                    var result = sw.ToString().Trim();

                    // Assert
                    Assert.True(beforeParking == result, $"Expected: {beforeParking}, Actual: {result}");

                    // Arrange
                    lotWithDisplay.Park(vehicle);
                }

                using (StringWriter sw = new())
                {
                    Console.SetOut(sw);

                    // Act
                    lotWithDisplay.GetFreeSlots(vehicleType);
                    var result = sw.ToString().Trim();

                    // Assert
                    Assert.True(afterParking == result, $"Expected: {afterParking}, Actual: {result}");
                }
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
                        @"Occupied slots for TRUCK on Floor 1: 
Occupied slots for TRUCK on Floor 2: ",
                        @"Occupied slots for TRUCK on Floor 1: 1
Occupied slots for TRUCK on Floor 2: "
                    },
                    new object[]
                    {
                        VehicleType.Car,
                        @"Occupied slots for CAR on Floor 1: 
Occupied slots for CAR on Floor 2: ",
                        @"Occupied slots for CAR on Floor 1: 4
Occupied slots for CAR on Floor 2: "
                    },
                    new object[]
                    {
                        VehicleType.Bike,
                        @"Occupied slots for BIKE on Floor 1: 
Occupied slots for BIKE on Floor 2: ",
                        @"Occupied slots for BIKE on Floor 1: 2
Occupied slots for BIKE on Floor 2: "
                    }
                };

            [Theory, MemberData(nameof(GetOccupiedSlotsShould.ReturnExpectedOccupiedSlotsParameters))]
            public void Return_occupied_slots_as_expected(VehicleType vehicleType, string beforeParking,
                string afterParking)
            {
                // Arrange
                var lot = new ParkingLot("PR1234", 2, 6);
                var lotWithDisplay = new ParkingLotWithDisplay(lot);

                using (StringWriter sw = new())
                {
                    Console.SetOut(sw);

                    // Act
                    lotWithDisplay.GetOccupiedSlots(vehicleType);
                    var result = sw.ToString().Trim();

                    // Assert
                    Assert.True(beforeParking.Trim() == result, $"Expected: {beforeParking}, Actual: {result}");

                    // Arrange
                    lotWithDisplay.Park(new Vehicle("ABC123", vehicleType, "Black"));
                }

                using (StringWriter sw = new())
                {
                    Console.SetOut(sw);

                    // Act
                    lotWithDisplay.GetOccupiedSlots(vehicleType);
                    var result = sw.ToString().Trim();

                    // Assert
                    Assert.True(afterParking.Trim() == result, $"Expected: {afterParking}, Actual: {result}");
                }
            }
        }
    }
}