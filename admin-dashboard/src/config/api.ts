const configuredBaseUrl = import.meta.env.VITE_API_BASE_URL?.trim();

export const API_BASE_URL = configuredBaseUrl
  ? configuredBaseUrl.replace(/\/+$/, "")
  : "";

console.log("[Admin API] VITE_API_BASE_URL:", import.meta.env.VITE_API_BASE_URL);
console.log("[Admin API] API_BASE_URL:", API_BASE_URL);
