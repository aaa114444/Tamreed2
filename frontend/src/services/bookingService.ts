import { apiRequest } from "./apiClient";

import type {
  BookingResponse,
  CreateBookingRequest,
  CreateBookingResponse,
} from "../types/booking";

export async function createBooking(
  request: CreateBookingRequest
): Promise<CreateBookingResponse> {
  return apiRequest<CreateBookingResponse>(
    "/api/Bookings",
    {
      method: "POST",
      body: JSON.stringify(request),
    }
  );
}

export async function getMyBookings(): Promise<BookingResponse[]> {
  return apiRequest<BookingResponse[]>("/api/Bookings/my");
}
