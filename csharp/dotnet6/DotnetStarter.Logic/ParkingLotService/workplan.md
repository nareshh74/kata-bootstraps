**Parking lot**

https://workat.tech/machine-coding/practice/design-parking-lot-qm6hwq4wkhp8

API
 - create_parking_lot
Created parking lot with <no_of_floors> floors and <no_of_slots_per_floor> slots per floor

 - park_vehicle
Parked vehicle. Ticket ID: <ticket_id>
Print "Parking Lot Full" if slot is not available for that vehicle type.

 - unpark_vehicle
Unparked vehicle with Registration Number: <reg_no> and Color: <color>
Print "Invalid Ticket" if ticket is invalid or parking slot is not occupied.

 - display free_count <vehicle_type>
No. of free slots for <vehicle_type> on Floor <floor_no>: <no_of_free_slots>
The above will be printed for each floor.

 - display free_slots <vehicle_type>
Free slots for <vehicle_type> on Floor <floor_no>: <comma_separated_values_of_slot_nos>
The above will be printed for each floor.

 - display occupied_slots <vehicle_type>
Occupied slots for <vehicle_type> on Floor <floor_no>: <comma_separated_values_of_slot_nos>
The above will be printed for each floor.

**data**
 - lot id
 - floor id
 - slot id
 - slots per floor
 - floors per lot
 - slots to parked vehicle mapping
 - vehicle info

**Behavior**
 - handle parking lot full
 - handle invalid ticket
 - first available slot should be used for parking
 - vehicle type should be considered for parking
 - slot allocation should be done based on vehicle type

 **optional**
 - additional vehicle types and slot types - *seperate type for maintaining vehicle types*
 - different strategies for slot allocation - *strategy pattern for slot allocation*
 - multiple parking lots with different i/p and o/p formats - *seperate class for dealing with i/p and o/p - polymorphism*
 - thread safety - *risk of using same slot for multiple vehicles - use locking*


**Design**

ParkingLot
 - string id
 - List<ParkingLotFloor> floors
 - Set<Ticket> tickets
 - ParkVehicle(vehicle) -> Ticket
     - GetAvilableSlotForParking(vehicle)
     - slot?.ParkVehicle(vehicle)
     - add ticket to tickets
     - return ticket
 - UnParkVehicle(ticket) -> void
     - check if ticket is in tickets
	 - slot?.UnParkVehicle()
	 - remove ticket from tickets
 - private GetAvilableSlotForParking(vehicle) -> ParkingSlot
     - for each floor in floors:
         - slot = floor.GetAvilableSlotForParking(vehicle)
         - if slot != null return slot
     - return null
 - DisaplyFreeCount(vehicleType) -> Map<int, int>
	 - for each floor in floors:
		 - count = floor.GetFreeCount(vehicleType)
		 - map.Add(floor.id, count)
	 - return map
 - DisaplyFreeSlots(vehicleType) -> Map<int, List<int>>
    - for each floor in floors:
        - slots = floor.GetFreeSlots(vehicleType)
        - map.Add(floor.id, slots)
    - return map
- DisaplyOccupiedSlots(vehicleType) -> Map<int, List<int>>
    - for each floor in floors:
        - slots = floor.GetOccupiedSlots(vehicleType)
        - map.Add(floor.id, slots)
    - return map

ParkingLotFloor
 - int id
 - string lotId
 - List<ParkingSlot> slots
 - private GetAvilableSlotForParking(vehicle) -> ParkingSlot
    - for each slot in slots:
        - if !slot.isOccupied() && slot.GetVehicleType() == vehicle.type
        - return slot
    - return null
 - GetFreeCount(vehicleType) -> int
    - for each slot in slots:
	    - if !slot.isOccupied() && slot.GetVehicleType() == vehicleType
	    - count++
    - return count
- GetFreeSlots(vehicleType) -> List<int>
    - for each slot in slots:
        - if !slot.isOccupied() && slot.GetVehicleType() == vehicleType
        - slots.Add(slot.id)
    - return slots
- GetOccupiedSlots(vehicleType) -> List<int>
	- for each slot in slots:
		- if slot.isOccupied() && slot.GetVehicleType() == vehicleType
		- slots.Add(slot.id)
	- return slots


ParkingSlot
 - int id
 - int floorId
 - Vehicle vehicle
 - isOccupied() -> bool
 - ParkVehicle(vehicle) -> void
 - UnParkVehicle() -> void
 - GetVehicleType() -> VehicleType
     - return VehicleType.GetVehicleTypeForTheSlot(id)

Vehicle
 - string regNo
 - string color
 - VehicleType type

VehicleType
- GetVehicleTypeForTheSlot(slotId) -> VehicleType

Ticket
 - ctor(ParkingSlot)
 - string id
 - ToString() -> id
 - special ticket if parking lot full