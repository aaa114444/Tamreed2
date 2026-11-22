export type CallbackRequestStatus = "New" | "Contacted" | "Completed";

export interface CallbackRequestResponse {
  id: string;
  name: string;
  mobileNumber: string;
  servicesNeeded: string[];
  message: string | null;
  status: CallbackRequestStatus;
  createdAtUtc: string;
  updatedAtUtc: string | null;
}

export interface CreateCallbackRequest {
  name: string;
  mobileNumber: string;
  servicesNeeded: string[];
  message?: string;
  website?: string;
}
