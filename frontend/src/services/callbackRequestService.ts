import { apiRequest } from "./apiClient";
import type {
  CallbackRequestResponse,
  CreateCallbackRequest,
  CallbackRequestStatus,
} from "../types/callbackRequest";

export async function createCallbackRequest(
  request: CreateCallbackRequest
): Promise<{ id: string; message: string }> {
  return apiRequest<{ id: string; message: string }>("/api/CallbackRequests", {
    method: "POST",
    body: JSON.stringify(request),
  });
}

export async function getCallbackRequests(): Promise<CallbackRequestResponse[]> {
  return apiRequest<CallbackRequestResponse[]>("/api/CallbackRequests");
}

export async function updateCallbackRequestStatus(
  id: string,
  status: CallbackRequestStatus
): Promise<void> {
  await apiRequest<void>(`/api/CallbackRequests/${id}/status`, {
    method: "PUT",
    body: JSON.stringify({ status }),
  });
}
