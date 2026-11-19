import { apiRequest } from "./apiClient";
import type {
  ServiceResponse,
  ServiceDetailsContent,
} from "../types/service";

export interface SaveServiceRequest {
  title: string;
  name: string;
  description: string;
  points: string[];
  titleAr: string;
  nameAr: string;
  descriptionAr: string;
  pointsAr: string[];
  details: ServiceDetailsContent;
}

export async function getServices(): Promise<ServiceResponse[]> {
  return apiRequest<ServiceResponse[]>("/api/Services");
}

export async function createService(
  request: SaveServiceRequest
): Promise<{ id: string }> {
  return apiRequest<{ id: string }>("/api/Services", {
    method: "POST",
    body: JSON.stringify(request),
  });
}

export async function updateService(
  id: string,
  request: SaveServiceRequest
): Promise<{ id: string; message: string }> {
  return apiRequest<{ id: string; message: string }>(
    `/api/Services/${id}`,
    {
      method: "PUT",
      body: JSON.stringify(request),
    }
  );
}
