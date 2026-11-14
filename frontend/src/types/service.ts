import type { LucideIcon } from "lucide-react";

export type ServiceColor = "mint" | "blue" | "peach";

export interface ServiceDefinition {
  icon: LucideIcon;
  number: string;
  titleKey: string;
  descriptionKey: string;
  color: ServiceColor;
}

export interface ServiceResponse {
  id: string;
  name: string;
  description: string;
  price: number;
  status: string;
  createdAtUtc: string;
}
