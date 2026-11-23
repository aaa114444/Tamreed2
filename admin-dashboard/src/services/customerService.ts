import { apiRequest } from "./apiClient";
import type { CustomerResponse } from "../types/customer";

export async function getCustomers(search?: string): Promise<CustomerResponse[]> {
  const query = search?.trim()
    ? `?search=${encodeURIComponent(search.trim())}`
    : "";

  return apiRequest<CustomerResponse[]>(`/api/Customers${query}`);
}

export async function setCustomerActiveStatus(
  customerId: string,
  isActive: boolean
): Promise<void> {
  return apiRequest<void>(
    `/api/Customers/${customerId}/status?isActive=${isActive}`,
    { method: "PATCH" }
  );
}
