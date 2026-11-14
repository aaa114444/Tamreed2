export interface CreateBookingRequest {
  serviceId: string;
  scheduledAtUtc: string;
  city: string;
  district: string;
  street: string;
  buildingNumber: string;
  apartmentNumber: string | null;
  notes: string | null;
}

export interface CreateBookingResponse {
  id: string;
  message: string;
}

export interface BookingResponse {
  id: string;
  customerId: string;
  serviceId: string;
  scheduledAtUtc: string;
  city: string;
  district: string;
  street: string;
  buildingNumber: string;
  apartmentNumber: string | null;
  notes: string | null;
  priceAtBooking: number;
  status: string;
  createdAtUtc: string;
}
