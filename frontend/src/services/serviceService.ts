import { apiRequest } from "./apiClient";
import type { ServiceResponse } from "../types/service";

export async function getServices(): Promise<ServiceResponse[]> {
  return apiRequest<ServiceResponse[]>("/api/Services");
}
