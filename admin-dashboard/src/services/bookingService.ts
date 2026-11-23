import { apiRequest } from "./apiClient";

import type {
  BookingResponse,
  CreateBookingRequest,
  CreateBookingResponse,
} from "../types/booking";

export async function createBooking(
  request: CreateBookingRequest
): Promise<CreateBookingResponse> {
  return apiRequest<CreateBookingResponse>("/api/Bookings", {
    method: "POST",
    body: JSON.stringify(request),
  });
}

export async function getMyBookings(): Promise<BookingResponse[]> {
  return apiRequest<BookingResponse[]>("/api/Bookings/my");
}

export async function getAllBookings(): Promise<BookingResponse[]> {
  return apiRequest<BookingResponse[]>("/api/Bookings");
}

export type BookingAction = "confirm" | "start" | "complete" | "cancel";

export async function updateBookingStatus(
  id: string,
  action: BookingAction
): Promise<void> {
  await apiRequest<void>(`/api/Bookings/${id}/${action}`, {
    method: "PUT",
  });
}
